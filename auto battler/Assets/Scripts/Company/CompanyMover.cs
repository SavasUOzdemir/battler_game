using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompanyFormations;
using static UnityEngine.GraphicsBuffer;

public class CompanyMover : MonoBehaviour
{
    Company company;
    private CompanyPathfinding _companyPathfinding;
    public CompanyPathfinding CompanyPathfinding
    {
        get => _companyPathfinding = _companyPathfinding != null ? _companyPathfinding : GetComponent<CompanyPathfinding>();
    }

    public ArrangementBehaviour arranger;

    public Vector3[] ModelPositions { get; private set; }
    public float ModelColliderDia { get; set; }
    [field: SerializeField] public Vector3 CurrentMovementTarget { get; private set; }
    [field: SerializeField] public Vector3 CompanyDir { get; private set; } = Vector3.right;
    [field: SerializeField] public bool Moving { get; private set; } = true;

    public void init()
    {
        ModelPositions = new Vector3[company.ModelCount];
    }
    void Awake()
    {
        company = GetComponent<Company>();
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateBannerPosition();
    }

    void MoveModels()
    {
        for (int i = 0; i < company.models.Count; i++)
        {
            company.models[i].GetComponent<Actor>().Move(ModelPositions[i]);
        }
    }

    public void MoveCompany(Vector3 target)
    {
        CurrentMovementTarget = target;
        Moving = true;
        Vector3 dir = target - transform.position;
        CompanyDir = dir.normalized;
        arranger.ArrangeModels(target, CompanyDir);
        MoveModels();
    }

    public void StopCompany()
    {
        Moving = false;
        Vector3 newDir = (CurrentMovementTarget - transform.position).normalized;
        arranger.ArrangeModels(transform.position + newDir, newDir);
        MoveModels();
    }

    public void RotateCompany(Vector3 dir)
    {
        CompanyDir = dir;
        arranger.ArrangeModels(transform.position, dir);
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
