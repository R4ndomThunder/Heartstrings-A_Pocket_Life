using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField]
    AIBehaviour ai;

    [SerializeField]
    GameObject alertIcon;

    // Update is called once per frame
    void Update()
    {
        alertIcon.SetActive(ai.snuggle.GetValue() < 20 || ai.hunger.GetValue() < 20 || ai.creativity.GetValue() < 20 || ai.energy.GetValue() < 20);
    }
}
