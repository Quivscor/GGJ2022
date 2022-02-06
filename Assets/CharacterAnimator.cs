using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
	private Transform root;

    private void Awake()
    {
        animator = GetComponent<Animator>();
		root = transform.root;

		if (root.TryGetComponent(out CharacterMovement movement))
			movement.MovementProcessed += ProcessMovement;

		if (root.TryGetComponent(out SpellcastingController spells))
			spells.SpellProcessed += ProcessCasting;
    }

    private void ProcessMovement(float vertical, float horizontal)
    {
		animator?.SetBool("isMoving", new Vector3(horizontal, 0.0f, vertical) != Vector3.zero);

		int quarterIndex = (int)(root.localRotation.eulerAngles.y / 90);

		if (quarterIndex * 90 + 45 < root.localRotation.eulerAngles.y)
			quarterIndex++;

		quarterIndex = quarterIndex % 4;

		if (quarterIndex == 0 || quarterIndex == 2)
		{
			animator?.SetFloat("verticalMovement", vertical * (quarterIndex == 2 ? -1.0f : 1.0f));
			animator?.SetFloat("horizontalMovement", horizontal * (quarterIndex == 2 ? -1.0f : 1.0f));
		}
		else
		{
			animator?.SetFloat("verticalMovement", horizontal * (quarterIndex == 3 ? -1.0f : 1.0f));
			animator?.SetFloat("horizontalMovement", vertical * (quarterIndex == 3 ? 1.0f : -1.0f));
		}
	}

    private void ProcessCasting(SpellcastEventData data)
    {
        bool isSpell = false;
        if (data.type != SpellType.Unknown)
        {
            animator.SetLayerWeight(1, 1);
            isSpell = true;
        }
        else
            animator.SetLayerWeight(1, 0);

        animator?.SetBool("isAttacking", isSpell);
    }
}
