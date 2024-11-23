using UnityEngine;
using UnityEngine.UI;

public class ButtonExtension : Button
{
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
            graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }
    }
}
