using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Company : MonoBehaviour
{
    public enum Formation
    {
        Line,
        Skirmish
    }

    
    public GameObject prefab;
    public int modelCount = 16;

    List<GameObject> models = new List<GameObject>();
    Formation formation = 0;
    Vector3[] modelPositions = new Vector3[16];
    Vector3 companyDir = Vector3.right;
    float range = 0;
    GameObject[] buffer = new GameObject[500];
    List<GameObject> enemiesList = new List<GameObject>();
    [SerializeField]int team = 0;
    float aiUpdateTime = 0.5f;
    float currentTime = 0;
    float attackArc = 60f;

    //TEMP
    [SerializeField] int UA;

    void Start()
    {
        Init();
        if(UA == 0)
            AddUnitActionToCompany(typeof(UnitActionProjectileTest));
        if(UA == 1)
            AddUnitActionToCompany(typeof(UnitActionBasicAttack));
    }

    void Update()
    {
        UpdateCompanyPosition();
        if(currentTime <= 0)
        {
            BehaviourLoop();
            currentTime = aiUpdateTime;
        }
        currentTime -= Time.deltaTime;      
    }

    private void Init()
    {
        ModelAttributes messagePar = new ModelAttributes(this, team);
        CalcModelPositions(transform.position, Vector3.right);
        for (int i = 0; i < modelCount; i++)
        {
            models.Add(Instantiate(prefab, modelPositions[i], Quaternion.identity));
            models[i].SendMessage("SetCompany",messagePar);
        }
    }

    void CalcModelPositions(Vector3 companyPos, Vector3 direction)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        Vector3 firstPosition = companyPos + 1.75f * localLeft;
        switch (formation)
        {
            case Formation.Line:
                for(int i = 0; i < modelCount; i++)
                {
                    if(i < 8)
                    {
                        modelPositions[i] = firstPosition - i * localLeft;
                    }else if (i >= 8)
                    {
                        modelPositions[i] = firstPosition + localBack - (i % 8) * localLeft;
                    }
                }
                break;
            case Formation.Skirmish:
                //TODO
                break;

        }
    }

    void MoveModels()
    {
        for (int i = 0; i < models.Count; i++)
        {
            models[i].gameObject.SendMessage("Move", modelPositions[i]);
        }
    }

    void MoveCompany(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        companyDir = dir.normalized;
        CalcModelPositions(target, companyDir);
        MoveModels();
    }

    void StopCompany()
    {
        for(int i = 0; i < models.Count; i++)
        {
            models[i].SendMessage("EndMove");
        }
    }

    void RotateCompany(Vector3 dir)
    {
        companyDir = dir;
        CalcModelPositions(transform.position, dir);
        MoveModels();
    }

    void UpdateCompanyPosition()
    {
        if (models.Count == 0)
        {
            Destroy(this);
            return;
        }
            
        Vector3 sum = Vector3.zero;
        foreach(GameObject model in models)
        {
            sum += model.transform.position;
        }
        transform.position = sum / models.Count;
    }

    bool AreEnemiesInRange()
    {
        foreach(GameObject obj in enemiesList)
        {
            if(!obj)
                continue;
            if((obj.transform.position - transform.position).sqrMagnitude <= (range*range * 0.81))
                return true;
        }
        return false;
    }

    bool AreEnemiesInFront()
    {
        Utils.CompaniesInRadius(transform.position, 1000, buffer);
        foreach(GameObject obj in enemiesList)
        {
            if (!obj)
                continue;
            if (Vector3.Angle(obj.transform.position - transform.position, companyDir) < attackArc)
                return true;
        }
        return false;
    }

    Vector3 FindClosestEnemyPosition()
    {
        float distSqr = Mathf.Infinity;
        Vector3 distVector;
        Vector3 target = Vector3.zero;
        foreach(GameObject company in enemiesList)
        {
            if(!company)
                continue;
            distVector = company.transform.position - transform.position;
            if (distSqr > distVector.sqrMagnitude) 
            { 
                distSqr = distVector.sqrMagnitude;
                target = company.transform.position;
            }
        }
        return target;
    }

    void FindEnemies()
    {
        Utils.CompaniesInRadius(transform.position, 2000, buffer);
        enemiesList.Clear();
        foreach(GameObject company in buffer)
        {
            if (!company || !company.GetComponent<Company>() || company.GetComponent<Company>().GetTeam() == team)
                continue;
            else
                enemiesList.Add(company);
        }
    }

    void BehaviourLoop()
    {
        FindEnemies();
        if (!AreEnemiesInRange())
        {
            Vector3 enemyPos = FindClosestEnemyPosition();
            if (enemyPos == Vector3.zero)
            {
                return;
            }
            Vector3 newPos = (transform.position - enemyPos).normalized * (range * 0.9f) + enemyPos;
            MoveCompany(enemyPos);
        }
        else if (!AreEnemiesInFront())
        {
            RotateCompany((FindClosestEnemyPosition() - transform.position).normalized);
        }
        else
        {
            StopCompany();
        }
    }

    public void AddUnitActionToCompany(System.Type type)
    {
        if (!type.IsSubclassOf(typeof(UnitAction)))
        {
            Debug.Log("Trying to add non UnitAction type!");
            return;
        }
            
        foreach(GameObject model in models)
        {
            model.GetComponent<Actor>().AddUnitAction(type);
        }

        float actionRange = (models[0].GetComponent(type) as UnitAction).GetRange();
        if (actionRange > range)
            range = actionRange;
    }

    public void RemoveModel(GameObject model)
    {
        models.Remove(model);
    }

    public int GetTeam()
    {
        return team;
    }

    public Vector3 GetFacing()
    {
        return companyDir;
    }
}

public class ModelAttributes
{
    public Company company;
    public int team;

    public ModelAttributes(Company _company, int _team)
    {
        company = _company;
        team = _team;
    }
}
