using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationButtonPress : MonoBehaviour
{
    [SerializeField] string formationName = null;
    [SerializeField] GameObject targetingPanel;
    [SerializeField] GameObject formationsPanel;
    Company company;


    public void OnButtonPress()
    {
        targetingPanel.GetComponent<TargetingOptions>().Formation = formationName;
        company = formationsPanel.GetComponent<FormationOptions>().Company;
        company.SendMessage("ChangeFormation", formationName, SendMessageOptions.RequireReceiver);
    }
}
