using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    List<UnitAction> actionList = new List<UnitAction>();
    public bool busy = false;
    Unit unit;
    Animator animator;
    AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
    }

    void Update()
    {
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
    }

    private void Move(Vector3 target)
    {
        StopAllCoroutines();
        animator.Play("Idle");
        unit.SetTarget(target);
        busy = true;
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
