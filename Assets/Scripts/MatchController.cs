using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class MatchStarted : UnityEvent { }
[System.Serializable] public class MatchFinished : UnityEvent { }

public class MatchController : MonoBehaviour
{
    public static MatchController Instance;
    private CharacterHolder combatantsHolder;
    public MatchState State { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        State = MatchState.DEFAULT;
    }
}

public enum MatchState
{
    PRESTART,
    ACTIVE,
    FINISHED,
    DEFAULT
}
