using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashView : MonoBehaviour
{
	[SerializeField] private Image leftSideFill = null;
	[SerializeField] private Image rightSideFill = null;

	[SerializeField] private CharacterMovement characterMovement = null;

	private void Awake()
	{
		characterMovement.DashValuesChanged += UpdateDashView;
	}

	private void UpdateDashView(int availableDashes, float progressToUnlockNextDash)
	{
		if (availableDashes == 0)
		{
			leftSideFill.fillAmount = progressToUnlockNextDash;
			rightSideFill.fillAmount = 0.0f;
		}
		else if (availableDashes == 1)
		{
			leftSideFill.fillAmount = 1.0f;
			rightSideFill.fillAmount = progressToUnlockNextDash;
		}
		else if (availableDashes == 2)
		{
			leftSideFill.fillAmount = 1.0f;
			rightSideFill.fillAmount = 1.0f;
		}
	}
}
