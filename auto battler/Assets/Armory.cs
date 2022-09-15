using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Armory : MonoBehaviour, IDropHandler
{
    GridLayoutGroup gr;
    private void Awake()
    {
        gr = GetComponent<GridLayoutGroup>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Stored in Armory");
        if (eventData.pointerDrag != null)
        {
            RectTransform thisItem = eventData.pointerDrag.GetComponent<RectTransform>();
            float rescaleFactor = gr.cellSize.x / thisItem.sizeDelta.x;
            thisItem.localScale = new Vector3(rescaleFactor, rescaleFactor, rescaleFactor);
            thisItem.SetParent(this.transform);
            thisItem.SetAsFirstSibling();
        }
    }
}
