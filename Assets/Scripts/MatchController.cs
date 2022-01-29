using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class MatchStateChanged : UnityEvent { }

public class MatchController : MonoBehaviour
{
    public static MatchController Instance;
    private CharacterHolder combatantsHolder;

    public MatchStateChanged onMatchPrestarted;
    public MatchStateChanged onMatchStarted;
    public MatchStateChanged onMatchFinished;

    public MatchState State { get; private set; }
    [SerializeField] MatchState initialStartState = MatchState.DEFAULT;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        State = initialStartState;
    }

    public void ChangeMatchState(MatchState state)
    {
        State = state;
        Debug.Log("MatchState: " + State.ToString());

        switch(State)
        {
            case MatchState.PRESTART:
                onMatchPrestarted?.Invoke();
                break;
            case MatchState.ACTIVE:
                onMatchStarted?.Invoke();
                break;
            case MatchState.FINISHED:
                onMatchFinished?.Invoke();
                break;
            default:
                Debug.Log("unknown");
                break;
        }
    }

    private void Update()
    {
        
    }
}

public enum MatchState
{
    PRESTART,
    ACTIVE,
    FINISHED,
    DEFAULT
}
