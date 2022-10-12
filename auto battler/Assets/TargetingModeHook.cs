using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeHook : MonoBehaviour
{
    [SerializeField] string targetingModeName = null;
    [SerializeField] GameObject targetingPanel;
    [SerializeField] GameObject formationsPanel;
    [SerializeField] Company company;
    bool primarySet = false;

    private void OnEnable()
    {
        company = formationsPanel.GetComponent<FormationOptions>().Company;
    }
    public string TargetingModeName
    {
        get { return targetingModeName; }
    }

    public void OnButtonPress()
    {
        if (!primarySet)
        {
            company.GetComponent<Company>().ChangePrimaryTargetingMode(targetingModeName);
            primarySet = !primarySet;
        }
        else
        { 
            company.GetComponent<Company>().ChangeSecondaryTargetingMode(targetingModeName); 
            primarySet = !primarySet;            
        }
    }
}
