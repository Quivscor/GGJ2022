using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;

public class Countdown : MonoBehaviour
{
    TMProText countdownText;
    Animator animator;

    public int countFrom = 5;
    private int currentTime = 0;

    private void Awake()
    {
        countdownText = GetComponent<TMProText>();
        animator = GetComponent<Animator>();
    }

    public void StartCountdown()
    {
        currentTime = countFrom;
        StartCountdown();
        StartCoroutine(Counting());
    }

    public IEnumerator Counting()
    {
        currentTime = countFrom;
        //animator.Play("Counting");
        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString();
            currentTime--;
            yield return new WaitForSeconds(1f);
        }
        MatchController.Instance.ChangeMatchState(MatchState.ACTIVE);
        countdownText.text = "";
    }
}
