using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
namespace DapperDino.TooltipUI
{
    public class Clickhandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TMP_Text text;
        [SerializeField] private TooltipPopup tooltipPopup;
        Item item;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                int index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
                item = Resources.Load<Consumable>("Items/Item_Consumable_Beating");
                if (index > -1)
                {
                    InstantiateRectTransform(item);
                }
            }
        }

        void InstantiateRectTransform(Item item)
        {
            var newRectTransform = Instantiate(this.transform.parent, Input.mousePosition, Quaternion.identity);
            newRectTransform.SetParent(this.transform.parent.parent);
            DisplayInfo displayInfo_ = newRectTransform.GetComponent<DisplayInfo>();
            displayInfo_.DisplayItemInfo(item, newRectTransform.GetComponent<RectTransform>());
        }
    }
}
