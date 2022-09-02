using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dummyscript : MonoBehaviour
{
    //biþiyleri denemek için bu scripti kullanabilirsin içi anlamsýz.

    private TMP_Text m_TextComponent;
    private Transform m_Transform;
    private Camera m_Camera;
    private Canvas m_Canvas;
    void Awake()
    {
        // Get a reference to the text component.
        m_TextComponent = gameObject.GetComponent<TMP_Text>();
        // Get a reference to the camera rendering the text taking into consideration the text component type.
        if (m_TextComponent.GetType() == typeof(TextMeshProUGUI))
        {
            m_Canvas = gameObject.GetComponentInParent<Canvas>();
            if (m_Canvas != null)
            {
                if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    m_Camera = null;
                else
                    m_Camera = m_Canvas.worldCamera;
            }
        }
        else
            m_Camera = Camera.main;
    }

    private void Start()
    {
        ShowWords();
    }

    void ShowWords()
    {
        TMP_TextInfo textInfo1 = m_TextComponent.textInfo;
        Debug.Log(textInfo1.wordInfo.Length);
        for (int i = 0; i < textInfo1.wordInfo.Length; i++)
        {
            TMP_WordInfo wInfo = textInfo1.wordInfo[i];
            Debug.Log(wInfo.GetWord());
        }
    }
}


