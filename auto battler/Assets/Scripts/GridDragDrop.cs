using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class GridDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler/*, IDropHandler*/
{
    GameObject objectToSwapWith;
    Vector3 startPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        objectToSwapWith = EventSystem.current.currentSelectedGameObject;
        int indexOfObject = objectToSwapWith.transform.GetSiblingIndex();
        int indexOfDrag = gameObject.transform.GetSiblingIndex();
        objectToSwapWith.transform.SetSiblingIndex(indexOfDrag);
        gameObject.transform.SetSiblingIndex(indexOfObject);
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent.GetComponent<RectTransform>());
        gameObject.GetComponent<Image>().raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {        
        gameObject.transform.GetComponent<RectTransform>().anchoredPosition += eventData.delta / gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Canvas>().scaleFactor;
    }
}
