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

    private void Start()
    {
        musicSlider.value = AudioSettings.GetVolume(mixer, "BGM");
        audioSlider.value = AudioSettings.GetVolume(mixer, "SFX");
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
