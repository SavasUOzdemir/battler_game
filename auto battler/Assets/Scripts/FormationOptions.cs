using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FormationOptions : MonoBehaviour
{
    [SerializeField] GameObject meleeWedge;
    [SerializeField] GameObject meleeSaw;
    [SerializeField] GameObject meleeSquare;
    [SerializeField] GameObject rangedSaw;
    [SerializeField] GameObject rangedDispersed;
    Company company;

    void UpdateFormationOptions()
    {
        if (company != null)
        {
            if (company.selected)
            {
                if (company.MeleeCompany)
                {
                    meleeWedge.SetActive(true);
                    meleeSaw.SetActive(true);
                    meleeSquare.SetActive(true);
                }

                if (company.RangedCompany)
                {
                    rangedSaw.SetActive(true);
                    rangedDispersed.SetActive(true);
                }
            }
        }

    }

   
    

}
