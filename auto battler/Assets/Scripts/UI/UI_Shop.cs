using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DapperDino.TooltipUI;
using System.Linq;



public class UI_Shop : MonoBehaviour, IDropHandler
{
    [SerializeField] protected Item[] items = new Item[100];
    [SerializeField] GameObject InventoryPanel;
    
 
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Sold");
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<ItemButton>() != null)
        {
            RectTransform thisItem = eventData.pointerDrag.GetComponent<RectTransform>();
            float rescaleFactor = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x / thisItem.sizeDelta.x;
            thisItem.localScale = new Vector3(rescaleFactor, rescaleFactor, 1);
            thisItem.SetParent(gameObject.transform);
            Item actualItem = eventData.pointerDrag.GetComponent<ItemButton>().Item;
            InventorySingleton.instance.CurrentGold+= actualItem.SellPrice/2;
        }
    }

    void RemoveItems(Item item)
    {
        int i;
        for (i = 0; items[i] != item; i++){}
        items[i] = null;
        UpdateArray(items);
        //SetImages(items);
    }

    //void SetImages(Item[] items)
    //{
    //    int i;
    //    for (i = 0; i < maxSlots-availableSlots; i++)
    //        transform.GetChild(i).GetComponent<Image>().sprite = items[i].Thumbnail;
    //    while (i<maxSlots)
    //    {
    //        transform.GetChild(i).GetComponent<Image>().sprite = null;
    //        i++;
    //    }
    //}

    void UpdateArray(Item[] items)
    {
        if (true)        
            for (int i = 0; i + 1 < items.Length; i++)            
                if (items[i]==null)
                {
                    items[i] = items[i + 1];
                    items[i + 1] = null;
                    if (transform.GetChild(i + 1).childCount == 1) 
                    {
                        transform.GetChild(i + 1).GetChild(0).SetParent(transform.GetChild(i));
                        transform.GetChild(i).transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
    }
}
