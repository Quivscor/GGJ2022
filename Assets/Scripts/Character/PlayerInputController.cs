using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
	[SerializeField] private CharacterMovement characterMovement = null;
	[SerializeField] private CharacterRotator characterRotator = null;
	[SerializeField] private SpellcastingController SpellcastingController = null;
	private PlayerInput input;
	private InputAction moveAction;
	private InputAction lookAction;
	private InputAction fireLeftAction;
	private InputAction fireRightAction;
	private Vector2 playerMovement;
	private Vector2 reticleMovement;
	private Vector3 reticleRelativePosition;
	private Vector3 reticleVelocityRef;

	[SerializeField] private Pointer pointer;
	[SerializeField] private float pointerSensitivity;
	[SerializeField] private float reticleMaxDistanceFromPlayer; //view area radius

    private void Awake()
    {
		input = GetComponent<PlayerInput>();

		fireLeftAction = input.currentActionMap.FindAction("FireLeft");
		fireRightAction = input.currentActionMap.FindAction("FireRight");
		input.currentActionMap.FindAction("Dash").performed += Dash;

		moveAction = input.currentActionMap.FindAction("Move");
		lookAction = input.currentActionMap.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
	}

    private void Start()
    {
		Vector3 position = this.transform.position;
		pointer.transform.position = new Vector3(position.x, 0, position.z);
		reticleRelativePosition = Vector3.zero;
		PlayerInputConstants.reticleTransform = pointer.transform;
    }

    private void Update()
	{
		playerMovement = moveAction.ReadValue<Vector2>();
		reticleMovement = lookAction.ReadValue<Vector2>();

		if (fireLeftAction.ReadValue<float>() > 0.5f)
			FireLeft();
		if (fireRightAction.ReadValue<float>() > 0.5f)
			FireRight();

		ProcessMovement();
		ProcessRotation();
		ProcessReticle();
	}

    private void ProcessRotation()
	{
		characterRotator.LookAt(pointer.transform.position);
	}

	private void Dash(InputAction.CallbackContext context)
    {
		characterMovement.ProcessDash(playerMovement.y, playerMovement.x);
	}

	private void FireLeft()
    {
		SpellcastingController.ProcessSpell(SpellType.Left);
    }

	private void FireRight()
    {
		SpellcastingController.ProcessSpell(SpellType.Right);
    }

	private void ProcessMovement()
	{
		characterMovement.ProcessMovement(playerMovement.y, playerMovement.x);

		if(Input.GetKeyDown(KeyCode.Tab))
        {
			HudController.Instance.ToggleStatsInfo();
        }
	}

	private void ProcessReticle()
    {
		Vector3 relativePositionDelta = new Vector3(reticleMovement.x, 0, reticleMovement.y) * Time.deltaTime * pointerSensitivity;
		Vector3 newRelativePos = reticleRelativePosition + relativePositionDelta;
		float distance = Vector3.Distance(this.transform.position, this.transform.position + newRelativePos);
		if (distance > reticleMaxDistanceFromPlayer)
		{
			newRelativePos *= reticleMaxDistanceFromPlayer / distance;
		}
		reticleRelativePosition = newRelativePos;

		pointer.transform.position = Vector3.SmoothDamp(pointer.transform.position, this.transform.position + reticleRelativePosition, ref reticleVelocityRef, 0.1f);
	}
}

public static class PlayerInputConstants
{
	public static Transform reticleTransform;
}