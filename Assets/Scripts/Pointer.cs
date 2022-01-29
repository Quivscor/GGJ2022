using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
	private void Awake()
	{
		Cursor.visible = false;
	}

	private void OnApplicationFocus(bool focus)
	{
		Cursor.visible = !focus;
	}

	private void LateUpdate()
	{
		this.transform.position = MouseTracker.Instance.WorldPosition;
	}
}
