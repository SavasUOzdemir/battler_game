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
        [SerializeField] private RectTransform mainBackground;
        [SerializeField] private Item item;
        bool pointerOut = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip_Manager.cursorOnTooltip = false;
            pointerOut = false;
            DisplayInfo displayinfo = mainBackground.GetComponent<DisplayInfo>();
            displayinfo.DisplayItemInfo(item, mainBackground);
            StartCoroutine(nameof(FixRectInPosition));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerOut = true;
            StartCoroutine(nameof(CheckTooltipsAfterAFrame));
            StartCoroutine(nameof(FreeRectPosition));
        }
        IEnumerator CheckTooltipsAfterAFrame()
        {
            DisplayInfo displayinfo = mainBackground.GetComponent<DisplayInfo>();
            yield return new WaitForEndOfFrame();
            if (Tooltip_Manager.cursorOnTooltip == false)
                displayinfo.HideInfo();
        }
        IEnumerator FixRectInPosition()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            if (pointerOut == false)
            {
                tooltipPopup.fixPosition = true;
            }
        }

        IEnumerator FreeRectPosition()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            if(Tooltip_Manager.cursorOnTooltip==false)
                tooltipPopup.fixPosition = false;
        }
    }
}

