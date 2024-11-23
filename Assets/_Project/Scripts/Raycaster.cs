using UnityEngine;
using UnityEngine.EventSystems;

public class Raycaster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField]
    Camera rtCamera;

    IClickable lastClickable;

    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    Ray GetRenderTextureRay(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, null, out localPoint);
        Vector2 normalizedPoint = Rect.PointToNormalized(rect.rect, localPoint);

        return rtCamera.ViewportPointToRay(normalizedPoint);
    }

    bool isClicked = false;

    public void OnPointerUp(PointerEventData eventData)
    {
        var renderRay = GetRenderTextureRay(eventData);

        isClicked = false;

        if (Physics.Raycast(renderRay, out var hit, Mathf.Infinity))
        {
            hit.collider.GetComponent<IClickable>()?.OnClickUp();
        }

        if (lastClickable != null)
        {
            lastClickable.OnClickUp();
            lastClickable = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;

        var renderRay = GetRenderTextureRay(eventData);
        if (Physics.Raycast(renderRay, out var hit, Mathf.Infinity))
        {
            lastClickable = hit.collider.GetComponent<IClickable>();
            lastClickable?.OnClickDown();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isClicked)
            lastClickable?.OnClick();
    }
}