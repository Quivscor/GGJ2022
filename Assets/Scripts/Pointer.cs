using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
	[SerializeField] private WandController wandController = null;
	[SerializeField] private Animator animator = null;

	private void Awake()
	{
		Cursor.visible = false;
		wandController.SpellTypeChanged += SwapPointer;
	}

	private void OnApplicationFocus(bool focus)
	{
		Cursor.visible = !focus;
	}

	private void LateUpdate()
	{
		this.transform.position = MouseTracker.Instance.WorldPosition;
	}

	private void SwapPointer(SpellType spellType)
	{
		animator?.SetBool("usingHarmony", spellType == SpellType.Harmony);
	}
}
