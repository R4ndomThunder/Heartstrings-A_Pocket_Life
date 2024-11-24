using MPUIKIT;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField]
    AIBehaviour ai;

    [SerializeField]
    MPImage happyIcon, hungerIcon, loveIcon, creativityIcon, energyIcon;

    [SerializeField]
    Gradient statGradient;

    public void Update()
    {
        var happyVal = ai.happyness.GetValue() / 100f;
        happyIcon.color = statGradient.Evaluate(happyVal);
        happyIcon.fillAmount = happyVal;

        var hungerVal = ai.hunger.GetValue() / 100f;
        hungerIcon.color = statGradient.Evaluate(hungerVal);
        hungerIcon.fillAmount = hungerVal;

        var loveVal = ai.snuggle.GetValue() / 100f;
        loveIcon.color = statGradient.Evaluate(loveVal);
        loveIcon.fillAmount = loveVal;

        var creativityVal = ai.creativity.GetValue() / 100f;
        creativityIcon.color = statGradient.Evaluate(creativityVal);
        creativityIcon.fillAmount = creativityVal;

        var energyVal = ai.energy.GetValue() / 100f;
        energyIcon.color = statGradient.Evaluate(energyVal);
        energyIcon.fillAmount = energyVal;
    }
}
