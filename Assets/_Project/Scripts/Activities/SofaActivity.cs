using System.Collections.Generic;
using UnityEngine;

public class SofaActivity : ActivityBase
{
    [SerializeField]
    float creativityIncrement = .5f;

    [SerializeField]
    List<GameObject> cozyObjects, idleObject = new();

    [SerializeField]
    ParticleSystem filmParticle, gameParticle;

    [SerializeField]
    AudioClip filmAudio, gameAudio;
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

            if (filmParticle != null)
            {
                if (playerIsVisible)
                    filmParticle.Stop();
                else
                    filmParticle.Play();
            }

            if (filmAudio != null)
            {
                source.clip = filmAudio;
                if (playerIsVisible)
                    source.Stop();
                else
                    source.Play();
            }
        }
        else if (ai.GetCurrentMood() == PetMood.Normal)
        {
            foreach (GameObject obj in idleObject)
            {
                obj.SetActive(!playerIsVisible);
            }

            if (gameParticle != null)
            {
                if (playerIsVisible)
                    gameParticle.Stop();
                else
                    gameParticle.Play();
            }

            if (gameAudio != null)
            {
                source.clip = gameAudio;
                if (playerIsVisible) 
                    source.Stop();
                else
                    source.Play();
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
