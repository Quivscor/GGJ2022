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

		animator?.SetFloat("verticalMovement", vertical);
		animator?.SetFloat("horizontalMovement", horizontal);
	}
}
