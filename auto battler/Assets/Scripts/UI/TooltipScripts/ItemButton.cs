using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace DapperDino.TooltipUI
{
    public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipPopup tooltipPopup;
        [SerializeField] private Item item;
        //[SerializeField] int extraPopupAmount = 0;

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipPopup.DisplayInfo(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            tooltipPopup.HideInfo();

        }
        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    Debug.Log("klikd");
        //    if (eventData.button == PointerEventData.InputButton.Left)
        //    {
        //        Debug.Log(TMP_TextUtilities.FindIntersectingLink(tooltipPopup.infoText, Input.mousePosition, null));
        //    }
        //}
    }
}
