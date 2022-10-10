using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingOptions : MonoBehaviour
{
    [SerializeField] GameObject closestSquad;
    [SerializeField] GameObject lowestHealth;
    [SerializeField] GameObject lowestMorale;
    [SerializeField] GameObject lowestEndurance;
    [SerializeField] GameObject rangedSquads;
    [SerializeField] GameObject flyingSquads;
    [SerializeField] GameObject highestHP;
    [SerializeField] GameObject protective;
    GameObject[] buttons;
    List<string> targetOpts;
    private void Awake()
    {
        buttons = new GameObject[8] {closestSquad, lowestHealth, lowestMorale, lowestEndurance, rangedSquads, flyingSquads, highestHP, protective};
        DisableAll();
    }


    void DisableAll()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

    void UpdateTargetingOptions(string formation)
    {
        targetOpts = CompanyFormations.GetTargetingOptions(formation, 0);
    }
}
