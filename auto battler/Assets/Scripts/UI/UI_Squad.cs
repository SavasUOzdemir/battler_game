using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DapperDino.TooltipUI;
using System.Linq;



public class UI_Squad : MonoBehaviour, IDropHandler
{
    [SerializeField] protected Item[] items = new Item[4];
    [SerializeField] int maxSlots = 4;
    [SerializeField] int availableSlots = 4;
    [SerializeField] int availableEnergy = 6;
    [SerializeField] Squad squad;

    private void Awake()
    {
        UpdateItemObject();
    }
    public bool IsItemAccepted(GameObject item)
    {
        var item_ = item.GetComponent<ItemButton>().Item;
        if (availableSlots == 0)
            return false;
        if (availableEnergy < item_.EnergyCost)
            return false;
        for (int i = 0; i < maxSlots - availableSlots; i++)
        {
            if (items[i].Type == item_.Type)
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
            thisItem.position = transform.GetChild(4 - availableSlots).position;
            thisItem.SetParent(transform.GetChild(4 - availableSlots).transform);
            items[maxSlots - availableSlots] = eventData.pointerDrag.GetComponent<ItemButton>().Item;
            availableSlots--;
            availableEnergy -= eventData.pointerDrag.GetComponent<ItemButton>().Item.EnergyCost;
            UpdateItemObject();
        }
    }

    void RemoveItems(Item item)
    {
        int i;
        availableEnergy += item.EnergyCost;
        availableSlots++;
        for (i = 0; items[i] != item; i++) { }
        items[i] = null;
        UpdateArray(items);
        UpdateItemObject();
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
    void UpdateItemObject()
    {
        for (int j = 0; j < maxSlots; j++)
        {
            squad.heldItems[j] = items[j];
        }
        squad.AvailableSlots = availableSlots;
        squad.CurrentEnergy = availableEnergy;
    }


    void UpdateArray(Item[] items)
    {
        if (availableSlots < maxSlots)
            for (int i = 0; i + 1 < items.Length; i++)
            {
                if (items[i] == null)
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
}
