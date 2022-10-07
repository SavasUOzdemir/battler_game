using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Company : MonoBehaviour
{
    //UI
    public bool selected = false;
    public bool done = false;
    //Misc
    GameObject[] buffer = new GameObject[500];
    public List<GameObject> enemiesList = new();
    public Vector3 CompanyDir { get; private set; } = Vector3.right;
    Vector3 fleeDir;
    float range = 0;
    [field: SerializeField] bool GameStarted { get; set; } = true;
    [field: SerializeField] public int Team { get; set; } = 0;
    [SerializeField] float meleeRange = 3f;
    [SerializeField] bool debug = false;
    [field: SerializeField] public bool MouseControl { get; private set; } = false;

    //Model Data
    public GameObject modelPrefab;
    [field: SerializeField] public int ModelCount { get; set; } = 16;
    public List<GameObject> models = new();
    Vector3[] modelPositions;
    float modelColliderDia = 1;

    //Upgrades
    List<System.Type> modelUpgradeList = new();
    List<CompanyAction> companyActions = new();
    List<CompanyPassive> companyPassives = new();
    [SerializeField] float globalCooldown = 1f;
    float currentGlobalCooldown = 0f;

    //Formation, Targeting and Pathfinding
    [SerializeField] CompanyFormations.Formation formation = CompanyFormations.Formation.Square;
    [SerializeField] CompanyFormations.Arrangement arrangement = CompanyFormations.Arrangement.Line;
    public GameObject CurrentEnemyTarget { get; private set; } = null;
    [field: SerializeField] public CompanyFormations.TargetingMode PrimaryTargetingMode { get; set; } = CompanyFormations.TargetingMode.ClosestSquad;
    [field: SerializeField] public CompanyFormations.TargetingMode SecondaryTargetingMode { get; set; } = CompanyFormations.TargetingMode.ClosestSquad;
    CompanyPathfinding companyPathfinding;
    CompanyMover companyMover;
    Vector3 currentMovementTarget;

    //AI STATE
    [field: SerializeField] public bool InMelee { get; private set; } = false;

    [field: SerializeField]
    [SerializeField] public bool Moving => companyMover.Moving;

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
    [field: SerializeField] public bool MeleeCompany { get; private set; } = false;
    [field: SerializeField] public bool RangedCompany { get; private set; } = false;
    [field: SerializeField] public bool FlyingCompany { get; private set; } = false;

    //Attach upgrades in editor
    [SerializeField] List<string> editorUpgrades = new();

    void AddEditorUpgrades()
    {
        foreach(string upgrade in editorUpgrades)
        {
            AddUnitUpgrade(System.Type.GetType(upgrade));
        }
    }

    void Awake()
    {
        BattlefieldManager.AddCompany(gameObject);
        companyMover = gameObject.AddComponent<CompanyMover>();
    }

    void Start()
    {
        AddEditorUpgrades();
        modelPositions = new Vector3[ModelCount];
        Init();
    }

    void Update()
    {
        if (MouseControl)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);
            UnityEngine.Plane battleFloor = new UnityEngine.Plane(Vector3.up, Vector3.zero);
            float distanceToCam;
            battleFloor.Raycast(ray, out distanceToCam);
            CompanyFormations.CalcModelPositions(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCam)),
                CompanyDir, models.Count, modelPositions, arrangement, modelColliderDia);
            for (int i = 0; i < ModelCount; i++)
            {
                models[i].transform.position = new Vector3(modelPositions[i].x, 0, modelPositions[i].z);
            }
        }

        if (!GameStarted)
            return;
        if (Time.timeSinceLevelLoad < aiFuse)
            return;

        CheckMelee();
        if (InMelee && !broken)
        {
            if (Moving)
                companyMover.StopCompany();
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

    private void Init() //Temp function for editor initialization
    {
        if (Team == 1)
            CompanyDir *= -1;
        ChangeFormation(formation);
        currentMorale = maxMorale;
        ModelAttributes messagePar = new ModelAttributes(this, Team);
        CompanyFormations.CalcModelPositions(transform.position, CompanyDir, models.Count, modelPositions, arrangement, modelColliderDia);
        var newParent = new GameObject();
        for (int i = 0; i < ModelCount; i++)
        {
            models.Add(Instantiate(modelPrefab, modelPositions[i], Quaternion.identity, newParent.transform));
            models[i].GetComponent<Attributes>().SetCompany(messagePar);
        }
        modelColliderDia = models[0].GetComponent<CapsuleCollider>().radius * 2;
        companyMover.ModelColliderDia = modelColliderDia;
        CompanyFormations.CalcModelPositions(transform.position, CompanyDir, models.Count, modelPositions, arrangement, modelColliderDia);
        for (int i = 0; i < ModelCount; i++)
        {
            models[i].transform.position = modelPositions[i];
        }
        AttachUpgradesToModels();
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

    void FindEnemies()
    {
        BattlefieldManager.CompaniesInRadius(transform.position, 2000, buffer);
        enemiesList.Clear();
        foreach (GameObject company in buffer)
        {
            if (!company || !company.GetComponent<Company>() || company.GetComponent<Company>().Team == Team)
                continue;
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
            companyMover.MoveCompany(fleeDir);
            return;
        }

        if (!CurrentEnemyTarget)
        {
            FindTarget();
        }

        currentMovementTarget = companyPathfinding.GetMovementTarget();

        if (!inRange)
        {
            companyMover.MoveCompany(currentMovementTarget);
        }
        else if (!inFront)
        {
            companyMover.RotateCompany((CurrentEnemyTarget.transform.position - transform.position).normalized);
        }
        else if(companyMover.Moving)
        {
            companyMover.StopCompany();
        }
    }

    void FindTarget()
    {
        GameObject newTarget = null;
        FindEnemies();
        if (!CompanyFormations.DetermineTarget(transform.position, enemiesList, PrimaryTargetingMode,
                ref newTarget))
        {
            if (!CompanyFormations.DetermineTarget(transform.position, enemiesList, SecondaryTargetingMode,
                    ref newTarget))
            {
                if (!CompanyFormations.DetermineTarget(transform.position, enemiesList,
                        CompanyFormations.TargetingMode.ClosestSquad,
                        ref newTarget))
                    return;
            }
        }

        CurrentEnemyTarget = newTarget;
    }

    void MovementStateMachine()
    {
        inRange = AreEnemiesInRange();
        inFront = AreEnemiesInFront();
    }

    void CheckMelee()
    {
        foreach (GameObject enemyCompany in enemiesList)
        {
            if(!enemyCompany)
                continue;
            if ((enemyCompany.transform.position - transform.position).sqrMagnitude <= meleeRange * meleeRange)
            {
                InMelee = true;
                return;
            }
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
                        MeleeCompany = true;
                    else
                    {
                        RangedCompany = true;
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

    public float AverageHealth()
    {
        float total = 0f;
        foreach (GameObject model in models)
        {
            total += model.GetComponent<Attributes>().CurrentHp;
        }
        return total / models.Count;
    }

    public void RemoveModel(GameObject model)
    {
        models.Remove(model);
        if(models.Count == 0)
        {
            RemoveCompany();
        }
    }

    public float GetMorale()
    {
        return currentMorale;
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

    public float AverageEndurance()
    {
        float total = 0f;
        foreach (GameObject model in models)
        {
            total += model.GetComponent<Attributes>().GetEndurance();
        }
        return total / models.Count;
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
        companyMover.Arrangement = CompanyFormations.GetArrangement(formation);
        CompanyFormations.TargetingMode[] possibleTargets = CompanyFormations.GetTargetingOptions(formation, Team);
        companyPathfinding = gameObject.AddComponent(CompanyFormations.GetCompanyPathfinder(formation)) as CompanyPathfinding;
        if (possibleTargets == null)
            return;
        PrimaryTargetingMode = possibleTargets[0];
        if(possibleTargets.Length > 1)
            SecondaryTargetingMode = possibleTargets[1];
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
