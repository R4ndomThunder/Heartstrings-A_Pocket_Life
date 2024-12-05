using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using AudioSettings = RTDK.GameSystems.GameSettings.AudioSettings;
using Slider = UnityEngine.UI.Slider;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    Slider musicSlider, audioSlider;

    [SerializeField]
    TMP_Dropdown displayDropdown;

    [SerializeField]
    TMP_InputField nameField;

    [SerializeField]
    RectTransform gameWindow;

    WindowSize windowSize;

    private void Start()
    {
        InitAudio();

        InitName();

        InitDisplayDropdown();
    }

    private void OnEnable()
    {
        InitAudio();

        InitName();

        InitDisplayDropdown();
    }

    void InitName()
    {
        nameField.text = SaveSystem.gameData.character.characterName;
    }

    void InitAudio()
    {
        musicSlider.value = SaveSystem.gameData.settings.musicVol;
        audioSlider.value = SaveSystem.gameData.settings.soundVol;
    }

    void InitDisplayDropdown()
    {
        displayDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var item in Enum.GetNames(typeof(WindowSize)))
        {
            options.Add(new TMP_Dropdown.OptionData
            {
                text = $"{item}"
            });
        }

        displayDropdown.AddOptions(options);

        displayDropdown.value = SaveSystem.gameData.settings.windowSize;
    }

    public void SetPetName(string name)
    {
        SaveSystem.gameData.character.characterName = name;
    }

    public void SetActiveDisplay(int id)
    {
        switch ((WindowSize)id)
        {
            case WindowSize.Small:
                gameWindow.sizeDelta = new Vector2(300, 340);
                break;

            case WindowSize.Medium:
                gameWindow.sizeDelta = new Vector2(400, 440);
                break;

            case WindowSize.Large:
                gameWindow.sizeDelta = new Vector2(500, 540);
                break;
        }

        SaveSystem.gameData.settings.windowSize = id;
    }

    public void OnMusicVolumeChange(float val)
    {
        AudioSettings.SetVolume(mixer, "BGM", val);
        SaveSystem.gameData.settings.musicVol = val;
    }

    public void OnSFXVolumeChange(float val)
    {
        AudioSettings.SetVolume(mixer, "SFX", val);
        SaveSystem.gameData.settings.soundVol = val;
    }
}

public enum WindowSize
{
    Small, Medium, Large
}