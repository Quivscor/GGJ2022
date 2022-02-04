using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTracker : MonoBehaviour
{
	public static MouseTracker Instance = null;

	[SerializeField] private LayerMask layerMasks;
	private PlayerInput _input;
	private Controls _controls;

	private const string gamepadScheme = "Gamepad";
	private const string keyboardScheme = "Keyboard&Mouse";

	private void Awake()
	{
		if (MouseTracker.Instance == null)
			MouseTracker.Instance = this;
		else
			Destroy(this);
	}

	public Vector3 WorldPosition { get; private set; }

	private void Update()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(GeneralizedCursor.cursorPosition);

		RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, layerMasks))
		{
			Vector3 newPosition = hit.point;
			newPosition.y = 0.0f;

			WorldPosition = newPosition;
			return;
		}
	}
}
