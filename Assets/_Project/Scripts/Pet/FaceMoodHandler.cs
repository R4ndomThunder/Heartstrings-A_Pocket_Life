using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FaceMoodHandler : MonoBehaviour
{
    [SerializeField]
    public List<FacePerMood> moodFaces;

    [SerializeField]
    TextMeshProUGUI face;

    private void Awake()
    {
        AIBehaviour.OnMoodChanged += OnMoodChange;
    }

    private void OnDestroy()
    {
        AIBehaviour.OnMoodChanged -= OnMoodChange;
    }

    public void OnMoodChange(PetMood oldMood, PetMood _mood)
    {
        var currentMood = moodFaces.FirstOrDefault(x => x.mood == _mood);
        face.text = currentMood.face;
    }
}

public enum PetMood
{
    Cozy, Creative, Normal, Sad, Blushy, Cry
}

[Serializable]
public class FacePerMood
{
    public PetMood mood;
    public string face;
}
