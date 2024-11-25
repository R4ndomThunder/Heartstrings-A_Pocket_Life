using UnityEngine;

public class FeedActivity : ActivityBase
{
    [SerializeField]
    float hungerIncrement = 0.05f;
    [SerializeField]
    float happynessIncrement = 0.05f;
    [SerializeField]
    float loveIncrement = 0.05f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        ai.hunger.Add(hungerIncrement);

        if (ai.GetCurrentMood() == PetMood.Idle)
        {
            ai.happyness.Add(happynessIncrement);
        }
        else if (ai.GetCurrentMood() == PetMood.Cozy)
        {
            ai.love.Add(loveIncrement);
        }

        if (ai.hunger.IsMax())
            LeaveActivity();
    }
}
