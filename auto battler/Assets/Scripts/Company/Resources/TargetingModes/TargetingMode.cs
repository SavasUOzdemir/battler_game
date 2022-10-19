using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetingMode", menuName = "ScriptableObjects/TargetingMode", order = 1)]
public class TargetingMode : ScriptableObject
{
    public string targetingModeName;
    public string[] availableToFormations;

    void OnEnable()
    {
        foreach (string formation in availableToFormations)
        {
            CompanyFormations.AddTargetingModeToFormation(formation, targetingModeName);
        }
    }

}
