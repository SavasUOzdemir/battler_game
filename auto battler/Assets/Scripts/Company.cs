using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    Vector3 directionVector = Vector3.right;
    float range = 0;
    GameObject[] rangeCheckBuffer = new GameObject[500];
    int team = 0;
    float aiUpdateTime = 0.2f;
    float currentTime = 0;
    Vector3 companyDirection = Vector3.right;

    void Start()
    {
        Init();
        AddUnitActionToCompany(typeof(UnitActionProjectileTest));
    }

    
    void Update()
    {
        updateCompanyPosition();
        if(currentTime <= 0)
        {
            BehaviourLoop();
            currentTime = aiUpdateTime;
        }
        currentTime -= Time.deltaTime;
    }

    private void Init()
    {
        ModelAttributes messagePar = new ModelAttributes(gameObject, team);
        calcModelPositions(transform.position);
        for (int i = 0; i < modelCount; i++)
        {
            models.Add(Instantiate(prefab, modelPositions[i], Quaternion.identity));
            models[i].SendMessage("SetCompany",messagePar);
        }
    }

    void calcModelPositions(Vector3 companyPos)
    {
        Vector3 directionVector = transform.position - companyPos;
        if (directionVector == Vector3.zero)
            directionVector = Vector3.right;
        Vector3 localLeft = Vector3.Cross(directionVector, Vector3.down).normalized;
        Vector3 localBack = -directionVector.normalized;
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

    void moveModels(Vector3 target)
    {
        calcModelPositions(target);
        for (int i = 0; i < models.Count; i++)
        {
            models[i].gameObject.SendMessage("Move", modelPositions[i]);
        }
    }

    void updateCompanyPosition()
    {
        Vector3 sum = Vector3.zero;
        foreach(GameObject model in models)
        {
            sum += model.transform.position;
        }
        transform.position = sum / models.Count;
    }

    bool AreEnemiesInRange()
    {
        Utils.UnitsInRadius(transform.position, range, rangeCheckBuffer);
        foreach(GameObject obj in rangeCheckBuffer)
        {
            if(!obj)
                continue;
            if(obj.GetComponent<Attributes>().GetTeam() != team &&
                (obj.transform.position - transform.position).sqrMagnitude <= range*range)
                return true;
        }
        return false;
    }

    bool AreEnemiesInFront()
    {
        return false;
    }

    Vector3 FindClosestEnemyPosition()
    {
        Utils.UnitsInRadius(transform.position, 2000, rangeCheckBuffer);
        float distSqr = Mathf.Infinity;
        Vector3 distVector;
        Vector3 target = Vector3.zero;
        foreach(GameObject model in rangeCheckBuffer)
        {
            if (!model || model.GetComponent<Attributes>().GetTeam() == team)
                continue;
            distVector = model.transform.position - transform.position;
            if (distSqr > distVector.sqrMagnitude) 
            { 
                distSqr = distVector.sqrMagnitude;
                target = model.transform.position;
            }
        }
        return target;
    }

    void BehaviourLoop()
    {
        if (!AreEnemiesInRange())
        {
            Vector3 enemyPos = FindClosestEnemyPosition();
            if(enemyPos == Vector3.zero)
            {
                return;
            }
            Vector3 newPos = (transform.position - enemyPos).normalized * 30 + enemyPos;
            moveModels(newPos);
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
        //TODO
    }
}

public class ModelAttributes
{
    public GameObject company;
    public int team;

    public ModelAttributes(GameObject _company, int _team)
    {
        company = _company;
        team = _team;
    }
}
