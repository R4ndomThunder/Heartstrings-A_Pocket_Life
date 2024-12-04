using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MoodText : MonoBehaviour
{
    [SerializeField]
    GameObject moodPanel;
    [SerializeField]
    TextMeshProUGUI moodText;
    [SerializeField]
    float timeText = 5;

    WaitForSeconds wfs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        wfs = new WaitForSeconds(timeText);

        AIBehaviour.OnMoodChanged += OnMoodChange;
    }

    private void OnDestroy()
    {
        AIBehaviour.OnMoodChanged -= OnMoodChange;
    }

    private void Start()
    {
        HideText();
    }

    private void OnMoodChange(PetMood oldMood, PetMood mood)
    {
        if (oldMood == PetMood.Blushy) return;

        if (oldMood == PetMood.Cry)
        {
            StopAllCoroutines();
            moodPanel.SetActive(true);
            moodText.text = $"{SaveSystem.gameData.character.characterName} stopped crying";
            StartCoroutine(HideTextCoroutine());
            return;
        }

        if (mood != PetMood.Cry && mood != PetMood.Blushy)
        {
            StopAllCoroutines();
            moodPanel.SetActive(true);
            moodText.text = $"{SaveSystem.gameData.character.characterName} is feeling {mood.ToString().ToLower()}";
            StartCoroutine(HideTextCoroutine());
        }
    }


    IEnumerator HideTextCoroutine()
    {
        yield return wfs;
        HideText();
    }

    void HideText()
    {
        moodText.text = string.Empty;
        moodPanel.SetActive(false);
    }
}
