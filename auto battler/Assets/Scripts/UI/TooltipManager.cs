using System.Collections;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;
    public TextMeshProUGUI textComponent;
    public RectTransform rectTransform;
    bool fixInPosition = false;
    float fixCountDown = 2.0f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fixInPosition) { 
        Vector2 position = Input.mousePosition;
        float pivotX = position.x / Screen.width - .2f;
        float pivotY = position.y / Screen.height + .35f;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
        }
    }
    public void SetAndShowTooltip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        StartCoroutine("fixPosCountdown");
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }
    private IEnumerator fixPosCountdown(float waitTime)
    {
        yield return new WaitForSeconds(fixCountDown);
        fixInPosition = true;
    }
}
