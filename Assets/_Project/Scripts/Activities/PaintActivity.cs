using UnityEngine;

public class PaintActivity : ActivityBase
{
    [SerializeField]
    float creativityIncrement = .5f;

    [SerializeField]
    GameObject paintWindow;

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
        paintWindow.SetActive(true);
    }
}
