using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Company_UI : MonoBehaviour
{
    [SerializeField] GameObject formationButtons;
    [SerializeField] GameObject targetingButtons;

    public bool selected = false;
    public bool done = false;
    Company company;
    GameObject[] allCompanies;
    [SerializeField] Camera camera_;
    [SerializeField] RaycastHit hit;
    [SerializeField] string formation = null;
    [SerializeField] string targeting_primary = null;
    [SerializeField] string targeting_secondary = null;


    private void Awake()
    {
        company = gameObject.GetComponent<Company>();
        allCompanies = GameObject.FindGameObjectsWithTag("Company");
        formation = company.Formation;
    }

    public string Formation
    {
        get {return formation; } 
        set {formation = value; }
    }
    public string Targeting_Primary
    {
        get { return targeting_primary; }
        set { targeting_primary = value; }
    }
    public string Targeting_Secondary
    {
        get { return targeting_secondary; }
        set { targeting_secondary = value; }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //add "!gamestarted" check here//
        {
            RaycastHit[] hits;
            Ray ray = camera_.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if (hit.transform.gameObject.layer == 8 && hit.transform == gameObject.transform)
                {
                    Debug.Log("You hit " + hit.transform.name);
                    break;
                }
            }
            if (!company.GameStarted && hit.transform == gameObject.transform)
            {
                formationButtons.GetComponent<FormationOptions>().Company_UI = null;
                formationButtons.GetComponent<FormationOptions>().Company = null;

                UnselectAll();
                selected = true;
                formationButtons.GetComponent<FormationOptions>().Company_UI = hit.transform.GetComponent<Company_UI>();
                formationButtons.GetComponent<FormationOptions>().Company = hit.transform.GetComponent<Company>();
                formationButtons.SendMessage("UpdateFormationOptions",null,SendMessageOptions.RequireReceiver);
                formation = company.Formation;
                if (targetingButtons.GetComponent<TargetingOptions>().Formation != formation)
                    targetingButtons.GetComponent<TargetingOptions>().Formation = formation;
            }
        }
    }
    void UnselectAll()
    {
        foreach (GameObject go in allCompanies)
            go.GetComponent<Company_UI>().selected = false;
    }
}
