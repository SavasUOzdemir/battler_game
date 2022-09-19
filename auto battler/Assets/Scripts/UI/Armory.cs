using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DapperDino.TooltipUI;


public class Armory : MonoBehaviour, IDropHandler
{
    GridLayoutGroup gr;
    private void Awake()
    {
        gr = GetComponent<GridLayoutGroup>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<ItemButton>() != null)
        {
            if (eventData.pointerDrag.transform.parent.GetComponent<UI_Shop>() == null)
            {
                RectTransform thisItem = eventData.pointerDrag.GetComponent<RectTransform>();
                float rescaleFactor = gr.cellSize.x / thisItem.sizeDelta.x;
                thisItem.localScale = new Vector3(rescaleFactor, rescaleFactor, rescaleFactor);
                thisItem.SetParent(this.transform);
                thisItem.SetAsFirstSibling();
                Debug.Log("Item Stored in Armory");
            }
            else if (eventData.pointerDrag.transform.parent.GetComponent<UI_Shop>() != null && eventData.pointerDrag.GetComponent<ItemButton>().Item.SellPrice <= InventorySingleton.instance.CurrentGold)
            {
                RectTransform thisItem = eventData.pointerDrag.GetComponent<RectTransform>();
                //float rescaleFactor = gr.cellSize.x / thisItem.sizeDelta.x;
                thisItem.localScale = Vector3.one;
                thisItem.SetParent(this.transform);
                thisItem.SetAsFirstSibling();
                InventorySingleton.instance.CurrentGold -= eventData.pointerDrag.GetComponent<ItemButton>().Item.SellPrice;
                Debug.Log("Item Bought");
            }
            else if (eventData.pointerDrag.GetComponent<ItemButton>().Item.SellPrice > InventorySingleton.instance.CurrentGold)
                Debug.Log("Insufficient Funds");
        }

    }
}
