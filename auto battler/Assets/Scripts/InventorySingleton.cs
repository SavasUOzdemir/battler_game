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
    [SerializeField] List<Item> armoryItems = new();
    [SerializeField] List<Item> squadItems = new();
    [SerializeField] List<Item> shopItems = new();
    Dictionary<Squad, Item[]> itemsDictionary = new();

    public int CurrentGold
    {
        get
        {
            return currentGold;
        }
        set
        {
            currentGold = value;
            goldUI.gameObject.SendMessage("UpdateGoldOnScreen", SendMessageOptions.RequireReceiver);
        }
    }

    //void TestDictionary()
    //{
    //    foreach (Squad squad in squads)
    //        for (int i = 0; i < 4; i++)
    //            Debug.Log(itemsDictionary[squad][i]);
    //}

    void UpdateDictionary()
    {
        itemsDictionary.Clear();
        foreach (Squad squad in squads)        
            itemsDictionary.Add(squad, squad.heldItems);
    }

    void UpdateTotalSquadItemList()
    {
        squadItems.Clear();
        foreach (Squad squad in squads)
        {
            for (int i = 0; i < squad.heldItems.Length; i++)
            {
                if (squad.heldItems[i] != null)
                {
                    squadItems.Add(squad.heldItems[i]);
                }
            }
        }
    }

    void UpdateArmoryItemList()
    {
        armoryItems.Clear();
        for (int i = 0; i < armory.transform.childCount; i++)
        {
            if (armory.transform.GetChild(i).GetComponent<ItemButton>() != null)
            {
                if (armory.transform.GetChild(i).GetComponent<ItemButton>().Item != null && armoryItems != null)
                {
                    armoryItems.Add(armory.transform.GetChild(i).GetComponent<ItemButton>().Item);
                }
                continue;
            }
            else break;
        }
    }

    void UpdateShopItemList()
    {
        shopItems.Clear();
        for (int i = 0; i < shop.transform.childCount; i++)
        {
            if (shop.transform.GetChild(i).GetComponent<ItemButton>() != null)
            {

                if (shop.transform.GetChild(i).GetComponent<ItemButton>().Item != null)
                {
                    shopItems.Add(shop.transform.GetChild(i).GetComponent<ItemButton>().Item);
                    continue;
                }
            }
            else break;
        }
    }


    public void UpdateItem()
    {
        UpdateDictionary();
        UpdateTotalSquadItemList();
        UpdateArmoryItemList();
        UpdateShopItemList();
        //TestDictionary();
    }

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