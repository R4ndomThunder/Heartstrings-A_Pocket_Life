using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accordion : MonoBehaviour
{
    [SerializeField]
    private MinMaxFloat ySize;

    [SerializeField]
    private Image btn;

    [SerializeField]
    private Sprite caretUp, caretDown;

    private RectTransform rect;

    private bool isCompact = false;

    public List<GameObject> objectsToDisplay;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        UpdateUI();
    }

    public void ToggleCompact()
    {
        isCompact = !isCompact;

        foreach (var item in objectsToDisplay)
        {
            item.SetActive(!isCompact);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        btn.sprite = isCompact ? caretUp : caretDown;
        var y = isCompact ? ySize.Min : ySize.Max;
        rect.sizeDelta = new(200, y);
    }
}
