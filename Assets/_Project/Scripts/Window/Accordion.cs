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

    private void Start()
    {
        SetActiveDisplay(SaveSystem.gameData.settings.windowSize);
    }

    public void SetActiveDisplay(int id)
    {
        switch ((WindowSize)id)
        {
            case WindowSize.Small:
                rect.sizeDelta = new Vector2(300, 340);
                break;

            case WindowSize.Medium:
                rect.sizeDelta = new Vector2(400, 440);
                break;

            case WindowSize.Large:
                rect.sizeDelta = new Vector2(500, 540);
                break;
        }
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
        rect.sizeDelta = new(rect.sizeDelta.x, y);
    }
}
