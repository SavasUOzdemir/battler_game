using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace DapperDino.TooltipUI
{
    public class DisplayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private GameObject popupCanvasObject;
        [SerializeField] private bool cursorOnThis;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            cursorOnThis = true;
            WhileMouseIsOver();
            Tooltip_Manager.cursorOnTooltip = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cursorOnThis = false;
            Tooltip_Manager.cursorOnTooltip = false;
            StartCoroutine(nameof(CheckTooltipsAfterAFrame));
        }

        private void WhileMouseIsOver()
        {
            if (cursorOnThis)
                if (Tooltip_Manager.rectTransforms.Count > 1)
                    if (Tooltip_Manager.rectTransforms[Tooltip_Manager.rectTransforms.Count - 1] != this.gameObject.GetComponent<RectTransform>())
                    {
                        int i = Tooltip_Manager.rectTransforms.IndexOf(this.gameObject.GetComponent<RectTransform>()) + 1;
                        while (i <= Tooltip_Manager.rectTransforms.Count - 1)
                        {
                            Destroy(Tooltip_Manager.rectTransforms[i].gameObject);
                            Tooltip_Manager.rectTransforms.RemoveAt(i);
                        }
                    }
        }

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

        public void DestroyThis()
        {
            Destroy(this.gameObject);
        }

        IEnumerator CheckTooltipsAfterAFrame()
        {
            yield return new WaitForSecondsRealtime(0.21f);
            if (Tooltip_Manager.cursorOnTooltip == false && cursorOnThis == false)
                Tooltip_Manager.Instance.HideLastTooltipObjectAndDestroyRest();
        }
    }
}
