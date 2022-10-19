using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeBehaviour_FlyingSquad : TargetingModeBehaviour
{
    public override bool DetermineTarget(Vector3 companyPosition, ref GameObject target)
    {
        GameObject currentTarget = null;
        float distSqr;
        float shortestDistSqr = Mathf.Infinity;
        foreach (GameObject enemyCompany in company.enemiesList)
        {
            if (!enemyCompany)
                continue;
            if (enemyCompany.GetComponent<Company>().FlyingCompany)
            {
                distSqr = (companyPosition - enemyCompany.transform.position).sqrMagnitude;
                if (distSqr < shortestDistSqr)
                {
                    shortestDistSqr = distSqr;
                    currentTarget = enemyCompany;
                }
            }
        }
        if (!currentTarget) return false;
        target = currentTarget;
        return true;
    }
}
