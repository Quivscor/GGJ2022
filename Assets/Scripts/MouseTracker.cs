using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
	public static MouseTracker Instance = null;

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
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit, Mathf.Infinity))
		{
			Vector3 newPosition = hit.point;
			newPosition.y = 0.0f;

			WorldPosition = newPosition;
			return;
		}
	}
}
