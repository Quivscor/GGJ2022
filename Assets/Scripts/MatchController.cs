using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

[System.Serializable] public class MatchStateChanged : UnityEvent { }
[System.Serializable] public class RoundChanged : UnityEvent<float> { }

public class MatchController : MonoBehaviour
{
    public static MatchController Instance;
    private CharacterHolder combatantsHolder;

    public MatchStateChanged onMatchPrestarted;
    public MatchStateChanged onMatchStarted;
    public MatchStateChanged onMatchFinished;
    public RoundChanged onRoundChange;

    private int round = 1;

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
                round++;
                HudController.Instance.ChangeRound("Round " + round);
                onRoundChange?.Invoke(round);
                onMatchPrestarted?.Invoke();
                break;
            case MatchState.ACTIVE:
                onMatchStarted?.Invoke();
                break;
            case MatchState.FINISHED:
                onMatchFinished?.Invoke();
                if (round == 1)
                {
                    HudController.Instance.ChangeRound("");
                    StartCoroutine(DisplayVictory());
                }
                    
                break;
            default:
                Debug.Log("unknown");
                break;
        }
    }

    private IEnumerator DisplayVictory()
    {
        Cursor.visible = true;
        yield return StartCoroutine(FadeOut(1f, curve));
        yield return StartCoroutine(FadeInText(1f, mainText, curve));
        yield return StartCoroutine(FadeInText(1f, questionText, curve));
        yield return StartCoroutine(FadeInButtons(1f, canvasGroup, curve, true));
    }

    public void Continue()
    {
        HudController.Instance.ChangeRound("Round " + round);
        StartCoroutine(HideVictory());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator HideVictory()
    {
        Cursor.visible = false;
        yield return StartCoroutine(FadeInButtons(1f, canvasGroup, reverseCurve, false));
        yield return StartCoroutine(FadeInText(1f, questionText, reverseCurve));
        yield return StartCoroutine(FadeInText(1f, mainText, reverseCurve));
        yield return StartCoroutine(FadeOut(1f, reverseCurve));
    }

    public AnimationCurve curve;
    public AnimationCurve reverseCurve;
    public Image img;
    public TMProText mainText;
    public TMProText questionText;
    public CanvasGroup canvasGroup;

    private IEnumerator FadeOut(float t, AnimationCurve curve)
    {
        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    private IEnumerator FadeInText(float t, TMProText text, AnimationCurve curve)
    {
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            text.color = new Color(1f, 1f, 1f, a);
            yield return 0;
        }
    }

    private IEnumerator FadeInButtons(float t, CanvasGroup canvasGroup, AnimationCurve curve, bool unlock)
    {
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            canvasGroup.alpha = a;
            yield return 0;
        }
        canvasGroup.interactable = unlock;
        canvasGroup.blocksRaycasts = unlock;
    }
}

public enum MatchState
{
    PRESTART,
    ACTIVE,
    FINISHED,
    DEFAULT
}
