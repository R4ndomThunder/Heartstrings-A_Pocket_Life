using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Canvas canvas;

    void OnValidate()
    {
        Setup();
    }

    void Awake()
    {
        Setup();
    }

    void Setup()
    {
        if (rect != null)
            rect = transform.parent.GetComponent<RectTransform>();

        if (canvas == null)
            canvas = transform.GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rect.SetAsLastSibling();
    }
}
