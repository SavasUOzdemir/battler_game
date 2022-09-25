using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    string goldText;

    private void Start()
    {
        UpdateGoldOnScreen();
    }
    void UpdateGoldOnScreen()
    {
        gameObject.GetComponent<TMP_Text>().text = "Gold: " + InventorySingleton.instance.CurrentGold.ToString();
        //InventorySingleton.cs sends message using the name of this, so either try to refrain from changing it or change it from InventorySingleton.cs file too.
    }
}