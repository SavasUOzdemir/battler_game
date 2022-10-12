using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyPathfinderBehaviour_ShortestPath : CompanyPathfinderBehaviour
{
    
    public override Vector3 GetMovementTarget(out Vector3 direction)
    {
        if (company.CurrentEnemyTarget)
        {
            direction = (company.CurrentEnemyTarget.transform.position - transform.position).normalized;
            return company.CurrentEnemyTarget.transform.position;
        }

        direction = mover.CurrentCompanyDir;
        return transform.position;
    }
}
