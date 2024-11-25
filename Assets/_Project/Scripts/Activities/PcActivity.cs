using UnityEngine;

public class PcActivity : ActivityBase
{
    [SerializeField]
    float creativityIncrement = .5f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        ai.creativity.Add(creativityIncrement);

        if (ai.creativity.IsMax())
            LeaveActivity();
    }
}
