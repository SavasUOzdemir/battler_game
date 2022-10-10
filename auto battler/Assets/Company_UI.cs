using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Company_UI : MonoBehaviour
{
    Canvas canvas;
    public bool selected = false;
    public bool done = false;
    Company company;
    GameObject[] allCompanies;
    [SerializeField] Camera camera_;
    [SerializeField] Transform objectHit;


    private void Awake()
    {
        if (canvas == null)
            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        company = gameObject.GetComponent<Company>();
        allCompanies = GameObject.FindGameObjectsWithTag("Company");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = camera_.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
                objectHit = hit.transform;
        }

        if (Input.GetButtonDown("Fire1") && !company.GameStarted && objectHit == gameObject.transform)
        {
            Debug.Log("Clicked on Company");
            canvas.GetComponent<FormationOptions>().Company_UI = null;
            UnselectAll();
            selected = true;
            canvas.GetComponent<FormationOptions>().Company_UI = objectHit.GetComponent<Company_UI>();
        }
    }

    void UnselectAll()
    {
        foreach (GameObject go in allCompanies)
            go.GetComponent<Company_UI>().selected = false;
    }
}