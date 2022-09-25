using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Actor : MonoBehaviour
{
    public bool busy = false;
    public bool moving = false;

    List<UnitAction> actionList = new List<UnitAction>();
    UnitAction meleeWeapon = null;
    UnitAction rangedWeapon = null;
    Animator animator;
    AnimatorOverrideController animatorOverrideController;
    Attributes attributes;
    AIDestinationSetter aiDest;
    Company company;
    float brainLag = 0.2f;
    float brainTime = 0;
    GameObject[] buffer = new GameObject[500];
    Vector3 currentTargetPos;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        attributes = GetComponent<Attributes>();
        aiDest = GetComponent<AIDestinationSetter>();
    }

    void Start()
    {
        company = attributes.GetCompany();
    }

    void Update()
    {
        if (brainTime < brainLag)
        {
            brainTime += Time.deltaTime;
            return;
        }
        if (attributes.GetWinded())
            return;
        brainTime = 0f;

        if (company.Moving() || busy)
            return;

        if (company.InMelee())
        {
            if (!meleeWeapon)
            {
                return;
            }
            MeleeBehaviour();
        }
        else if (IsInPosition()) 
        {
            NotMeleeBehaviour();
        }
    }

    //TODO:: REWRITE
    public void Move(Vector3 target)
    {
        if (attributes.GetWinded())
            return;
        currentTargetPos = target;
        busy = false;
        StopAllCoroutines();
        animator.Play("Idle");
        moving = true;
        aiDest.targetVector = target;
    }

    bool IsInPosition()
    {
        if ((transform.position - currentTargetPos).sqrMagnitude < 0.2f)
            return true;
        return false;
    }

    private GameObject FindClosestEnemyModel()
    {
        BattlefieldManager.ModelsInRadius(transform.position, 20, buffer);
        float distSqr = Mathf.Infinity;
        Vector3 distVector;
        GameObject target = null;
        foreach (GameObject model in buffer)
        {
            if (!model || model.GetComponent<Attributes>().GetTeam() == attributes.GetTeam())
                continue;
            distVector = model.transform.position - transform.position;
            if (distSqr > distVector.sqrMagnitude)
            {
                distSqr = distVector.sqrMagnitude;
                target = model;
            }
        }
        return target;
    }

    private bool IsModelInRange(GameObject target)
    {
        if (!target || !target.transform)
            return false;
        if ((target.transform.position - transform.position).sqrMagnitude <= meleeWeapon.GetRange()*meleeWeapon.GetRange())
            return true;
        return false;
    }

    private void MeleeBehaviour()
    {
        foreach (UnitAction action in actionList)
        {
            if (action.GetCurrentCooldown() <= 0 && action.FindTargets())
            {
                StartCoroutine(action.DoAction());
                busy = true;
                attributes.SetFacing(company.GetFacing());
                return;
            }
        }
        GameObject closestModel = FindClosestEnemyModel();
        if (!closestModel || busy)
            return;
        if (IsModelInRange(closestModel) && meleeWeapon.FindTargets())
        {
            StartCoroutine(meleeWeapon.DoAction());
            busy = true;
            moving = false;
        }
        else
        {
            Move(closestModel.transform.position);
        }
    }

    private void NotMeleeBehaviour()
    {
        foreach (UnitAction action in actionList)
        {
            if (action.GetCurrentCooldown() <= 0 && action.FindTargets())
            {
                StartCoroutine(action.DoAction());
                busy = true;
                attributes.SetFacing(company.GetFacing());
                return;
            }
        }
        if (rangedWeapon == null)
            return;
        if(rangedWeapon.GetCurrentCooldown() <= 0 && rangedWeapon.FindTargets())
        {
            StartCoroutine(rangedWeapon.DoAction());
            busy = true;
            attributes.SetFacing(company.GetFacing());
        }
    }

    public void EndMovement()
    {
        moving = false;
    }

    public void AddUnitAction(System.Type type)
    {
        UnitUpgrade unitUpgrade = gameObject.AddComponent(type) as UnitUpgrade;
        if(unitUpgrade is UnitAction)
        {
            if((unitUpgrade as UnitAction).IsMainWeapon())
            {
                if ((unitUpgrade as UnitAction).IsActionMelee())
                {
                    meleeWeapon = unitUpgrade as UnitAction;
                    return;
                }
                else
                {
                    rangedWeapon = unitUpgrade as UnitAction;
                }
            }          
            actionList.Add(unitUpgrade as UnitAction);
            actionList.Sort((x, y) => x.GetPrio().CompareTo(y.GetPrio()));
        }
        else if(unitUpgrade is UnitPassive)
        {
            attributes.AddUnitPassive(unitUpgrade as UnitPassive);
        }
    }
}
