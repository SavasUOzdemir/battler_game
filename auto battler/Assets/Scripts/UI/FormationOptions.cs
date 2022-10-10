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
    [SerializeField] Company_UI company_ui;
    [SerializeField] Company company;

    public Company Company 
    {
        get
        {
            return company;
        }

        set
        {
            company = value;
        } 
    }

    void UpdateFormationOptions()
    {
        if (company_ui != null)
        {
            if (company_ui.selected)
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
