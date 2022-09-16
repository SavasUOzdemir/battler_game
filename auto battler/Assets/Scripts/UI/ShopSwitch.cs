using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSwitch : MonoBehaviour
{
    [SerializeField] GameObject squadTab;
    [SerializeField] GameObject shopTab;
    [SerializeField] TMP_Text unitsText;
    public void OnButtonDown()
    {
        if (squadTab.activeSelf)
        {
            squadTab.SetActive(false);
            shopTab.SetActive(true);
            unitsText.text = "Shop";
        }
        else if (shopTab.activeSelf)
        {
            shopTab.SetActive(false);
            squadTab.SetActive(true);
            unitsText.text = "Squads";
        }
    }
}
