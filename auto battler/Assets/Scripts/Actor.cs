using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public bool busy = false;
    public bool moving = false;

    List<UnitAction> actionList = new List<UnitAction>();
    UnitAction meleeAction = null;
    Unit unit;
    Animator animator;
    AnimatorOverrideController animatorOverrideController;
    Attributes attributes;
    Company company;
    float brainLag = 0.2f;
    float brainTime = 0;
    GameObject[] buffer = new GameObject[500];

    private void Awake()
    {
        unit = GetComponent<Unit>();
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        attributes = GetComponent<Attributes>(); 
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
        brainTime = 0f;
        
        if (company.InMelee())
        {
            if (!meleeAction)
            {
                Debug.Log("No Melee Action");
                return;
            }
            MeleeBehaviour();
        }
        else
        {
            NotMeleeBehaviour();
        }
    }

    private void Move(Vector3 target)
    {
        busy = false;
        StopAllCoroutines();
        animator.Play("Idle");
        unit.SetTarget(target);
        moving = true;
    }

    private GameObject FindClosestEnemyModel()
    {
        Utils.UnitsInRadius(transform.position, 10, buffer);
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
        if ((target.transform.position - transform.position).sqrMagnitude <= meleeAction.GetRange()*meleeAction.GetRange())
            return true;
        return false;
    }

    private void MeleeBehaviour()
    {
        GameObject closestModel = FindClosestEnemyModel();
        if (!closestModel || busy)
            return;
        if (IsModelInRange(closestModel))
        {
            unit.EndMove();
            StartCoroutine(meleeAction.DoAction());
            busy = true;
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
            if (action.getCurrentCooldown() <= 0)
            {
                StartCoroutine(action.DoAction());
                busy = true;
                break;
            }
        }
        attributes.SetFacing(company.GetFacing());
    }

    public void EndMovement()
    {
        moving = false;
    }

    public void AddUnitAction(System.Type type)
    {
        UnitAction unitAction = gameObject.AddComponent(type) as UnitAction;
        if (unitAction.IsActionMelee())
        {
            meleeAction = unitAction;
            return;
        }
        actionList.Add(gameObject.AddComponent(type) as UnitAction);
        actionList.Sort((x, y) => x.getPrio().CompareTo(y.getPrio()));
    }

}
