using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Given to Squad");
        if(eventData.pointerDrag != null)
        {
            RectTransform thisItem = eventData.pointerDrag.GetComponent<RectTransform>();
            float rescaleFactor = GetComponent<RectTransform>().sizeDelta.x / thisItem.sizeDelta.x;
            thisItem.localScale = new Vector3(rescaleFactor, rescaleFactor, 1);
            thisItem.position = transform.position;
            thisItem.SetParent(this.transform);
        }
    }
}
