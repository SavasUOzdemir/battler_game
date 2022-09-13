using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler/*, IDropHandler*/
{
    CanvasGroup canvasGroup_;
    private RectTransform rectTransform_;
    [SerializeField] Canvas canvas_;
    Image img_;
    GameObject ogParent;
    Vector3 localPos;

    private void Awake()
    {
        rectTransform_ = GetComponent<RectTransform>();
        canvasGroup_= GetComponent<CanvasGroup>();
        img_ = GetComponent<Image>();

    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform_.anchoredPosition += eventData.delta / canvas_.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        localPos = transform.localPosition;
        ogParent = transform.parent.gameObject;
        Debug.Log("OnBeginDrag");
        canvasGroup_.alpha= .6f;
        img_.maskable = false;
        canvasGroup_.blocksRaycasts = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup_.alpha = 1f;
        canvasGroup_.blocksRaycasts = true;
        img_.maskable = true;
        if (this.transform.parent.gameObject == ogParent)
            transform.localPosition = localPos;
        if (this.transform.parent.gameObject != ogParent)
            transform.parent.parent.SendMessage("RegisterItems", SendMessageOptions.RequireReceiver);        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}
