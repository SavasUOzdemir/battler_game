using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace DapperDino.TooltipUI
{
    public class Tooltip_Manager : MonoBehaviour
    {
        public static bool cursorOnTooltip;
        public static List<RectTransform> rectTransforms = new List<RectTransform>();
        [SerializeField]GameObject firstRectTransform;    
       
        public static Tooltip_Manager Instance
        {
            get;
            private set;
        }
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
            rectTransforms.Add(firstRectTransform.GetComponent<RectTransform>());
        }

        public void HideLastTooltipObjectAndDestroyRest()
        {
            rectTransforms[0].gameObject.SetActive(false);
            int i = 1;
            while(i <= rectTransforms.Count - 1)
            {
                Destroy(rectTransforms[i].gameObject);
                rectTransforms.RemoveAt(i);
            } 
        }
       
    }
}