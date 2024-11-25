using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    string url = "https://samuelepadalino.dev";

    TextMeshProUGUI text;
    string textValue = string.Empty;

    [SerializeField, ColorUsage(false, false)]
    Color hoverColor = Color.white;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        textValue = text.text;
        text.text = $"{textValue}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
        text.text = $"<u>{textValue}</u>";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.white;
        text.text = $"{textValue}";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Application.OpenURL(url);
        text.color = Color.white;
        text.text = $"{textValue}";
    }
}
