using System;
using TMPro;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateCounter), 0, 1);
    }

    void UpdateCounter()
    {
        var date = SaveSystem.gameData.character.lastCryDate;
        var time = DateTime.Now.Subtract(date);
        counterText.text = $"<size=50%>{time.Days} days {time.Hours} hours {time.Minutes} minutes</size>\nwithout crying";
    }
}
