using RTDK.InspectorPlus;
using RTDK.Logger;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour, IClickable
{
    [SerializeField]
    FaceMoodHandler faceMoodHandler;

    NavMeshAgent agent;

    public PetMood GetCurrentMood() => currentMood;
    private PetMood currentMood = PetMood.Idle;

    public float GetCurrentHappyness() => happyness;
    [SerializeField, ProgressBar, ColorField(GUIColor.Lime)]
    float happyness = 80f;

    public float GetCurrentEnergy() => energy;
    [SerializeField, ProgressBar, ColorField(GUIColor.Blue)]
    float energy = 100f;

    [SerializeField, ProgressBar(), ColorField(GUIColor.Brown)]
    float hunger = 100f;

    public float GetCurrentCreativity() => creativity;
    [SerializeField, ProgressBar, ColorField(GUIColor.Pink)]
    float creativity = 0f;

    public float GetCurrentSnuggle() => snuggle;
    [SerializeField, ProgressBar, ColorField(GUIColor.Red)]
    float snuggle = 0;


    [SerializeField]
    MinMaxFloat decisionTimes, movementDistance, eatTimes;
    float nextDecisionTimer;
    float eatTimer;

    [SerializeField]
    private ParticleSystem loveParticle;

    Vector3 targetPosition;

    PetState currentState = PetState.Idle;

    Vector2 lastMousePos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        nextDecisionTimer = decisionTimes.GetRandom();
        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {
        ConsumeEnergy();

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
            case PetState.Sleeping:
                Sleep();
                break;
        }
    }

    void Sleep()
    {
        //TODO: Anims ecc

        if (energy < 100)
            energy += 5f;
    }

    void Draw()
    {

    }

    void PlayGames()
    {

    }

    void Eat()
    {
        if (eatTimer > 0)
        {
            eatTimer -= Time.deltaTime;
            ChangeState(PetState.Idle);
        }
    }

    void Pat()
    {
        if (currentState != PetState.Idle && currentState != PetState.Walking) return;

        if (!loveParticle.isPlaying)
        {
            RTDKLogger.Log("Pattata");
            happyness += 0.05f;
            snuggle += .5f;
            loveParticle.Play();
        }

        ChangeMood(PetMood.Blushy);
    }

    void ConsumeEnergy()
    {
        if (currentState != PetState.Sleeping)
            energy -= 0.001f;
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
        var random = Random.insideUnitCircle * movementDistance.GetRandom();
        var target = transform.position + new Vector3(random.x, 0, random.y);
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

public enum PetState
{
    Idle, Walking, Drawing, Gaming, Eating, Sleeping
}