using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        UpdateThumbnails();
    }
    private void UpdateThumbnails()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<HoldSquad>().Squad != null)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = transform.GetChild(i).GetComponent<HoldSquad>().Squad.Sprite;
            }
            else
                transform.GetChild(i).GetComponent<Image>().sprite = null;
        }
    }
}
