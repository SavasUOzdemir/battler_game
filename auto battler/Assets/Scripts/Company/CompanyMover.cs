using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompanyFormations;
using static UnityEngine.GraphicsBuffer;

public class CompanyMover : MonoBehaviour
{
    Company company;
    private CompanyPathfinderBehaviour _companyPathfinderBehaviour;
    public CompanyPathfinderBehaviour companyPathfinderBehaviour
    {
        get => _companyPathfinderBehaviour = _companyPathfinderBehaviour != null ? _companyPathfinderBehaviour : GetComponent<CompanyPathfinderBehaviour>();
    }

    public ArrangementBehaviour arranger;

    public Vector3[] ModelPositions { get; private set; }
    public float ModelColliderDia { get; set; }
    [field: SerializeField] public Vector3 CurrentMovementTarget { get; private set; }
    [field: SerializeField] public Vector3 FinalCompanyDir { get; private set; }
    [field: SerializeField] public Vector3 CurrentCompanyDir { get; private set; }
    [field: SerializeField] public bool Moving { get; private set; } = true;
    private Vector3 posLastFrame;

    public void init()
    {
        ModelPositions = new Vector3[company.ModelCount];
    }
    void Awake()
    {
        company = GetComponent<Company>();
        FinalCompanyDir = Vector3.right - Vector3.right * 2 * company.Team;
    }

    void Start()
    {
        posLastFrame = transform.position;
    }

    void Update()
    {
        if(Moving)
            UpdateCurrentDirection();
    }

    void MoveModels()
    {
        for (int i = 0; i < company.models.Count; i++)
        {
            company.models[i].GetComponent<Actor>().Move(ModelPositions[i]);
        }
    }

    void UpdateCurrentDirection()
    {
        CurrentCompanyDir = (transform.position - posLastFrame).normalized;
        posLastFrame = transform.position;
    }

    public void MoveCompany(Vector3 target)
    {
        CurrentMovementTarget = target;
        Moving = true;
        Vector3 dir = target - transform.position;
        FinalCompanyDir = dir.normalized;
        arranger.ArrangeModels(target, FinalCompanyDir);
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
        FinalCompanyDir = dir;
        arranger.ArrangeModels(transform.position, dir);
        MoveModels();
    }

}
