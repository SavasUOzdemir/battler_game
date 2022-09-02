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
        private int m_lastWordIndex = -1;

        TMP_TextEventHandler.WordSelectionEvent onWordSelection
        {
            get { return m_OnWordSelection; }
            set { m_OnWordSelection = value; }
        }
        TMP_TextEventHandler.WordSelectionEvent m_OnWordSelection = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                int index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, null);
                if (wordIndex != -1 && wordIndex != m_lastWordIndex)
                {
                    m_lastWordIndex = wordIndex;

                    // Get the information about the selected word.
                    TMP_WordInfo wInfo = text.textInfo.wordInfo[wordIndex];
                    string suffix = wInfo.GetWord();
                    Debug.Log(suffix);
                    item = Resources.Load<Consumable>("Items/Item_Consumable_" + suffix);
                }
                if (index > -1)
                    InstantiateRectTransform(item);

            }
        }
        void InstantiateRectTransform(Item item)
        {
            Transform newRectTransform = Instantiate(this.transform.parent, Input.mousePosition, Quaternion.identity);
            newRectTransform.SetParent(this.transform.parent.parent);
            DisplayInfo displayInfo_ = newRectTransform.GetComponent<DisplayInfo>();
            displayInfo_.DisplayItemInfo(item, newRectTransform.GetComponent<RectTransform>());
            Tooltip_Manager.rectTransforms.Add(newRectTransform.GetComponent<RectTransform>());
        }
    }
}
