using RTDK.InspectorPlus;
using RTDK.Logger;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    FaceMoodHandler faceMoodHandler;

    [SerializeField]
    DragAndDrop dragNDrop;

    public PetMood GetCurrentMood() => currentMood;
    private PetMood currentMood;

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
    MinMaxFloat decisionTimes;
    float nextDecisionTimer;

    private void Awake()
    {
        dragNDrop.onStartDrag += StartDragging;
        dragNDrop.onEndDrag += EndDragging;
    }

    private void OnDestroy()
    {
        dragNDrop.onStartDrag -= StartDragging;
        dragNDrop.onEndDrag -= EndDragging;
    }

    private void OnMouseDown()
    {
        Pat();
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
    }

    void Idle()
    {
    }

    void Cozy() { }

    void Sad() { }

    void Creative() { }


    void StartDragging()
    {
        ChangeMood(PetMood.Grabbed);
    }

    void EndDragging()
    {
        ChangeMood(PetMood.Idle);
    }

    public void ChangeMood(PetMood mood)
    {
        faceMoodHandler.OnMoodChange(mood);
    }
}
