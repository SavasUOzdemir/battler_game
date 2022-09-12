using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    CanvasGroup canvasGroup_;
    private RectTransform rectTransform_;
    [SerializeField] Canvas canvas_;

    private void Awake()
    {
        rectTransform_ = GetComponent<RectTransform>();
        canvasGroup_= GetComponent<CanvasGroup>();

    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform_.anchoredPosition += eventData.delta / canvas_.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup_.alpha= .6f;
        canvasGroup_.blocksRaycasts = false;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup_.alpha = 1f;
        canvasGroup_.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}
