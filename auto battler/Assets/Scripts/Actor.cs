using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    List<UnitAction> actionList = new List<UnitAction>();
    public bool busy = false;
    Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    void Start()
    {
        Component[] actions = GetComponents(typeof(UnitAction));
        foreach(UnitAction action in actions)
        {
            actionList.Add(action);
        }
        actionList.Sort((x,y) => x.getPrio().CompareTo(y.getPrio()));
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
        unit.SetTarget(target);
        busy = true;
    }

    public void EndMovement()
    {
        busy = false;
    }

    public void AddUnitAction()
    {
        //TODO
    }

}
