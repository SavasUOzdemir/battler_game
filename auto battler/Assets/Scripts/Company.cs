using System.Collections.Generic;
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
    Vector3[] modelPositions;
    Vector3 companyDir = Vector3.right;
    Vector3 currentTarget;
    float range = 0;
    float modelColliderDia = 1;
    GameObject[] buffer = new GameObject[500];
    List<GameObject> enemiesList = new List<GameObject>();
    [SerializeField] int team = 0;
    float aiUpdateTime = 0.5f;
    float currentTime = 0;
    float attackArc = 60f;
    float aiFuse = 01f;


    //TEMP
    [SerializeField] float meleeRange = 5f;
    [SerializeField] bool debug = false;

    //Formation Variables
    [SerializeField] int columns = 8;

    //AI STATE
    [SerializeField] bool inMelee = false;
    [SerializeField] bool inMeleeLastFrame = false;
    [SerializeField] bool moving = true;

    //GAMEPLAY STATS
    [SerializeField] float maxMorale = 100f;
    [SerializeField] float currentMorale;
    [SerializeField] bool broken = false;

    //Attach upgrades in editor
    [SerializeField] List<string> editorUpgrades = new List<string>();
    bool addedEditorUpgrades = false;

    void AddEditorUpgrades()
    {
        foreach(string upgrade in editorUpgrades)
        {
            Debug.Log(System.Type.GetType(upgrade));
            AddUnitUpgrade(System.Type.GetType(upgrade));
        }
    }


    void Start()
    {
        BattlefieldManager.AddCompany(gameObject);
        modelPositions = new Vector3[modelCount];
        Init();
    }

    void Update()
    {
        if (!addedEditorUpgrades)
        {
            AddEditorUpgrades();
            addedEditorUpgrades = true;
        }
            
        if (Time.timeSinceLevelLoad < aiFuse)
            return;
        UpdateBannerPosition();
        CheckMelee();
        if (inMelee)
        {
            if (moving)
                StopCompany();
            inMeleeLastFrame = true;
            moving = false;
            return;
        }
            
        if(currentTime <= 0)
        {
            BehaviourLoop();
            currentTime = aiUpdateTime;
        }
        currentTime -= Time.deltaTime;
    }

    private void Init()
    {
        if (team == 1)
            companyDir *= -1;
        currentMorale = maxMorale;
        ModelAttributes messagePar = new ModelAttributes(this, team);
        CalcModelPositions(transform.position, companyDir);
        var newParent = new GameObject();
        for (int i = 0; i < modelCount; i++)
        {
            models.Add(Instantiate(prefab, modelPositions[i], Quaternion.identity, newParent.transform));
            models[i].GetComponent<Attributes>().SetCompany(messagePar);
        }
        modelColliderDia = models[0].GetComponent<CapsuleCollider>().radius * 2;
        CalcModelPositions(transform.position, companyDir);
        for (int i = 0; i < modelCount; i++)
        {
            models[i].transform.position = modelPositions[i];
        }      
    }

    void CalcModelPositions(Vector3 companyPos, Vector3 direction)
    {
        Vector3 localLeft = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 localBack = -(direction.normalized);
        Vector3 firstPosition;
        int rows = models.Count / columns;
        int currentRow = 0;
        int currentModel;
        int leftOvers = models.Count % columns;
        switch (formation)
        {
            case Formation.Line:
                firstPosition = ((columns - 1) / 2f * modelColliderDia) * localLeft + companyPos;
                for(currentModel = 0; currentModel < rows * columns; currentModel++)
                {
                    modelPositions[currentModel] = firstPosition - (currentModel % columns) * localLeft + currentRow * localBack;
                    if (currentModel > 0 && (currentModel + 1) % columns == 0)
                        currentRow++;
                }
                firstPosition = ((leftOvers - 1) / 2 * modelColliderDia) * localLeft + rows * localBack + companyPos;
                for(int i = 0; i < leftOvers; i++)
                {
                    modelPositions[currentModel + i] = firstPosition - i * localLeft;
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
            models[i].GetComponent<Actor>().Move(modelPositions[i]);
        }
    }

    void MoveCompany(Vector3 target)
    {
        currentTarget = target;
        moving = true;
        Vector3 dir = target - transform.position;
        companyDir = dir.normalized;
        CalcModelPositions(target, companyDir);
        MoveModels();
    }

    void StopCompany()
    {
        moving = false;
        Vector3 newDir = (currentTarget - transform.position).normalized;
        CalcModelPositions(transform.position + newDir, newDir);
        MoveModels();
    }

    
    void RotateCompany(Vector3 dir)
    {
        companyDir = dir;
        CalcModelPositions(transform.position, dir);
        MoveModels();
    }

    void UpdateBannerPosition()
    {
        if (models.Count == 0)
        {
            RemoveCompany();
        }

        Vector3 sum = Vector3.zero;
        for(int i = 0; i < models.Count && i < columns; i++)
        {
            sum += models[i].transform.position;
        }
        transform.position = sum / models.Count;
    }

    bool AreEnemiesInRange()
    {
        foreach (GameObject obj in enemiesList)
        {
            if (!obj)
                continue;
            if ((obj.transform.position - transform.position).sqrMagnitude <= (range * range * 0.81))
                return true;
        }
        return false;
    }

    bool AreEnemiesInFront()
    {
        BattlefieldManager.CompaniesInRadius(transform.position, 1000, buffer);
        foreach (GameObject obj in enemiesList)
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
        foreach (GameObject company in enemiesList)
        {
            if (!company)
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
        BattlefieldManager.CompaniesInRadius(transform.position, 2000, buffer);
        enemiesList.Clear();
        foreach (GameObject company in buffer)
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
            MoveCompany(enemyPos);
        }
        else if (!AreEnemiesInFront())
        {
            RotateCompany((FindClosestEnemyPosition() - transform.position).normalized);
        }
        else if(moving)
        {
            StopCompany();
        }
    }

    void CheckMelee()
    {
        if((FindClosestEnemyPosition()-transform.position).sqrMagnitude <= meleeRange * meleeRange)
        {
            inMelee = true;
            return;
        }
        inMelee = false;
    }

    public void AddUnitUpgrade(System.Type type)
    {
        if (!type.IsSubclassOf(typeof(UnitUpgrade)))
        {
            Debug.Log("Trying to add non UnitUpgrade type!");
            return;
        }

        foreach (GameObject model in models)
        {
            model.GetComponent<Actor>().AddUnitAction(type);
        }

        UnitAction unitAction = models[0].GetComponent(type) as UnitAction;
        if (unitAction)
            if (unitAction.GetRange() > range)
                range = unitAction.GetRange();
            
    }

    void RemoveCompany()
    {
        Destroy(gameObject);
        BattlefieldManager.RemoveCompany(gameObject);
    }

    public void RemoveModel(GameObject model)
    {
        models.Remove(model);
        if(models.Count == 0)
        {
            RemoveCompany();
        }
    }

    public int GetTeam()
    {
        return team;
    }

    public Vector3 GetFacing()
    {
        return companyDir;
    }

    public bool InMelee()
    {
        return inMelee;
    }

    public bool Moving()
    {
        return moving;
    }

    public void changeMorale(float change)
    {
        currentMorale += change;
        if(currentMorale <= 0)
        {
            currentMorale = 0;
            broken = true;
        }
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
