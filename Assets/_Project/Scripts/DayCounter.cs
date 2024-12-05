using RTDK.GameSystems.SaveSystem;
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
        var time = DateTime.Now.Subtract(DateTime.FromBinary(date));
        counterText.text = $"<color=#EE1E7C>{SaveSystem.gameData.character.characterName}</color> has passed\n<color=#EE1E7C>{time.Days} days {time.Hours} hours {time.Minutes} minutes</color>\nwithout crying";
    }
}
