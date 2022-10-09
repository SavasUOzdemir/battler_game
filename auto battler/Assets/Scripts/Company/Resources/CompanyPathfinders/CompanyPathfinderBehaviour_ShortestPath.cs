using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyPathfinderBehaviour_ShortestPath : CompanyPathfinderBehaviour
{
    private Company company;

    void Awake()
    {
        company = GetComponent<Company>();
    }
    public override Vector3 GetMovementTarget()
    {
        return company.CurrentEnemyTarget.transform.position;
    }
}
