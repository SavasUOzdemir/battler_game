using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    List<UnitAction> actionList = new List<UnitAction>();
    public bool busy = false;

    // Start is called before the first frame update
    void Start()
    {
        Component[] actions = GetComponents(typeof(UnitAction));
        foreach(UnitAction action in actions)
        {
            actionList.Add(action);
        }
        actionList.Sort((x,y) => x.getPrio().CompareTo(y.getPrio()));
    }

    // Update is called once per frame
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

}
