using System.Collections.Generic;
using UnityEngine;

public abstract class ActivityBase : MonoBehaviour
{
    [SerializeField]
    internal AIBehaviour ai;

    [SerializeField]
    internal List<GameObject> objectToToggle = new();

    [SerializeField]
    internal Transform targetPos;

    internal bool isDoingSomething = false;

    private void Awake()
    {
        ai = FindFirstObjectByType<AIBehaviour>();
    }

    internal virtual void LeaveActivity()
    {
        ToggleObjects(true);
        ai.ChangeState(PetState.Idle);
    }

    public virtual void JoinActivity()
    {
        ToggleObjects(false);
    }

    public virtual void OnUpdate()
    {
        if (!isDoingSomething) return;
    }

    internal virtual void ToggleObjects(bool playerIsVisible)
    {
        isDoingSomething = !playerIsVisible;
        ai.body.SetActive(playerIsVisible);
        foreach (GameObject obj in objectToToggle)
        {
            obj.SetActive(!playerIsVisible);
        }
    }
}
