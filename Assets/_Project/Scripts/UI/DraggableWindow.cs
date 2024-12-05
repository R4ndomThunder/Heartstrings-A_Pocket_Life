using RTDK.Logger;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
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

    void Start()
    {
        var parent = transform.parent.gameObject;

        var winData = SaveSystem.gameData.GetWindowState(parent.name);
        if (winData != null)
        {
            rect.anchoredPosition = new(winData.x, winData.y);
            parent.SetActive(winData.isOpen);
        }
    }

    void OnEnable()
    {
        var parent = transform.parent.gameObject;
        SaveSystem.gameData.SaveWindowState(parent.name, rect.anchoredPosition, parent.activeSelf);
        RTDKLogger.Log($"Saving window data.");
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

    public void OnPointerUp(PointerEventData eventData)
    {
        var parent = transform.parent.gameObject;
        SaveSystem.gameData.SaveWindowState(parent.name, rect.anchoredPosition, parent.activeSelf);
    }

    public void CloseWindow()
    {
        var parent = transform.parent.gameObject;
        parent.SetActive(false);
        SaveSystem.gameData.SaveWindowState(parent.name, rect.anchoredPosition, parent.activeSelf);
    }
}
