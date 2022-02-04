using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
	[SerializeField] private CharacterMovement characterMovement = null;
	[SerializeField] private CharacterRotator characterRotator = null;
	[SerializeField] private SpellcastingController SpellcastingController = null;

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

		//SpellcastingController.ProcessSpell(fire1pressed, fire2pressed);
	}

	private void ProcessMovement()
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		bool dash = (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftShift));

		if (dash)
			characterMovement.ProcessDash(vertical, horizontal);
		characterMovement.ProcessMovement(vertical, horizontal);

		if(Input.GetKeyDown(KeyCode.Tab))
        {
			HudController.Instance.ToggleStatsInfo();
        }
	}
}
