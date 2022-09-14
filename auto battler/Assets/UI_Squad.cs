using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DapperDino.TooltipUI;


public class UI_Squad : MonoBehaviour, IDropHandler
{ 
    protected Item[] items = new Item[4];
    int maxSlots = 4;
    int availableSlots = 4;
    int availableEnergy = 6;
    
    public bool IsItemAccepted(GameObject item)
    {
        var item_ = item.GetComponent<ItemButton>().Item;
        if (availableSlots == 0)
            return false;
        if (availableEnergy < item_.EnergyCost)
            return false;
        for (int i = 0; i < maxSlots-availableSlots; i++)
        {
            if(items[i].Type == item_.Type)
            {
                return false;
            }
        }
        return true;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Given to Squad");
        if (eventData.pointerDrag != null && IsItemAccepted(eventData.pointerDrag))
        {
            RectTransform thisItem = eventData.pointerDrag.GetComponent<RectTransform>();
            float rescaleFactor = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x / thisItem.sizeDelta.x;
            thisItem.localScale = new Vector3(rescaleFactor, rescaleFactor, 1);
            thisItem.position = transform.GetChild(4-availableSlots).position;
            thisItem.SetParent(transform.GetChild(4-availableSlots).transform);
            items[maxSlots - availableSlots] = eventData.pointerDrag.GetComponent<ItemButton>().Item;
            availableSlots--;
            availableEnergy -= eventData.pointerDrag.GetComponent<ItemButton>().Item.EnergyCost;
        }
    }

    void RemoveItems(Item item)
    {
        int i;
        availableEnergy += item.EnergyCost;
        availableSlots++;
        for (i = 0; items[i] != item; i++){}
        items[i] = null;
    }
}
