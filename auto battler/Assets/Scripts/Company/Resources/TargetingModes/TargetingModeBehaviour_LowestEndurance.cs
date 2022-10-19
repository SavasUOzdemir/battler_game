using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeBehaviour_LowestEndurance : TargetingModeBehaviour
{
    public override bool DetermineTarget(Vector3 companyPosition, ref GameObject target)
    {
        GameObject currentTarget = null;
        float distSqr;
        float shortestDistSqr = Mathf.Infinity;
        float currentTargetEndurance = Mathf.Infinity;
        foreach (GameObject enemyCompany in company.enemiesList)
        {
            if (!enemyCompany)
                continue;
            float currentEndurance = enemyCompany.GetComponent<Company>().AverageEndurance();
            if (currentTargetEndurance > currentEndurance)
            {
                distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                if (distSqr < shortestDistSqr)
                {
                    shortestDistSqr = distSqr;
                    currentTargetEndurance = currentEndurance;
                    currentTarget = enemyCompany;
                }
            }
        }
        if (!currentTarget) return false;
        target = currentTarget;
        return true;
    }
}
