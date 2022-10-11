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
    [SerializeField] string formation_ = null;
    List<string> targetOpts;
    Company company;

    public List<string> TargetOptions { get { return targetOpts; }}
    public string Formation
    {
        get
        {
            return formation_;
        }
        set
        {
            formation_ = value;
            UpdateTargetingOptions(formation_);
            Debug.Log("Formation set to: " + formation_);
        }
    }

    private void Awake()
    {
        buttons = new GameObject[8] { closestSquad, lowestHealth, lowestMorale, lowestEndurance, rangedSquads, flyingSquads, highestHP, protective };
        DisableAllButtons();
    }


    void DisableAllButtons()
    {
        foreach (GameObject button in buttons)
            button.SetActive(false);
    }

    public List<string> UpdateTargetingOptions(string formation)
    {
        targetOpts = CompanyFormations.GetTargetingOptions(formation, 0);
        UpdateButtons(targetOpts);
        return targetOpts;
    }

    void UpdateButtons(List<string> options)
    {
        DisableAllButtons();
        if (options != null)
            foreach (var go in buttons)
                if (options.Contains(go.GetComponent<TargetingModeHook>().TargetingModeName))
                    go.SetActive(true);
    }
}
