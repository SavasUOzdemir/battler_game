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
        //[SerializeField] int extraPopupAmount = 0;


        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerOut = false;
            DisplayInfo displayinfo = mainBackground.GetComponent<DisplayInfo>();
            displayinfo.DisplayItemInfo(item, mainBackground);
            StartCoroutine("FixRectInPosition");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerOut = true;
            DisplayInfo displayinfo = mainBackground.GetComponent<DisplayInfo>();
            displayinfo.HideInfo();
        }

        IEnumerator FixRectInPosition()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            //if (new PointerEventData(EventSystem.current).hovered.Contains(this.gameObject))
            if (pointerOut == false)
            {
                tooltipPopup.fixPosition = true;
            }
        }
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

