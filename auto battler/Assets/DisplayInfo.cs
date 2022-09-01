using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.TooltipUI
{
    public class DisplayInfo : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI infoText;
        [SerializeField] private GameObject popupCanvasObject;

        public void DisplayItemInfo(Item item, RectTransform popupObject_)
        {
            gameObject.SetActive(true);
            StringBuilder builder = new();

            builder.Append("<size=35>").Append(item.ColouredName).Append("</size>").AppendLine();
            builder.Append(item.GetTooltipInfoText());

            infoText.text = builder.ToString();

            if (popupCanvasObject.activeSelf == false)
                popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject_);
        }
        public void HideInfo()
        {
            gameObject.SetActive(false);
        }
    }
}
