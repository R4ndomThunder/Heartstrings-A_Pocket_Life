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
    }

    public void OnMoodChange(PetMood mood)
    {
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
