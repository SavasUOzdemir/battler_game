using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FormationOptions : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject meleeWedge;
    [SerializeField] GameObject meleeSaw;
    [SerializeField] GameObject meleeSquare;
    [SerializeField] GameObject rangedSquare;
    [SerializeField] GameObject rangedDispersed;
    [SerializeField] Company_UI company_ui;
    [SerializeField] Company company;
    [SerializeField] GameObject targetingPanel;

    GameObject[] formationOpts;
    string[] formationStrs;
    Dictionary<GameObject, string> formationDict;
    int i = 0;

    public Dictionary<GameObject, string> FormationDict { get { return formationDict; } }
    
    private void Awake()
    {
        formationOpts = new GameObject[5] { meleeWedge, meleeSaw, meleeSquare, rangedSquare, rangedDispersed};
        formationStrs = new string[5] { "Formation_Wedge", "Formation_Saw", "Formation_Square", "Formation_RangedSquare", "Formation_RangedDispersed" };
        formationDict = new();

        foreach (GameObject go in formationOpts)
        {
            formationDict.Add(go, formationStrs[i]);
            i++;
        }
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
                    rangedSquare.SetActive(true);
                    rangedDispersed.SetActive(true);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        foreach (GameObject go in formationOpts)
        {
            if (go.gameObject == pointerEventData.pointerPress)
            {
                targetingPanel.GetComponent<TargetingOptions>().Formation = formationDict[go];
                Debug.Log("Sth else is wrong"); //this be broken
            }
        }
    }



}
