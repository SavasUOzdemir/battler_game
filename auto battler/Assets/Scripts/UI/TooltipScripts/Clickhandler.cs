using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class Clickhandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]TMP_Text text;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            int index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);

            if (index > -1)
            {
                InstantiateRectTransform();
            }
        }
    }

    void InstantiateRectTransform()
    {
        var newRectTransform = Instantiate(this.transform.parent, Input.mousePosition, Quaternion.identity);
        newRectTransform.SetParent(this.transform.parent.parent);
    }
}
