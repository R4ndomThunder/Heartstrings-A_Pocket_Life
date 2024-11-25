using UnityEngine;

public class BedActivity : ActivityBase
{
    [Header("Idle")]
    [SerializeField]
    private float idleEnergyIncrement = 0.05f;
    [Header("Sad")]
    [SerializeField]
    private float sadEnergyIncrement = 0.25f;
    [SerializeField]
    private float sadHappyDecrement = 0.01f;
    [Header("Cozy")]
    [SerializeField]
    private float cozyEnergyIncrement = 0.05f;
    [SerializeField]
    private float cozyLoveIncrement = 0.05f;
    [SerializeField]
    private float cozyHappyIncrement = 0.05f;
    [Header("Creative")]
    [SerializeField]
    private float creativeHappyDecrement = 0.01f;
    private float creativeEnergyIncrement = 0.01f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        switch (ai.GetCurrentMood())
        {
            case PetMood.Creative:
                HandleCreativity();
                break;
            case PetMood.Cozy:
                HandleCozy();
                break;
            case PetMood.Idle:
                HandleIdle();
                break;
            case PetMood.Sad:
                HandleSad();
                break;
        }

        if (ai.energy.IsMax())
            LeaveActivity();
    }

    void HandleCreativity()
    {
        ai.energy.Add(creativeEnergyIncrement);
        ai.happyness.Add(creativeHappyDecrement);
    }

    void HandleCozy()
    {
        ai.energy.Add(cozyEnergyIncrement);
        ai.happyness.Add(cozyHappyIncrement);
        ai.love.Add(cozyLoveIncrement);
    }

    void HandleIdle()
    {
        ai.energy.Add(idleEnergyIncrement);
    }

    void HandleSad()
    {
        ai.energy.Add(sadEnergyIncrement);
        ai.happyness.Remove(sadHappyDecrement);
    }
}
