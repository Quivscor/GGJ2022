using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
	[SerializeField] private CharacterMovement characterMovement = null;
	[SerializeField] private CharacterRotator characterRotator = null;
	[SerializeField] private WandController wandController = null;

	private void Update()
	{
		ProcessRotation();
		ProcessMovement();
		ProcessMouseInputs();
	}

	private void ProcessRotation()
	{
		characterRotator.LookAt(MouseTracker.Instance.WorldPosition);
	}

	private void ProcessMouseInputs()
	{
		bool fire1pressed = Input.GetButton("Fire1");
		bool fire2pressed = Input.GetButton("Fire2");

		wandController.ProcessSpell(fire1pressed, fire2pressed);
	}

	private void ProcessMovement()
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		bool dash = Input.GetKeyDown(KeyCode.Space);

		if (dash)
			characterMovement.ProcessDash(vertical, horizontal);
		characterMovement.ProcessMovement(vertical, horizontal);
	}
}
