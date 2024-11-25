using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField]
    AIBehaviour ai;

    [SerializeField]
    GameObject alertIcon;

    [SerializeField]
    AudioSource alertSound;

    // Update is called once per frame
    void Update()
    {
        var isAlert = ai.happyness.GetValue() < ai.statsThreshold || ai.love.GetValue() < ai.statsThreshold || ai.hunger.GetValue() < ai.statsThreshold || ai.creativity.GetValue() < ai.statsThreshold || ai.energy.GetValue() < ai.statsThreshold;

        alertIcon.SetActive(isAlert);

        if (!alertSound.isPlaying && (ai.happyness.GetValue() < ai.statsThreshold && ai.GetCurrentState() != PetState.GameOver))
            alertSound.Play();
    }
}
