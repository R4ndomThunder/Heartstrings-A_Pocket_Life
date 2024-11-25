using System.Collections.Generic;
using UnityEngine;

public class SofaActivity : ActivityBase
{
    [SerializeField]
    float creativityIncrement = .5f;

    [SerializeField]
    List<GameObject> cozyObjects, idleObject = new();

    public override void OnUpdate()
    {
        base.OnUpdate();

        ai.creativity.Add(creativityIncrement);

        if (ai.creativity.IsMax())
            LeaveActivity();
    }

    internal override void ToggleObjects(bool playerIsVisible)
    {
        isDoingSomething = !playerIsVisible;
        ai.body.SetActive(playerIsVisible);

        if (ai.GetCurrentMood() == PetMood.Cozy)
        {
            foreach (GameObject obj in cozyObjects)
            {
                obj.SetActive(!playerIsVisible);
            }
        }
        else if (ai.GetCurrentMood() == PetMood.Idle)
        {
            foreach (GameObject obj in idleObject)
            {
                obj.SetActive(!playerIsVisible);
            }
        }

        if (playerIsVisible)
        {
            foreach (GameObject obj in idleObject)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in cozyObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}
