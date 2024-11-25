using UnityEngine;

public class PaintActivity : ActivityBase
{
    [SerializeField]
    float creativityIncrement = .5f;
    [SerializeField]
    float happynessIncrement = .5f;

    [SerializeField]
    GameObject paintWindow;

    public override void OnUpdate()
    {
        base.OnUpdate();

        ai.creativity.Add(creativityIncrement);
        ai.happyness.Add(happynessIncrement);

        if (ai.creativity.IsMax())
            LeaveActivity();
    }

    public override void JoinActivity()
    {
        base.JoinActivity();
        //paintWindow.SetActive(true);
    }
}
