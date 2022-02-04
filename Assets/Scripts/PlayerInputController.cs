using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
	[SerializeField] private CharacterMovement characterMovement = null;
	[SerializeField] private CharacterRotator characterRotator = null;
	[SerializeField] private WandController wandController = null;

	private Controls _controls;
	private Vector2 _inputVector;

    private void Awake()
    {
		_controls = new Controls();
		_controls.Enable();
		_controls.Gameplay.Dash.performed += ProcessDash;
	}

    private void Update()
	{
		ProcessRotation();

		_inputVector = _controls.Gameplay.Movement.ReadValue<Vector2>();
		ProcessMovement();

		if (_controls.Gameplay.FireRight.IsPressed())
			ProcessFireRight();
		if (_controls.Gameplay.FireLeft.IsPressed())
			ProcessFireLeft();
	}

	private void ProcessRotation()
	{
		characterRotator.LookAt(MouseTracker.Instance.WorldPosition);
	}

	private void ProcessMovement()
	{
		characterMovement.ProcessMovement(_inputVector.y, _inputVector.x);
	}

	private void ProcessFireLeft()
    {
		wandController.ProcessSpell(true, false);
	}

	private void ProcessFireRight()
	{
		wandController.ProcessSpell(false, true);
	}

	private void ProcessDash(InputAction.CallbackContext context)
    {
		characterMovement.ProcessDash(_inputVector.y, _inputVector.x);
	}
}
