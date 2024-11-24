using RTDK.InspectorPlus;
using RTDK.Logger;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour, IClickable
{
    [SerializeField]
    FaceMoodHandler faceMoodHandler;

    [SerializeField]
    GameObject body;

    NavMeshAgent agent;

    Animator anim;

    public PetMood GetCurrentMood() => currentMood;
    private PetMood currentMood = PetMood.Idle;

    [SerializeField]
    float statsAmountDecrement = 0.005f, statsAmountIncrement = 0.01f;

    [SerializeField]
    internal Stat happyness, snuggle, hunger, creativity, energy;

    [SerializeField]
    Transform bedZone, pcZone, sofaZone, musicZone, eatZone;

    [SerializeField]
    List<GameObject> bedItems, pcItems, sofaItems, musicItems, eatItems;

    [SerializeField]
    MinMaxFloat decisionTimes, movementDistance, activityTimes;
    float nextDecisionTimer;

    [SerializeField]
    private ParticleSystem loveParticle;

    Vector3 targetPosition;

    PetState currentState = PetState.Idle;

    Vector2 lastMousePos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        nextDecisionTimer = decisionTimes.GetRandom();
        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {
        ConsumeHappyness();
        ConsumeValues();

        switch (currentState)
        {
            case PetState.Idle:
                Idle();
                break;
            case PetState.Walking:
                CheckDestination();
                break;
            case PetState.Eating:
                Eat();
                break;
            case PetState.Gaming:
                PlayGames();
                break;
            case PetState.Drawing:
                Draw();
                break;
            case PetState.Music:
                Music();
                break;
            case PetState.Sleeping:
                Sleep();
                break;
        }
    }

    public void DoEating()
    {
        if (currentState == PetState.Idle || currentState == PetState.Walking)
        {
            anim.SetFloat("Speed", 1);
            targetPosition = eatZone.position;
            agent.SetDestination(targetPosition);
            ChangeState(PetState.Eating);
        }
    }

    public void GoToEntertain()
    {
        if (currentState == PetState.Idle || currentState == PetState.Walking)
        {
            anim.SetFloat("Speed", 1);
            var random = UnityEngine.Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    GoToPc();
                    break;
                case 1:
                    GoToSofa();
                    break;
                case 2:
                    GoToMusic();
                    break;
            }
        }
    }

    void GoToPc()
    {
        targetPosition = pcZone.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.Drawing);
    }

    void GoToSofa()
    {
        targetPosition = sofaZone.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.Gaming);
    }

    void GoToMusic()
    {
        targetPosition = musicZone.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.Music);
    }

    public void GoToSleep()
    {
        if (currentState == PetState.Idle || currentState == PetState.Walking)
        {
            anim.SetFloat("Speed", 1);
            targetPosition = bedZone.position;
            agent.SetDestination(targetPosition);
            ChangeState(PetState.Sleeping);
        }
    }

    void Sleep()
    {
        if (targetPosition == bedZone.position)
        {
            if (agent.pathPending) return;

            if (agent.remainingDistance <= .25f)
            {
                ChangeVisibility(bedItems, false);

                energy.Add(statsAmountIncrement);

                if (energy.GetValue() == energy.MinMaxValue.Max)
                {
                    ChangeVisibility(bedItems, true);
                    GetUp();
                }
            }
        }
    }

    void ChangeVisibility(List<GameObject> items, bool isPlayerVisible)
    {
        if (!isPlayerVisible)
            anim.SetFloat("Speed", 0);
        //agent.enabled = isPlayerVisible;
        foreach (var item in items)
        {
            item.SetActive(!isPlayerVisible);
        }

        body.SetActive(isPlayerVisible);
    }

    void GetUp()
    {
        targetPosition = Vector3.zero;
        ChangeState(PetState.Idle);
    }

    void Music()
    {
        if (targetPosition == musicZone.position)
        {
            if (agent.pathPending) return;

            if (agent.remainingDistance <= .05f)
            {
                ChangeVisibility(musicItems, false);

                creativity.Add(statsAmountIncrement);

                if (creativity.GetValue() == creativity.MinMaxValue.Max)
                {
                    ChangeVisibility(musicItems, true);
                    GetUp();
                }
            }
        }
    }

    void Draw()
    {
        if (targetPosition == pcZone.position)
        {
            if (agent.pathPending) return;

            if (agent.remainingDistance <= .05f)
            {
                ChangeVisibility(pcItems, false);

                creativity.Add(statsAmountIncrement);

                if (creativity.GetValue() == creativity.MinMaxValue.Max)
                {
                    ChangeVisibility(pcItems, true);
                    GetUp();
                }
            }
        }
    }

    void PlayGames()
    {
        if (targetPosition == sofaZone.position)
        {
            if (agent.pathPending) return;

            if (agent.remainingDistance <= .05f)
            {
                ChangeVisibility(sofaItems, false);

                creativity.Add(statsAmountIncrement);

                if (creativity.GetValue() == creativity.MinMaxValue.Max)
                {
                    ChangeVisibility(sofaItems, true);
                    GetUp();
                }
            }
        }
    }

    void Eat()
    {
        if (targetPosition == eatZone.position)
        {
            if (agent.pathPending) return;

            if (agent.remainingDistance <= .05f)
            {
                ChangeVisibility(eatItems, false);

                hunger.Add(statsAmountIncrement);

                if (hunger.GetValue() == hunger.MinMaxValue.Max)
                {
                    ChangeVisibility(eatItems, true);
                    GetUp();
                }
            }
        }
    }

    void Pat()
    {
        if (currentState != PetState.Idle && currentState != PetState.Walking) return;

        if (!loveParticle.isPlaying)
        {
            RTDKLogger.Log("Pattata");
            happyness.Add(.1f);
            snuggle.Add(10f);
            loveParticle.Play();
        }

        ChangeMood(PetMood.Blushy);
    }

    void ConsumeHappyness()
    {
        if (snuggle.GetValue() < 20 || hunger.GetValue() < 20 || creativity.GetValue() < 20 || energy.GetValue() < 20)
        {
            happyness.Remove(statsAmountDecrement);
            snuggle.Remove(statsAmountDecrement);
        }

        if (snuggle.GetValue() > 80 && hunger.GetValue() > 80 && creativity.GetValue() > 80 && energy.GetValue() > 80)
        {
            happyness.Add(statsAmountIncrement);
        }

        if (happyness.GetValue() < 20)
            ChangeMood(PetMood.Sad);
        else
            ChangeMood(PetMood.Idle);
    }

    void ConsumeValues()
    {
        if (currentState != PetState.Sleeping)
            energy.Remove(statsAmountDecrement);

        if (currentState != PetState.Eating)
            hunger.Remove(statsAmountDecrement);

        if (currentState != PetState.Drawing && currentState != PetState.Gaming)
            creativity.Remove(statsAmountDecrement);
    }

    void Idle()
    {
        if (nextDecisionTimer > 0)
            nextDecisionTimer -= Time.deltaTime;
        else
        {
            SetNewDestination();
            currentState = PetState.Walking;
        }
    }

    void SetNewDestination()
    {
        anim.SetFloat("Speed", 1);
        var random = UnityEngine.Random.insideUnitCircle * movementDistance.GetRandom();
        var target = new Vector3(random.x, 0, random.y);
        targetPosition = target;
        agent.SetDestination(targetPosition);
    }

    void CheckDestination()
    {
        if (agent.remainingDistance <= 0.25f)
        {
            nextDecisionTimer = decisionTimes.GetRandom();
            anim.SetFloat("Speed", 0);
            currentState = PetState.Idle;
        }
    }

    public void ChangeState(PetState state)
    {
        currentState = state;
    }

    public void ChangeMood(PetMood mood)
    {
        currentMood = mood;
        faceMoodHandler.OnMoodChange(mood);
    }

    public void OnClickDown()
    {

    }

    public void OnClickUp()
    {
        ChangeMood(PetMood.Idle);
    }

    public void OnClick()
    {
        if (Vector3.Distance(lastMousePos, Input.mousePosition) > 0.5f)
        {
            Pat();
        }

        lastMousePos = Input.mousePosition;
    }
}

[Serializable]
public class Stat
{
    public float value;
    public MinMaxFloat MinMaxValue;
    public float GetValue() => value;
    public void Add(float val)
    {
        value += val;
        if (value > MinMaxValue.Max)
            value = MinMaxValue.Max;
    }

    public void Remove(float val)
    {
        value -= val;
        if (value < MinMaxValue.Min)
            value = MinMaxValue.Min;
    }
}

public enum PetState
{
    Idle, Walking, Drawing, Gaming, Eating, Sleeping, Music
}