using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Company : MonoBehaviour
{
    //Misc
    GameObject[] buffer = new GameObject[500];
    public List<GameObject> enemiesList = new();
    public Vector3 CompanyDir { get; private set; } = Vector3.right;
    Vector3 currentTarget;
    Vector3 fleeDir;
    float range = 0;
    [field: SerializeField] public int Team { get; set; } = 0;
    [SerializeField] float meleeRange = 3f;
    [SerializeField] bool debug = false;

    //Model Data
    public GameObject modelPrefab;
    [field: SerializeField] public int ModelCount { get; set; } = 16;
    List<GameObject> models = new();
    Vector3[] modelPositions;
    float modelColliderDia = 1;

    //Upgrades
    List<System.Type> modelUpgradeList = new();
    List<CompanyAction> companyActions = new();
    List<CompanyPassive> companyPassives = new();
    [SerializeField] float globalCooldown = 1f;
    float currentGlobalCooldown = 0f;

    //Formation Variables
    [SerializeField] CompanyFormations.Formation formation = CompanyFormations.Formation.Square;
    [SerializeField] CompanyFormations.Arrangement arrangement = CompanyFormations.Arrangement.Line;
    [field: SerializeField] public CompanyFormations.Targets PrimaryTarget { get; set; } = CompanyFormations.Targets.ClosestSquad;
    [field: SerializeField] public CompanyFormations.Targets SecondaryTarget { get; set; } = CompanyFormations.Targets.ClosestSquad;

    //AI STATE
    [field: SerializeField] public bool InMelee { get; private set; } = false;
    [field: SerializeField] public bool Moving { get; private set; } = true;
    [SerializeField] bool inRange = false;
    [SerializeField] bool inFront = false;
    float aiUpdateTime = 0.5f;
    float currentTime = 0;
    float attackArc = 60f;
    float aiFuse = 01f;

    //GAMEPLAY STATS
    [SerializeField] float maxMorale = 100f;
    [SerializeField] float currentMorale;
    [SerializeField] bool broken = false;
    bool meleeCompany = false;
    bool rangedCompany = false;

    //Attach upgrades in editor
    [SerializeField] List<string> editorUpgrades = new();

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
        if (InMelee && !broken)
        {
            if (Moving)
                StopCompany();
            Moving = false;
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
        if (Team == 1)
            CompanyDir *= -1;
        ChangeFormation(formation);
        currentMorale = maxMorale;
        ModelAttributes messagePar = new ModelAttributes(this, Team);
        CompanyFormations.CalcModelPositions(transform.position, CompanyDir, models, modelPositions, arrangement, modelColliderDia);
        var newParent = new GameObject();
        for (int i = 0; i < ModelCount; i++)
        {
            models.Add(Instantiate(modelPrefab, modelPositions[i], Quaternion.identity, newParent.transform));
            models[i].GetComponent<Attributes>().SetCompany(messagePar);
        }
        modelColliderDia = models[0].GetComponent<CapsuleCollider>().radius * 2;
        CompanyFormations.CalcModelPositions(transform.position, CompanyDir, models, modelPositions, arrangement, modelColliderDia);
        for (int i = 0; i < ModelCount; i++)
        {
            models[i].transform.position = modelPositions[i];
        }
        AttachUpgradesToModels();
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
        Moving = true;
        Vector3 dir = target - transform.position;
        CompanyDir = dir.normalized;
        CompanyFormations.CalcModelPositions(target, CompanyDir, models, modelPositions, arrangement, modelColliderDia);
        MoveModels();
    }

    void StopCompany()
    {
        Moving = false;
        Vector3 newDir = (currentTarget - transform.position).normalized;
        CompanyFormations.CalcModelPositions(transform.position + newDir, newDir, models, modelPositions, arrangement, modelColliderDia);
        MoveModels();
    }

    
    void RotateCompany(Vector3 dir)
    {
        CompanyDir = dir;
        CompanyFormations.CalcModelPositions(transform.position, dir, models, modelPositions, arrangement, modelColliderDia);
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
        for (i = 0; i < models.Count && i < CompanyFormations.Columns; i++)
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
            if (Vector3.Angle(obj.transform.position - transform.position, CompanyDir) < attackArc)
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
            if (!company || !company.GetComponent<Company>() || company.GetComponent<Company>().Team == Team)
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
        else if(Moving)
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
            InMelee = true;
            return;
        }
        InMelee = false;
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
            {
                if (unitAction.GetRange() > range)
                    range = unitAction.GetRange();
                if (unitAction.IsMainWeapon())
                {
                    if (unitAction.IsActionMelee())
                        meleeCompany = true;
                    else
                    {
                        rangedCompany = true;
                    }
                }
            }
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

    public void ChangeMorale(float change)
    {
        currentMorale += change;
        if(currentMorale <= 0)
        {
            currentMorale = 0;
            broken = true;
            fleeDir = new Vector3(-100 + 200 * Team, transform.position.y, transform.position.z);
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

    public void ChangeFormation(CompanyFormations.Formation _formation)
    {
        formation = _formation;
        switch (formation)
        {
            case CompanyFormations.Formation.Square:
                arrangement = CompanyFormations.Arrangement.Line;
                break;
            case CompanyFormations.Formation.Saw:
                arrangement = CompanyFormations.Arrangement.Skirmish;
                break;
            case CompanyFormations.Formation.Wedge:
                arrangement = CompanyFormations.Arrangement.Wedge;
                break;
            case CompanyFormations.Formation.RangedSquare:
                arrangement = CompanyFormations.Arrangement.Line;
                break;
            case CompanyFormations.Formation.Dispersed:
                arrangement = CompanyFormations.Arrangement.Skirmish;
                break;
        }
        CompanyFormations.Targets[] possibleTargets = CompanyFormations.GetTargetingOptions(formation, Team);
        if (possibleTargets == null)
            return;
        PrimaryTarget = possibleTargets[0];
        if(possibleTargets.Length > 1)
            SecondaryTarget = possibleTargets[1];
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
