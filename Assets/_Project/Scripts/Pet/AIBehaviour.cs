using RTDK.Logger;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour, IClickable
{
    [SerializeField]
    internal GameObject copertina;

    [SerializeField]
    internal GameObject body;
    [SerializeField]
    internal GameObject cryingBody;

    [SerializeField]
    private Collider baseCollider, cryingCollider;

    internal int activityCounter = 0;

    NavMeshAgent agent;
    Animator anim;

    public PetMood GetCurrentMood() => currentMood;
    private PetMood currentMood = PetMood.Normal;

    [SerializeField]
    float statsAmountDecrement = 0.005f;

    [SerializeField]
    internal Stat happyness, love, hunger, creativity, energy;

    [SerializeField]
    ActivityBase pc, music, bed, sofa, cookies, art;

    [SerializeField]
    internal float statsThreshold = 30;

    [SerializeField]
    internal AnimationCurve loveDecreasingCurve, happynessDecreasingCurve, creativityDecreasingCurve;

    [SerializeField]
    MinMaxFloat decisionTimes, movementDistance;
    float nextDecisionTimer;

    [SerializeField]
    float gameOverPatTime = 30;
    float pattingTimer = 30;

    [SerializeField]
    private ParticleSystem loveParticle;

    public static Action<PetMood, PetMood> OnMoodChanged;


    Vector3 targetPosition;

    public PetState GetCurrentState() => currentState;
    PetState currentState = PetState.Idle;

    Vector2 lastMousePos;

    bool isDoingActivity => currentState != PetState.Idle && currentState != PetState.Walking && currentState != PetState.Sleeping;

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
        ConsumeStats();

        CheckMood();

        HandleState();

        anim.SetFloat("Speed", agent.velocity.sqrMagnitude > 0 ? 1 : 0);
    }

    void ConsumeStats()
    {
        if (currentState != PetState.GameOver)
        {
            energy.Remove(statsAmountDecrement * (isDoingActivity ? 1.5f : 1));

            hunger.Remove(statsAmountDecrement);

            creativity.Remove(statsAmountDecrement * creativityDecreasingCurve.Evaluate(creativity.GetValue() / 100f));

            happyness.Remove(statsAmountDecrement * happynessDecreasingCurve.Evaluate(happyness.GetValue() / 100f));

            love.Remove(statsAmountDecrement * (loveDecreasingCurve.Evaluate(happyness.GetValue() / 100f)));

            if (happyness.GetValue() <= 0)
            {
                Cry();
            }
        }
    }

    void CheckMood()
    {
        switch (currentMood)
        {
            case PetMood.Normal:
                {
                    if (happyness.GetValue() < statsThreshold && love.GetValue() < statsThreshold)
                    {
                        ChangeMood(PetMood.Sad);
                    }
                    else if (energy.GetValue() < statsThreshold)
                    {
                        ChangeMood(PetMood.Cozy);
                    }
                    else if (activityCounter > 3 && energy.GetValue() > statsThreshold)
                    {
                        activityCounter = 0;
                        ChangeMood(PetMood.Creative);
                    }
                }
                break;
            case PetMood.Cozy:
                {
                    if (energy.GetValue() >= 98)
                    {
                        ChangeMood(PetMood.Normal);
                    }
                }
                break;
            case PetMood.Creative:
                {
                    if (energy.GetValue() < statsThreshold)
                        ChangeMood(PetMood.Cozy);
                    else
                    {
                        if (activityCounter > 3)
                        {
                            activityCounter = 0;
                            ChangeMood(PetMood.Normal);
                        }
                    }
                }
                break;
            case PetMood.Sad:
                {
                    if (happyness.GetValue() > statsThreshold)
                    {
                        ChangeMood(PetMood.Normal);
                    }
                }
                break;
        }
    }

    void HandleState()
    {
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
            case PetState.WatchTV:
                UseSofa();
                break;
            case PetState.Working:
                Work();
                break;
            case PetState.Drawing:
                Draw();
                break;
            case PetState.ListenMusic:
                Music();
                break;
            case PetState.Sleeping:
                Sleep();
                break;
            case PetState.GameOver:
                GameOver();
                break;
        }
    }

    public void OnHungerClick()
    {
        if (currentState == PetState.Idle || currentState == PetState.Walking)
        {
            targetPosition = cookies.targetPos.position;
            agent.SetDestination(targetPosition);
            ChangeState(PetState.Eating);
        }
    }

    public void OnActivityClick()
    {
        if (currentState == PetState.Idle || currentState == PetState.Walking)
        {
            switch (currentMood)
            {
                case PetMood.Normal:
                    {
                        var random = UnityEngine.Random.Range(0, 3);

                        switch (random)
                        {
                            case 0:
                                GoToPc();
                                break;
                            case 1:
                                GoToGamingSofa();
                                break;
                            case 2:
                                GoToMusic();
                                break;
                        }
                    }
                    break;
                case PetMood.Creative:
                    GoToArt();
                    break;
                case PetMood.Cozy:
                    GoToTVSofa();
                    break;
                case PetMood.Sad:
                    //TODO: Dormire triste
                    break;
            }
        }
    }

    public void OnEnergyClick()
    {
        if (currentState == PetState.Idle || currentState == PetState.Walking)
        {
            targetPosition = bed.targetPos.position;
            agent.SetDestination(targetPosition);
            ChangeState(PetState.Sleeping);
        }
    }

    void GoToPc()
    {
        targetPosition = pc.targetPos.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.Working);
    }

    void GoToTVSofa()
    {
        targetPosition = sofa.targetPos.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.WatchTV);
    }

    void GoToGamingSofa()
    {
        targetPosition = sofa.targetPos.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.Gaming);
    }

    void GoToMusic()
    {
        targetPosition = music.targetPos.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.ListenMusic);
    }

    void GoToArt()
    {
        targetPosition = art.targetPos.position;
        agent.SetDestination(targetPosition);
        ChangeState(PetState.Drawing);
    }

    void Sleep()
    {
        UseActivity(bed);
    }

    void Music()
    {
        UseActivity(music);
    }

    void Draw()
    {
        UseActivity(art);
    }

    void UseSofa()
    {
        UseActivity(sofa);
    }

    void Eat()
    {
        UseActivity(cookies);
    }

    void Work()
    {
        UseActivity(pc);
    }

    void GameOver() { }

    ActivityBase currentActivity;
    void UseActivity(ActivityBase act)
    {
        var dist = GetDistanceFromActivity(act);

        if (dist.HasValue && dist.Value <= .05f)
        {
            if (!act.isDoingSomething)
            {
                act.JoinActivity();
                currentActivity = act;
            }
            else
                act.OnUpdate();
        }
    }

    float? GetDistanceFromActivity(ActivityBase act)
    {
        if (targetPosition == act.targetPos.position)
        {
            if (agent.pathPending)
            {
                return null;
            }

            return agent.remainingDistance;
        }

        return null;
    }

    void Pat()
    {
        if (currentState != PetState.Idle && currentState != PetState.Walking && currentState != PetState.GameOver) return;

        if (currentState == PetState.GameOver)
        {
            if (!loveParticle.isPlaying)
            {
                loveParticle.Play();
            }

            pattingTimer -= Time.deltaTime;
            if (pattingTimer <= 0)
            {
                ResetPet();
            }
        }
        else
        {
            if (currentMood != PetMood.Blushy)
            {
                oldMood = currentMood;
                ChangeMood(PetMood.Blushy);
            }

            if (!loveParticle.isPlaying)
            {
                love.Add(10f);
                happyness.Add(2f);
                loveParticle.Play();
            }
        }
    }

    void Cry()
    {
        pattingTimer = gameOverPatTime;
        ChangeState(PetState.GameOver);
        targetPosition = transform.position;
        agent.SetDestination(targetPosition);
        body.SetActive(false);
        cryingBody.SetActive(true);

        baseCollider.enabled = false;
        cryingCollider.enabled = true;
    }

    void ResetPet()
    {
        happyness.value = happyness.MinMaxValue.Max;
        energy.value = energy.MinMaxValue.Max;
        creativity.value = creativity.MinMaxValue.Max;
        hunger.value = hunger.MinMaxValue.Max;
        love.value = love.MinMaxValue.Max;


        baseCollider.enabled = true;
        cryingCollider.enabled = false;

        cryingBody.SetActive(false);
        body.SetActive(true);
        ChangeState(PetState.Idle);
        ChangeMood(PetMood.Normal);
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
            currentState = PetState.Idle;
        }
    }

    public void ChangeState(PetState state)
    {
        currentState = state;
    }

    public void ChangeMood(PetMood mood)
    {
        OnMoodChanged.Invoke(currentMood, mood);
        currentMood = mood;
        copertina.SetActive(mood == PetMood.Cozy);
    }

    public void OnClickDown()
    {

    }

    PetMood oldMood = PetMood.Normal;
    public void OnClickUp()
    {
        if (currentState == PetState.GameOver && pattingTimer >= 0)
            pattingTimer = gameOverPatTime;

        if (currentMood == PetMood.Blushy)
            ChangeMood(oldMood);
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

    public bool IsMax() => value == MinMaxValue.Max;
    public float GetValue() => value;
    public void Add(float val)
    {
        value += val;
        if (value >= MinMaxValue.Max)
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
    Idle, Walking, Working, Drawing, Gaming, Eating, Sleeping, ListenMusic, WatchTV, GameOver
}