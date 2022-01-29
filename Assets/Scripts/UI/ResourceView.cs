using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour
{
	[SerializeField] private Image fillBar = null;

	public void UpdateResourceView(float currentValue)
	{
		fillBar.fillAmount = currentValue;
	}
}
