using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Company : MonoBehaviour
{
    public enum Formation
    {
        Line,
        Skirmish
    }

    public GameObject prefab;
    [SerializeField] public GameObject plane;
    [SerializeField] public int ModelCount { get; set; } = 16;

    List<GameObject> models = new List<GameObject>();
    Formation formation = 0;
    Vector3[] modelPositions;
    Vector3 companyDir = Vector3.right;
    Vector3 currentTarget;
    [SerializeField] float range = 0;
    float modelColliderDia = 1;
    GameObject[] buffer = new GameObject[500];
    float aiUpdateTime = 0.5f;
    float currentTime = 0;
    float attackArc = 60f;
    float aiFuse = 01f;
    List<System.Type> modelUpgradeList = new List<System.Type>();
    List<CompanyAction> companyActions = new List<CompanyAction>();
    List<CompanyPassive> companyPassives = new List<CompanyPassive>();
    float globalCooldown = 1f;
    float currentGlobalCooldown = 0f;
    Vector3 fleeDir;

    public List<GameObject> enemiesList = new List<GameObject>();


    //TEMP
    [SerializeField] int team = 0;
    [SerializeField] float meleeRange = 3f;
    [SerializeField] bool debug = false;

    //Formation Variables
    [SerializeField] int columns = 8;

    //AI STATE
    [SerializeField] bool inMelee = false;
    [SerializeField] bool inMeleeLastFrame = false;
    [SerializeField] bool moving = true;
    [SerializeField] bool inRange = false;
    [SerializeField] bool inFront = false;

    //GAMEPLAY STATS
    [SerializeField] float maxMorale = 100f;
    [SerializeField] float currentMorale;
    [SerializeField] bool broken = false;

    //Attach upgrades in editor
    [SerializeField] List<string> editorUpgrades = new List<string>();

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
        AddEditorUpgrades();
        BattlefieldManager.AddCompany(gameObject);
        modelPositions = new Vector3[ModelCount];
        Init();
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad < aiFuse)
            return;

        UpdateBannerPosition();

        CheckMelee();
        if (inMelee && !broken)
        {
            if (moving)
                StopCompany();
            inMeleeLastFrame = true;
            moving = false;
            return;
        }

        if (currentGlobalCooldown > 0f)
            currentGlobalCooldown -= Time.deltaTime;
        if(!broken)
            ActionLoop();
        
        MovementStateMachine();
        if(currentTime <= 0)
        {
            MovementLoop();
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
        for (int i = 0; i < ModelCount; i++)
        {
            models.Add(Instantiate(prefab, modelPositions[i], Quaternion.identity, newParent.transform));
            models[i].GetComponent<Attributes>().SetCompany(messagePar);
        }
        modelColliderDia = models[0].GetComponent<CapsuleCollider>().radius * 2;
        CalcModelPositions(transform.position, companyDir);
        for (int i = 0; i < ModelCount; i++)
        {
            models[i].transform.position = modelPositions[i];
        }
        AttachUpgradesToModels();
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
        Debug.Log("Stop");
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
        int i;
        for (i = 0; i < models.Count && i < columns; i++)
        {
            sum += models[i].transform.position;
        }
        transform.position = sum / i;
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

    void ActionLoop()
    {
        if (currentGlobalCooldown <= 0)
        {
            foreach (CompanyAction action in companyActions)
            {
                if (action.GetCurrentCooldown() <= 0 && action.FindTarget())
                {
                    if (action.DoAction())
                        currentGlobalCooldown = globalCooldown;
                }
            }
        }
    }

    void MovementLoop()
    {
        if (broken)
        {
            MoveCompany(fleeDir);
            return;
        }

        FindEnemies();
        if (!inRange)
        {
            Vector3 enemyPos = FindClosestEnemyPosition();
            if (enemyPos == Vector3.zero)
            {
                return;
            }
            MoveCompany(enemyPos);
        }
        else if (!inFront)
        {
            RotateCompany((FindClosestEnemyPosition() - transform.position).normalized);
        }
        else if(moving)
        {
            StopCompany();
        }
    }

    void MovementStateMachine()
    {
        inRange = AreEnemiesInRange();
        inFront = AreEnemiesInFront();
    }

    void CheckMelee()
    {
        if((FindClosestEnemyPosition() - transform.position).sqrMagnitude <= meleeRange * meleeRange)
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

        if (type.IsSubclassOf(typeof(UnitAction)) || type.IsSubclassOf(typeof(UnitPassive)))
        {
            modelUpgradeList.Add(type);
        }else if (type.IsSubclassOf(typeof(CompanyAction)))
        {
            companyActions.Add(gameObject.AddComponent(type) as CompanyAction);
        }else if (type.IsSubclassOf(typeof(CompanyPassive)))
        {
            companyPassives.Add(gameObject.AddComponent(type) as CompanyPassive);
            companyPassives.Last().OnPurchase();
        }
    }

    void AttachUpgradesToModels()
    {
        foreach (System.Type upg in modelUpgradeList)
        {
            foreach (GameObject model in models)
            {
                model.GetComponent<Actor>().AddUnitAction(upg);
            }

            UnitAction unitAction = models[0].GetComponent(upg) as UnitAction;
            if (unitAction)
                if (unitAction.GetRange() > range)
                    range = unitAction.GetRange();
        }

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

    public void ChangeMorale(float change)
    {
        currentMorale += change;
        if(currentMorale <= 0)
        {
            currentMorale = 0;
            broken = true;
            fleeDir = new Vector3(-100 + 200 * team, transform.position.y, 0);
        }
    }

    public void ExhaustCompany(float exhaust)
    {
        foreach (GameObject model in models)
        {
            if(!model)
                continue;
            model.GetComponent<Attributes>().ChangeEndurance(exhaust);
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
