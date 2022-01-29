using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 1.0f;

	public void LookAt(Vector3 target)
	{
		if (target == Vector3.zero)
			target = this.transform.forward;

		target.y = 0.0f;

		Quaternion targetRotation = Quaternion.LookRotation((target - this.transform.position).normalized, Vector3.up);

		if (rotationSpeed >= 0)
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		else
			this.transform.rotation = targetRotation;

		//// Determine which direction to rotate towards
		//Vector3 targetDirection = target - transform.position;

		//// The step size is equal to speed times frame time.
		//float singleStep = rotationSpeed * Time.deltaTime;

		//// Rotate the forward vector towards the target direction by one step
		//Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

		//// Draw a ray pointing at our target in
		//Debug.DrawRay(transform.position, newDirection, Color.red);

		//// Calculate a rotation a step closer to the target and applies rotation to this object
		//transform.rotation = Quaternion.LookRotation(newDirection);
	}
}
