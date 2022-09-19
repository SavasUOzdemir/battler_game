using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.TooltipUI;

public class InventorySingleton : MonoBehaviour
{
    public static InventorySingleton instance;
    [SerializeField] GameObject goldUI;
    [SerializeField] GameObject armory;
    [SerializeField] GameObject shop;
    [SerializeField] Squad[] squads = new Squad[10];

    [SerializeField] private int currentGold = 0;
    List<Item> armoryItems;
    List<Item> squadItems;
    List<Item> shopItems;

    public int CurrentGold
    { 
        get 
        { 
            return currentGold; 
        }
        set
        {
            currentGold = value;
            goldUI.gameObject.SendMessage("UpdateGoldOnScreen",SendMessageOptions.RequireReceiver);
        }
    }
    //public void UpdateItem()
    //{
    //    foreach (Squad squad in squads)
    //    {
    //        for (int i = 0; i < squad.heldItems.Length; i++)
    //        {
    //            if (squad.heldItems[i]!= null)
    //            {
    //                InventorySingleton.instance.squadItems.Add(squad.heldItems[i]);
    //            }
    //        }
    //    }

    //    for (int i = 0; i < armory.transform.childCount; i++)
    //    {
    //        if (armory.transform.GetChild(i).GetComponent<ItemButton>() != null)
    //        {
    //            Debug.Log(armory.transform.GetChild(i).GetComponent<ItemButton>().Item);
    //            InventorySingleton.instance.armoryItems.Add(armory.transform.GetChild(i).GetComponent<ItemButton>().GetComponent<Item>());
    //            continue;
    //        }
    //        else break;
    //    }

    //    for (int i = 0; i < shop.transform.childCount; i++)
    //    {
    //        if (shop.transform.GetChild(i).GetComponent<ItemButton>().Item != null)
    //        {
    //            InventorySingleton.instance.shopItems.Add(shop.transform.GetChild(i).GetComponent<ItemButton>().Item);
    //            continue;
    //        }
    //        else break;
    //    }
    //}

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}