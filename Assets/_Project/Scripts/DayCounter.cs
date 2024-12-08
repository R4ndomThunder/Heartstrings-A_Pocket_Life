using RTDK.GameSystems.SaveSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        if (SaveSystem.gameData.character.currentState == 9) return;

        SaveSystem.gameData.character.timeFromLastCry++;

        TimeSpan time = TimeSpan.FromSeconds(SaveSystem.gameData.character.timeFromLastCry);

        counterText.text = $"<color=#EE1E7C>{SaveSystem.gameData.character.characterName}</color> has passed\n<color=#EE1E7C>{time.Days} days {time.Hours} hours {time.Minutes} minutes</color>\nwithout crying";
    }
}
