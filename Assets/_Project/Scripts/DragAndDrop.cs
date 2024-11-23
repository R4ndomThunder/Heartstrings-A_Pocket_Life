using RTDK.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;

public class DragAndDrop : MonoBehaviour
{
    Vector3 mousePosition;
    Rigidbody rb;

    public Action onStartDrag, onEndDrag;

    Vector3 GetMousePos() => Utils.Camera.WorldToScreenPoint(transform.position);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        onStartDrag.Invoke();
        rb.isKinematic = true;
    }

    private void OnMouseDrag()
    {
        var target = Utils.Camera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        rb.MovePosition(target);
    }

    private void OnMouseUp()
    {
        onEndDrag.Invoke();
        rb.isKinematic = false;
    }
}
