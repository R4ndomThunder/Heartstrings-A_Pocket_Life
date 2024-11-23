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

    public void OnMoodChange(PetMood _mood)
    {
        var currentMood = moodFaces.FirstOrDefault(x => x.mood == _mood);
        face.text = currentMood.face;
    }
}

public enum PetMood
{
    Cozy, Creative, Idle, Sad, Grabbed,
    Blushy
}

[Serializable]
public class FacePerMood
{
    public PetMood mood;
    public string face;
}
