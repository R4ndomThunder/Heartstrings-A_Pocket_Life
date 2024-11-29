using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using AudioSettings = RTDK.GameSystems.GameSettings.AudioSettings;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    Slider musicSlider, audioSlider;

    [SerializeField]
    TMP_Dropdown displayDropdown;

    public static Action onDisplayChange = delegate { };

    private void Start()
    {
        musicSlider.value = AudioSettings.GetVolume(mixer, "BGM");
        audioSlider.value = AudioSettings.GetVolume(mixer, "SFX");

        InitDisplayDropdown();
    }

    void InitDisplayDropdown()
    {
        displayDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < Display.displays.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData
            {
                text = $"Screen {i}"
            });
        }

        displayDropdown.AddOptions(options);
    }

    public void SetActiveDisplay(int id)
    {
        var x = Display.displays[id].systemWidth;
        var y = Display.displays[id].systemHeight;

        Screen.MoveMainWindowTo(new DisplayInfo
        {
            width = x,
            height = y,
            refreshRate = new RefreshRate { numerator = 60, denominator = 1 },
        }, new Vector2Int(0, 0));

        Display.displays[id].Activate();

        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

        onDisplayChange?.Invoke();
    }

    public void OnMusicVolumeChange(float val)
    {
        AudioSettings.SetVolume(mixer, "BGM", val);
    }

    public void OnSFXVolumeChange(float val)
    {
        AudioSettings.SetVolume(mixer, "SFX", val);
    }
}
