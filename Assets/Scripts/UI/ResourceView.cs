using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
	[SerializeField] private Image fillBar = null;
	[SerializeField] private Image fillBarBorder = null;

	public void UpdateResourceView(float currentValue)
	{
		fillBar.fillAmount = currentValue;
		fillBarBorder.fillAmount = currentValue;
	}
}
