using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 2.0f;

	private Animator animator = null;
	private new Rigidbody rigidbody = null;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public void ProcessMovement(float vertical, float horizontal)
	{
		Vector3 velocityToApply = new Vector3(horizontal, 0, vertical).normalized * movementSpeed;
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
		animator.SetBool("isMoving", new Vector3(horizontal, 0.0f, vertical) != Vector3.zero);

		int quarterIndex = (int)(this.transform.rotation.eulerAngles.y / 90);

		if (quarterIndex * 90 + 45 < this.transform.rotation.eulerAngles.y)
			quarterIndex++;

		quarterIndex = quarterIndex % 4;

		if (quarterIndex == 0 || quarterIndex == 2)
		{
			animator.SetFloat("verticalMovement", vertical * (quarterIndex == 2 ? -1.0f : 1.0f));
			animator.SetFloat("horizontalMovement", horizontal * (quarterIndex == 2 ? -1.0f : 1.0f));
		}
		else
		{
			animator.SetFloat("verticalMovement", vertical * (quarterIndex == 3 ? -1.0f : 1.0f));
			animator.SetFloat("horizontalMovement", horizontal * (quarterIndex == 3 ? 1.0f : -1.0f));
		}
	}
}
