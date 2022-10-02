using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.TooltipUI;


public class SquadPlacement : MonoBehaviour
{
    [SerializeField] Squad[] squads = new Squad[10];
    bool defaultPlacement = true;
    
    private void OnEnable()
    {
        if (defaultPlacement)
        {
            int i = 0;
            foreach (Squad squad in squads)
            {
                transform.GetChild(i).GetComponent<HoldSquad>().Squad = squad;
                i++;
            }
        }
        else if (!defaultPlacement)
        {

        }
    }
}
