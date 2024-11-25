using UnityEngine;
using UnityEngine.EventSystems;

public class DrawOnTexture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 localCurs = Vector2.zero;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, null, out localCurs))
            return;

        Vector2 normalizedPoint = Rect.PointToNormalized(rect.rect, localCurs);
        lastPos = normalizedPoint;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    Vector2 lastPos = Vector2.zero;
    void Paint(Vector2 pixelPos)
    {
        Color color = Color.black;

    }
}