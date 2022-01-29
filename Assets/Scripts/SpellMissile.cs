using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissile : MonoBehaviour
{
	private GameObject parent = null;

	public void Initialize(GameObject parent)
	{
		this.parent = parent;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.gameObject == parent)
			return;
		if (other.transform.root.CompareTag("Bullet"))
			return; 

		Destroy(this.gameObject);
	}
}
