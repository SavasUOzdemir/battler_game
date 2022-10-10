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
    GameObject[] formationOpts = new GameObject[5];

    private void Awake()
    {
        formationOpts[0] = meleeWedge;
        formationOpts[1] = meleeSaw;
        formationOpts[2] = meleeSquare;
        formationOpts[3] = rangedSaw;
        formationOpts[4] = rangedDispersed;
    }

    public Company_UI Company_UI 
    {
        get
        {
            return company_ui;
        }

        set
        {
            company_ui = value;
        } 
    }
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
        foreach (GameObject go in formationOpts)
        {
            go.SetActive(false);
        }

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
