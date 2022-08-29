using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class clickhandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]TMP_Text text;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            int index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);

            if (index > -1)
            {
//                "ÝSTEDÝÐÝM ÞEYLERÝ YAP"
            }
        }
    }
}
