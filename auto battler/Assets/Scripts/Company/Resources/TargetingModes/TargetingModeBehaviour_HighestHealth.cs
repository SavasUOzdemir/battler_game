using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeBehaviour_HighestHealth : TargetingModeBehaviour
{
    public override bool DetermineTarget(Vector3 companyPosition, ref GameObject target)
    {
        GameObject currentTarget = null;
        float distSqr;
        float shortestDistSqr = Mathf.Infinity;
        float AverageHP;
        float TargetAverageHP = 0f;
        foreach (GameObject enemyCompany in company.enemiesList)
        {
            if (!enemyCompany)
                continue;
            AverageHP = enemyCompany.GetComponent<Company>().AverageHealth();
            if (TargetAverageHP <= AverageHP)
            {
                distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                if (distSqr < shortestDistSqr)
                {
                    shortestDistSqr = distSqr;
                    TargetAverageHP = AverageHP;
                    currentTarget = enemyCompany;
                }
            }
        }
        if (!currentTarget) return false;
        target = currentTarget;
        return true;
    }
}
