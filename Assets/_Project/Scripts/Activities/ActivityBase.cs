using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class ActivityBase : MonoBehaviour
{
    [SerializeField]
    internal AIBehaviour ai;

    [SerializeField]
    internal List<GameObject> objectToToggle = new();

    [SerializeField]
    internal Transform targetPos;

    internal bool isDoingSomething = false;
    [SerializeField]
    internal bool isACreativeActivity = false;

    [SerializeField]
    ParticleSystem activityParticle;

    private void Awake()
    {
        ai = FindFirstObjectByType<AIBehaviour>();
    }

    internal virtual void LeaveActivity()
    {
        ToggleObjects(true);
        ai.ChangeState(PetState.Idle);

        if (activityParticle != null)
            activityParticle.Stop();

        if (isACreativeActivity)
            ai.activityCounter++;
    }

    public virtual void JoinActivity()
    {
        ToggleObjects(false);
        if (activityParticle != null)
            activityParticle.Play();
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
