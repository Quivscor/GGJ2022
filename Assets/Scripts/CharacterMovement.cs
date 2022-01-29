using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 2.0f;
	
	[Header("References:")]
	[SerializeField] private Animator animator = null;
	[SerializeField] private Transform character = null;

	#region Slow Effect
	[Header("Slow Effect Options:")]
	[SerializeField] private float slowEffectTime = 2.0f;
	[SerializeField] private float slowedMovementSpeedFactor = 0.5f;
	
	private bool isSlowed = false;
	private Coroutine slowedEffectCoroutine = null;
	#endregion

	private new Rigidbody rigidbody = null;


	private void Awake()
	{
		if (animator == null)
			animator = GetComponentInChildren<Animator>();

		rigidbody = GetComponent<Rigidbody>();
	}

	public void ProcessMovement(float vertical, float horizontal)
	{
		Vector3 velocityToApply = new Vector3(horizontal, 0, vertical).normalized * movementSpeed;

		if (isSlowed)
			velocityToApply *= slowedMovementSpeedFactor;

		rigidbody.velocity = velocityToApply;

		ProcessAnimator(vertical, horizontal);
	}

	private void ProcessAnimator(float vertical, float horizontal)
	{
		//	bool swap = false;

		//	if (Mathf.Abs(this.transform.forward.x) > 0.5f)
		//	{
		//		float tmp = horizontal;
		//		horizontal = vertical;
		//		vertical = tmp;
		//		swap = true;
		//	}

		//	if (this.transform.forward.z < 0)
		//		if (!swap)
		//			vertical *= -1;


		//	animator.SetFloat("verticalMovement", vertical);
		//	animator.SetFloat("horizontalMovement", horizontal);
		//}
		animator?.SetBool("isMoving", new Vector3(horizontal, 0.0f, vertical) != Vector3.zero);

		int quarterIndex = (int)(character.localRotation.eulerAngles.y / 90);

		if (quarterIndex * 90 + 45 < character.localRotation.eulerAngles.y)
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

	public void AddSlowEffect()
	{
		if (slowedEffectCoroutine != null)
			StopCoroutine(slowedEffectCoroutine);

		slowedEffectCoroutine = StartCoroutine(SlowEffectTimer());
	}

	private IEnumerator SlowEffectTimer()
	{
		isSlowed = true;
		yield return new WaitForSeconds(slowEffectTime);
		isSlowed = false;
	}
}
