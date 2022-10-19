using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompanyPathfinder", menuName = "ScriptableObjects/CompanyPathfinder", order = 1)]
public class CompanyPathfinder : ScriptableObject
{
    public string companyPathfinderName;
    public string[] availableToFormations;

    void OnEnable()
    {
        foreach (string formation in availableToFormations)
        {
            CompanyFormations.AddPathfinderToFormation(formation, companyPathfinderName);
        }
    }
}
