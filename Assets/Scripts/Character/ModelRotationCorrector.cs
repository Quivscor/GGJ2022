using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotationCorrector : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
	[SerializeField] private Transform boneToRotationFix = null;
	[SerializeField] private Transform spellSpawner = null;

	private void LateUpdate()
	{
		bool isAttacking = animator.GetBool("isAttacking");

		if (isAttacking)
		{
			boneToRotationFix.LookAt(spellSpawner.position);
			boneToRotationFix.rotation = Quaternion.Euler(boneToRotationFix.eulerAngles + new Vector3(30, 90, 30));
		}
	}
}
