using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyPathfinding_ShortestPath : CompanyPathfinding
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
