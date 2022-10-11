using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Company : MonoBehaviour
{
    //Misc
    GameObject[] buffer = new GameObject[500];
    public List<GameObject> enemiesList = new();
    Vector3 fleeDir;
    [field: SerializeField] public bool GameStarted { get; set; } = true;
    [field: SerializeField] public float MeleeRange { get; private set; } = 3f;
    [SerializeField] bool debug = false;
    [field: SerializeField] public bool MouseControl { get; private set; } = false;

    //Model Data
    public GameObject modelPrefab;
    [field: SerializeField] public int ModelCount { get; set; } = 16;
    public List<GameObject> models = new();
    float modelColliderDia = 1;

    //Upgrades
    List<System.Type> modelUpgradeList = new();
    List<CompanyAction> companyActions = new();
    List<CompanyPassive> companyPassives = new();
    [SerializeField] float globalCooldown = 1f;
    float currentGlobalCooldown = 0f;

    //Formation, Targeting and Pathfinding
    [SerializeField] string formation = "Square";
    public GameObject CurrentEnemyTarget { get; private set; } = null;
    TargetingModeBehaviour primaryTargetingMode;
    TargetingModeBehaviour secondaryTargetingMode;
    CompanyMover companyMover;

    //AI STATE
    [field: SerializeField] public bool InMelee { get; private set; } = false;
    [SerializeField] public bool Moving => companyMover.Moving;
    [SerializeField] bool inRange = false;
    [SerializeField] bool inFront = false;
    float aiUpdateTime = 0.5f;
    float currentTime = 0;
    float attackArc = 60f;
    float aiFuse = 01f;
    [SerializeField] List<Company> inMeleeCompaniesList = new();
    [SerializeField] List<GameObject> inMeleeModels = new();

    //GAMEPLAY STATS
    [field: SerializeField] public int Team { get; set; } = 0;
    [SerializeField] float maxMorale = 100f;
    [SerializeField] float currentMorale;
    [SerializeField] bool broken = false;
    [SerializeField] float range = 0;
    [SerializeField] float rangeBuffer = 0.1f;
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
        companyMover.init();
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
            GetComponent<ArrangementBehaviour>().ArrangeModels(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCam)),
                companyMover.CurrentCompanyDir);
            for (int i = 0; i < ModelCount; i++)
            {
                models[i].transform.position = new Vector3(companyMover.ModelPositions[i].x, 0, companyMover.ModelPositions[i].z);
            }
        }

        if (!GameStarted)
            return;
        if (Time.timeSinceLevelLoad < aiFuse)
            return;

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
        
        if(currentTime <= 0)
        {
            MovementLoop();
            currentTime = aiUpdateTime;
        }
        currentTime -= Time.deltaTime;
    }

    private void Init() //Temp function for editor initialization
    {
        ChangeFormation(formation);
        currentMorale = maxMorale;
        ModelAttributes messagePar = new ModelAttributes(this, Team);
        GetComponent<ArrangementBehaviour>().ArrangeModels(transform.position,companyMover.CurrentCompanyDir);
        var newParent = new GameObject();
        for (int i = 0; i < ModelCount; i++)
        {
            models.Add(Instantiate(modelPrefab, companyMover.ModelPositions[i], Quaternion.identity, newParent.transform));
            models[i].GetComponent<Attributes>().SetCompany(messagePar);
        }
        modelColliderDia = models[0].GetComponent<CapsuleCollider>().radius * 2;
        companyMover.ModelColliderDia = modelColliderDia;
        GetComponent<ArrangementBehaviour>().ArrangeModels(transform.position, companyMover.CurrentCompanyDir);
        for (int i = 0; i < ModelCount; i++)
        {
            models[i].transform.position = companyMover.ModelPositions[i];
        }
        AttachUpgradesToModels();
    }

    bool IsCurrentTargetInRange()
    {
        return (CurrentEnemyTarget.transform.position - transform.position).sqrMagnitude < range * (1 - rangeBuffer) * range * (1 - rangeBuffer);
    }

    bool IsCurrentTargetInFront()
    {
        return (Vector3.Angle(CurrentEnemyTarget.transform.position - transform.position, companyMover.CurrentCompanyDir) < attackArc);
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

        MovementStateMachine();

        if (!inRange)
        {
            companyMover.MoveOnPath();
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
        if (!primaryTargetingMode.DetermineTarget(transform.position, ref newTarget))
        {
            if (!secondaryTargetingMode.DetermineTarget(transform.position, ref newTarget))
            {
                return;
            }
        }

        CurrentEnemyTarget = newTarget;
    }

    void MovementStateMachine()
    {
        inRange = IsCurrentTargetInRange();
        inFront = IsCurrentTargetInFront();
    }

    void OnTriggerEnter(Collider other)
    {
        Attributes otherAtt = other.GetComponent(typeof(Attributes)) as Attributes;
        Company otherCompany = otherAtt.GetCompany();
        if (!otherAtt || !otherCompany)
            return;
        if (otherAtt.GetTeam() != Team)
        {
            InMelee = true;
            inMeleeModels.Add(other.gameObject);
            if (!inMeleeCompaniesList.Contains(otherCompany))
                inMeleeCompaniesList.Add(otherCompany);
            otherAtt.onDeathExitMelee += ExitMelee;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        inMeleeModels.Remove(other.gameObject);
        Attributes otherAtt = other.GetComponent(typeof(Attributes)) as Attributes;
        if (!otherAtt)
           return;
        if (!inMeleeModels.Exists(x => ReferenceEquals((x.GetComponent(typeof(Attributes)) as Attributes).GetCompany(),otherAtt.GetCompany()))) 
            inMeleeCompaniesList.Remove(otherAtt.GetCompany());
        if (inMeleeCompaniesList.Count == 0)
            InMelee = false;
    }

    void ExitMelee(Company company, GameObject model)
    {
        inMeleeModels.Remove(model);
        if (!inMeleeModels.Exists(x => ReferenceEquals((x.GetComponent(typeof(Attributes)) as Attributes).GetCompany(), company)))
            inMeleeCompaniesList.Remove(company);
        if (inMeleeCompaniesList.Count == 0)
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
            companyActions.Sort((x, y) => x.GetPrio().CompareTo(y.GetPrio()));
        }
        else if (type.IsSubclassOf(typeof(CompanyPassive)))
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

    void KillCompany()
    {
        BattlefieldManager.KillCompany(gameObject);
        gameObject.SetActive(false);
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
            KillCompany();
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

    public void ChangeFormation(string _formation)
    {
        formation = _formation;
        Destroy(GetComponent(typeof(ArrangementBehaviour)));
        companyMover.arranger = gameObject.AddComponent(System.Type.GetType(CompanyFormations.GetArrangement(_formation))) as ArrangementBehaviour;
        List<string> possibleTargets = CompanyFormations.GetTargetingOptions(_formation, Team);
        Destroy(GetComponent(typeof(CompanyPathfinderBehaviour)));
        gameObject.AddComponent(System.Type.GetType(CompanyFormations.GetCompanyPathfinder(_formation)));
        if (possibleTargets == null)
            return;
        ChangePrimaryTargetingMode(possibleTargets[0]);
        if(possibleTargets.Count > 1)
            ChangeSecondaryTargetingMode(possibleTargets[1]);
    }

    public void ChangePrimaryTargetingMode(string newTargetingMode)
    {
        if(primaryTargetingMode)
            Destroy(primaryTargetingMode);
        primaryTargetingMode = gameObject.AddComponent(System.Type.GetType(newTargetingMode)) as TargetingModeBehaviour;
    }

    public void ChangeSecondaryTargetingMode(string newTargetingMode)
    {
        if (secondaryTargetingMode)
            Destroy(secondaryTargetingMode);
        secondaryTargetingMode = gameObject.AddComponent(System.Type.GetType(newTargetingMode)) as TargetingModeBehaviour;
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