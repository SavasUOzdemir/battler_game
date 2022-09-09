using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Actor : MonoBehaviour
{

    List<UnitAction> actionList = new List<UnitAction>();
    public bool busy = false;

    Animator animator;
    AnimatorOverrideController animatorOverrideController;
    Attributes attributes;
    Vector3 aiDest;
    Company company;
    float brainLag = 0.2f;
    float brainTime = 0;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        attributes = GetComponent<Attributes>();
        aiDest = GetComponent<AIDestinationSetter>().targetVector;
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

        if (!busy)
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
        }

        attributes.SetFacing(company.GetFacing());
    }

    private void Move(Vector3 target)
    {
        StopAllCoroutines();
        animator.Play("Idle");
        aiDest = target;
        busy = true;
        Debug.Log("Move Commanded"); //Move'a þu an girmiyor.
    }

    public void EndMovement()
    {
        busy = false;
    }

    public void AddUnitAction(System.Type type)
    {
        actionList.Add(gameObject.AddComponent(type) as UnitAction);
        actionList.Sort((x, y) => x.getPrio().CompareTo(y.getPrio()));
    }

}
