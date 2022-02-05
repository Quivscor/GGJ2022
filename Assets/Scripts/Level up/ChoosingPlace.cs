using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingPlace : MonoBehaviour
{
    [SerializeField] private Animator[] planesAnimator;

    private void OnEnable()
    {
        foreach (Animator animator in planesAnimator)
            animator.SetTrigger("ShowTrigger");
    }
}
