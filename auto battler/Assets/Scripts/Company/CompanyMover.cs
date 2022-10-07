using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompanyFormations;

public class CompanyMover : MonoBehaviour
{
    Company company;
    private CompanyPathfinding _companyPathfinding;
    public CompanyPathfinding CompanyPathfinding
    {
        get => _companyPathfinding = _companyPathfinding != null ? _companyPathfinding : GetComponent<CompanyPathfinding>();
    }
    Vector3[] modelPositions;
    public float ModelColliderDia { get; set; }
    [field: SerializeField] public Vector3 CurrentMovementTarget { get; private set; }
    [field: SerializeField] public Vector3 CompanyDir { get; private set; } = Vector3.right;
    [field: SerializeField] public bool Moving { get; private set; } = true;
    [field: SerializeField] public Arrangement Arrangement { get; set; } = CompanyFormations.Arrangement.Line;

    void Awake()
    {
        company = GetComponent<Company>();
    }

    void Start()
    {
        modelPositions = new Vector3[company.models.Count];
    }

    void Update()
    {
        UpdateBannerPosition();
    }

    void MoveModels()
    {
        for (int i = 0; i < company.models.Count; i++)
        {
            company.models[i].GetComponent<Actor>().Move(modelPositions[i]);
        }
    }

    public void MoveCompany(Vector3 target)
    {
        CurrentMovementTarget = target;
        Moving = true;
        Vector3 dir = target - transform.position;
        CompanyDir = dir.normalized;
        CompanyFormations.CalcModelPositions(target, CompanyDir, company.models.Count, modelPositions, Arrangement, ModelColliderDia);
        MoveModels();
    }

    public void StopCompany()
    {
        Moving = false;
        Vector3 newDir = (CurrentMovementTarget - transform.position).normalized;
        CompanyFormations.CalcModelPositions(transform.position + newDir, newDir, company.models.Count, modelPositions, Arrangement, ModelColliderDia);
        MoveModels();
    }


    public void RotateCompany(Vector3 dir)
    {
        CompanyDir = dir;
        CompanyFormations.CalcModelPositions(transform.position, dir, company.models.Count, modelPositions, Arrangement, ModelColliderDia);
        MoveModels();
    }

    void UpdateBannerPosition()
    {
        Vector3 sum = Vector3.zero;
        int i;
        for (i = 0; i < company.models.Count && i < CompanyFormations.Columns; i++)
        {
            sum += company.models[i].transform.position;
        }
        transform.position = sum / i;

        if ((transform.position - CurrentMovementTarget).sqrMagnitude < 2)
        {
            CompanyPathfinding.OnTargetReached();
        }
    }
}
