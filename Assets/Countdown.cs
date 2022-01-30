using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;

public class Countdown : MonoBehaviour
{
    TMProText countdownText;
    Animator animator;

    private void Awake()
    {
        countdownText = GetComponent<TMProText>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        StartCoroutine(Counting());
    }

    public IEnumerator Counting()
    {
        animator.Play("Counting");
        countdownText.text = "3";
        yield return new WaitForSeconds(0.2f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.2f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.2f);
        MatchController.Instance.ChangeMatchState(MatchState.ACTIVE);
    }
}
