using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeBehaviour_LowestHealth : TargetingModeBehaviour
{
    public override bool DetermineTarget(Vector3 companyPosition, ref GameObject target)
    {
        GameObject currentTarget = null;
        float distSqr;
        float shortestDistSqr = Mathf.Infinity;
        float companyAverageHP;
        float currentTargetAverageHP = Mathf.Infinity;
        foreach (GameObject enemyCompany in company.enemiesList)
        {
            if (!enemyCompany)
                continue;
            companyAverageHP = enemyCompany.GetComponent<Company>().AverageHealth();
            if (currentTargetAverageHP >= companyAverageHP)
            {
                distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                if (distSqr < shortestDistSqr)
                {
                    shortestDistSqr = distSqr;
                    currentTargetAverageHP = companyAverageHP;
                    currentTarget = enemyCompany;
                }
            }
        }
        if (!currentTarget) return false;
        target = currentTarget;
        return true;
    }
}
