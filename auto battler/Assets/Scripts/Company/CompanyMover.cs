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
    [field: SerializeField] public bool Moving { get; private set; } = false;
    [field: SerializeField] public bool Rotating { get; private set; } = false;
    private Vector3 posLastFrame;

    public void init()
    {
        ModelPositions = new Vector3[company.ModelCount];
    }
    void Awake()
    {
        company = GetComponent<Company>();
        FinalCompanyDir = Vector3.right - Vector3.right * 2 * company.Team;
        CurrentCompanyDir = FinalCompanyDir;
        CurrentMovementTarget = transform.position;
    }

    void Start()
    {
        posLastFrame = transform.position;
    }

    void Update()
    {
        if(Moving && !company.InMelee)
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
        if (Rotating)
        {
            Vector3 sum = Vector3.zero;
            foreach (GameObject model in company.models)
            {
               sum += (model.GetComponent(typeof(Attributes)) as Attributes).GetFacing();
            }

            CurrentCompanyDir = sum / company.models.Count;
            if (CurrentCompanyDir == FinalCompanyDir)
            {
                Rotating = false;
                Moving = false;
            }
                
            return;
        }
        CurrentCompanyDir = (transform.position - posLastFrame).normalized;
        posLastFrame = transform.position;
    }

    //This method is called for moving on the path drawn by the pathfinder
    public void MoveOnPath()
    {
        CurrentMovementTarget = companyPathfinderBehaviour.GetMovementTarget();
        if (CurrentCompanyDir != FinalCompanyDir)
            RotateCompany(FinalCompanyDir);
        else
            MoveCompany(CurrentMovementTarget);
    }

    //Calling this method forces the company to move straight towards the target
    public void MoveCompany(Vector3 target)
    {
        CurrentMovementTarget = target;
        Moving = true;
        Vector3 dir = target - transform.position;
        if (dir.normalized == Vector3.zero)
            FinalCompanyDir = (dir * 10f).normalized;
        FinalCompanyDir = dir.normalized;
        arranger.ArrangeModels(target, FinalCompanyDir);
        MoveModels();
    }

    public void StopCompany()
    {
        Moving = false;
        Rotating = false;
        Vector3 newDir = (CurrentMovementTarget - transform.position).normalized;
        arranger.ArrangeModels(transform.position + newDir, newDir);
        MoveModels();
    }

    public void RotateCompany(Vector3 dir)
    {
        Moving = true;
        Rotating = true;
        FinalCompanyDir = dir;
        arranger.ArrangeModels(transform.position, dir);
        MoveModels();
    }

}
