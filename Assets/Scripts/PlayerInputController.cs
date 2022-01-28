using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
	[SerializeField] private CharacterMovement characterMovement = null;

	private void Update()
	{
		ProcessMovement();
		ProcessMouseInputs();
	}

	private void ProcessMouseInputs()
	{
		//add your code here :)
	}

	private void ProcessMovement()
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		characterMovement.ProcessMovement(vertical, horizontal);
	}
}
