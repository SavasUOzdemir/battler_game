using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrangement", menuName = "ScriptableObjects/Arrangement", order = 1)]
public class Arrangement : ScriptableObject
{
    public string arrangementName;
    public string[] availableToFormations;

    void OnEnable()
    {
        foreach (string formation in availableToFormations)
        {
            CompanyFormations.AddArrangementToFormation(formation, arrangementName);
        }
    }
}
