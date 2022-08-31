using UnityEngine.EventSystems;
using UnityEngine;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        TooltipManager._instance.SetAndShowTooltip(message);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        TooltipManager._instance.HideTooltip();
    }
}
