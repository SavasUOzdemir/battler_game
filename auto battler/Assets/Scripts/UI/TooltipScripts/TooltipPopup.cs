using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.TooltipUI
{
    public class TooltipPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvasObject;
        [SerializeField] private RectTransform popupObject;
        public TextMeshProUGUI infoText;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float padding;
        public bool fixPosition;

        private Canvas popupCanvas;

        private void Awake()
        {
            popupCanvas = popupCanvasObject.GetComponent<Canvas>();
        }

        private void Update()
        {
            if(!fixPosition)
                FollowCursor();
        }

        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector3 newPos = Input.mousePosition + offset;
            newPos.z = 0f;
            float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
            if (rightEdgeToScreenEdgeDistance < 0)
            {
                newPos.x += rightEdgeToScreenEdgeDistance;
            }
            float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
            if (leftEdgeToScreenEdgeDistance > 0)
            {
                newPos.x += leftEdgeToScreenEdgeDistance;
            }
            float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
            if (topEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += topEdgeToScreenEdgeDistance;
            }
            float bottomEdgeToScreenEdgeDistance = Screen.height - (newPos.y - popupObject.rect.height * popupCanvas.scaleFactor) + padding;
            if (bottomEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += bottomEdgeToScreenEdgeDistance;
            }
            popupObject.transform.position = newPos;
        }
    }
}
