using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompanyFormations;

public class CompanyMover : MonoBehaviour
{
    Company company;
    Vector3[] modelPositions;
    [field: SerializeField] public Vector3 CurrentMovementTarget { get; private set; }
    [field: SerializeField] public Vector3 CompanyDir { get; private set; } = Vector3.right;
    [field: SerializeField] public bool Moving { get; private set; } = true;

    void Awake()
    {
        company = GetComponent<Company>();
    }

    void MoveModels()
    {
        for (int i = 0; i < company.models.Count; i++)
        {
            company.models[i].GetComponent<Actor>().Move(modelPositions[i]);
        }
    }

    void MoveCompany(Vector3 target)
    {
        CurrentMovementTarget = target;
        Moving = true;
        Vector3 dir = target - transform.position;
        CompanyDir = dir.normalized;
        CompanyFormations.CalcModelPositions(target, CompanyDir, company.models.Count, modelPositions, arrangement, modelColliderDia);
        MoveModels();
    }

    void StopCompany()
    {
        Moving = false;
        Vector3 newDir = (CurrentMovementTarget - transform.position).normalized;
        CompanyFormations.CalcModelPositions(transform.position + newDir, newDir, company.models.Count, modelPositions, arrangement, modelColliderDia);
        MoveModels();
    }


    void RotateCompany(Vector3 dir)
    {
        CompanyDir = dir;
        CompanyFormations.CalcModelPositions(transform.position, dir, company.models.Count, modelPositions, arrangement, modelColliderDia);
        MoveModels();
    }

}
