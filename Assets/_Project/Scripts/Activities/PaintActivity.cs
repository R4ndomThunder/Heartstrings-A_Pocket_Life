using RTDK.InspectorPlus;
using UnityEngine;

public class PaintActivity : ActivityBase
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

    public override void JoinActivity()
    {
        base.JoinActivity();
        Application.OpenURL("mspaint");
    }
}
