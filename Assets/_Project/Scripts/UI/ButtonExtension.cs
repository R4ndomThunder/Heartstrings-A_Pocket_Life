using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[AddComponentMenu("UI/BetterButton", 35)]
public class BetterButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Serializable]
    /// <summary>
    /// Function definition for a button click event.
    /// </summary>
    public class ButtonClickEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickEvent m_OnClick = new ButtonClickEvent();

    [SerializeField]
    private GameObject tooltip;

    public ButtonClickEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    protected BetterButton()
    {

    }


    protected override void OnEnable()
    {
        base.OnEnable();

        if (tooltip != null)
            tooltip.SetActive(false);

    }
    protected override void Awake()
    {
        base.Awake();
        if (tooltip == null)
        {
            tooltip = transform.Find("Tooltip")?.gameObject;
        }

        if (tooltip != null)
            tooltip.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Press();
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;

        UISystemProfilerApi.AddMarker("BetterButton.onClick", this);
        m_OnClick.Invoke();
    }


    public virtual void OnSubmit(BaseEventData eventData)
    {
        Press();

        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (tooltip != null && IsActive() && IsInteractable())
            tooltip.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (tooltip != null)
            tooltip.SetActive(false);
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        Color targetColor =
            state == SelectionState.Disabled ? colors.disabledColor :
            state == SelectionState.Highlighted ? colors.highlightedColor :
            state == SelectionState.Normal ? colors.normalColor :
            state == SelectionState.Pressed ? colors.pressedColor :
            state == SelectionState.Selected ? colors.selectedColor : Color.white;

        var items = GetComponentsInChildren<Graphic>();

        foreach (Graphic graphic in items)
        {
            if (graphic.gameObject != tooltip)
                graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }
    }
}
