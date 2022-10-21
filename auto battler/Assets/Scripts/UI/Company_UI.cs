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

    /* Position squads before combat*/
    [SerializeField] float firstLeftClickTime;
    [SerializeField] float timeBetweenLeftClick = 0.5f;
    [SerializeField] bool isTimeCheckAllowed = true;
    [SerializeField] int leftClickNumber = 0;
    /* end position variables */

    private void Awake()
    {
        company = gameObject.GetComponent<Company>();
        allCompanies = GameObject.FindGameObjectsWithTag("Company");
        formation = company.Formation;
    }

    public string Formation
    {
        get { return formation; }
        set { formation = value; }
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
                formationButtons.SendMessage("UpdateFormationOptions", null, SendMessageOptions.RequireReceiver);
                formation = company.Formation;
                if (targetingButtons.GetComponent<TargetingOptions>().Formation != formation)
                    targetingButtons.GetComponent<TargetingOptions>().Formation = formation;
            }
        }

        if (Input.GetMouseButtonUp(0))
            leftClickNumber++;
        if (leftClickNumber == 1 && isTimeCheckAllowed)
        {
            firstLeftClickTime = Time.time;
            StartCoroutine(PositioningDoubleClick());
        }
    }
    void UnselectAll()
    {
        foreach (GameObject go in allCompanies)
            go.GetComponent<Company_UI>().selected = false;
    }

    IEnumerator PositioningDoubleClick()
    {
        isTimeCheckAllowed = false;
        while (Time.time < firstLeftClickTime + timeBetweenLeftClick)
        {
            if (leftClickNumber == 2)
            {
                RaycastHit[] hits;
                Ray ray = camera_.ScreenPointToRay(Input.mousePosition);

                hits = Physics.RaycastAll(ray);

                for (int i = 0; i < hits.Length; i++)
                {
                    hit = hits[i];
                    if (hit.transform.gameObject.layer == 8 && hit.transform == gameObject.transform)
                    {
                        hit.transform.GetComponent<Company>().MouseControl = !hit.transform.GetComponent<Company>().MouseControl;
                        break;
                    }
                }
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        leftClickNumber = 0;
        isTimeCheckAllowed = true;
    }
}
