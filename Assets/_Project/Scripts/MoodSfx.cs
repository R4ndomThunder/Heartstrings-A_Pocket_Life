using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoodSfx : MonoBehaviour
{
    [SerializeField]
    List<MoodAudioPair> clips;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        AIBehaviour.OnMoodChanged += OnMoodChange;
    }

    private void OnDestroy()
    {
        AIBehaviour.OnMoodChanged -= OnMoodChange;
    }

    public void OnMoodChange(PetMood oldMood, PetMood mood)
    {
        if (oldMood == PetMood.Blushy) return;

        var clip = clips.FirstOrDefault(x => x.mood == mood);

        if (clip != null)
        {
            source.PlayOneShot(clip.clip);
        }
    }
}

[Serializable]
public class MoodAudioPair
{
    public PetMood mood;
    public AudioClip clip;
}
