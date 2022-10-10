using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Company_UI : MonoBehaviour, IPointerClickHandler
{
    Canvas canvas;
    public bool selected = false;
    public bool done = false;

    private void Awake()
    {
        if (canvas == null)
            canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        canvas.GetComponent<FormationOptions>().Company = null;
        selected = true;
        canvas.GetComponent<FormationOptions>().Company = this.gameObject.GetComponent<Company>();
    }
}
