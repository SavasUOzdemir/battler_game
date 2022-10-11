using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationButtonPress : MonoBehaviour
{
    [SerializeField] string formationName = null;
    [SerializeField] GameObject targetingPanel;


    public void OnButtonPress()
    {
        targetingPanel.GetComponent<TargetingOptions>().Formation = formationName;
    }
}
