using RTDK.InspectorPlus;
using RTDK.Logger;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    FaceMoodHandler faceMoodHandler;

    NavMeshAgent agent;

    [SerializeField]
    float moveSpeed = 3;

    public PetMood GetCurrentMood() => currentMood;
    private PetMood currentMood = PetMood.Idle;

    public float GetCurrentHappyness() => happyness;
    [SerializeField, ProgressBar]
    float happyness = 80f;

    public float GetCurrentCreativity() => creativity;
    [SerializeField, ProgressBar]
    float creativity = 0f;

    public float GetCurrentSnuggle() => snuggle;
    [SerializeField, ProgressBar]
    float snuggle = 0;

    [SerializeField]
    MinMaxFloat decisionTimes, movementDistance;
    float nextDecisionTimer;

    Vector3 targetPosition;

    PetState currentState = PetState.Idle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        nextDecisionTimer = decisionTimes.GetRandom();
    }

    private void OnMouseDown()
    {
        Pat();
    }

    private void OnMouseUp()
    {
        ChangeMood(PetMood.Idle);
    }

    private void Update()
    {
        switch (currentMood)
        {
            case PetMood.Idle:
                Idle();
                break;
            case PetMood.Cozy:
                Cozy();
                break;
            case PetMood.Sad:
                Sad();
                break;
            case PetMood.Creative:
                Creative();
                break;
        }
    }

    void Pat()
    {
        RTDKLogger.Log("Pattata");
        happyness += 0.05f;
        snuggle += .5f;
        ChangeMood(PetMood.Blushy);
    }

    void Idle()
    {
        if (currentState == PetState.Idle)
        {
            if (nextDecisionTimer > 0)
                nextDecisionTimer -= Time.deltaTime;
            else
            {
                var target = transform.position;
                target.x += movementDistance.GetRandom();
                target.z += movementDistance.GetRandom();
                targetPosition = target;
                agent.SetDestination(targetPosition);

                currentState = PetState.Walking;
            }
        }
        else if (currentState == PetState.Walking)
            CheckDestination();
    }

    void CheckDestination()
    {
        if (agent.remainingDistance <= 0.25f)
        {
            nextDecisionTimer = decisionTimes.GetRandom();
            currentState = PetState.Idle;
        }
    }

    void Cozy()
    {

    }

    void Sad()
    {

    }

    void Creative()
    {

    }


    void StartDragging()
    {
        ChangeMood(PetMood.Blushy);
    }

    void EndDragging()
    {
        ChangeMood(PetMood.Idle);
    }

    void Dragging()
    {
        ChangeMood(PetMood.Grabbed);
    }

    public void ChangeMood(PetMood mood)
    {
        faceMoodHandler.OnMoodChange(mood);
    }
}

public enum PetState
{
    Idle, Walking
}