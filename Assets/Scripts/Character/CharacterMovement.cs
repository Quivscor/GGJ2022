using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 2.0f;
	[SerializeField] private float dashSpeed = 10f;
	[SerializeField] private float dashCooldown = 2f;
	[SerializeField] private float dashDuration = .5f;

	private float dashCooldownCurrentTime = 0;
	private float dashDurationCurrentTime = 0;
	private bool isDashing = false;
	private int availableDashes = 2;
	private int currentDashes = 0;
	public event Action<int, float> DashValuesChanged = null;
	[SerializeField] private ParticleSystem slowParticles = null;
	
	[Header("References:")]
	[SerializeField] private Animator animator = null;
	[SerializeField] private Transform character = null;
	[SerializeField] private DashVisualEffect dashVisualEffects = null;
	private float basicMovementSpeed;

	#region Slow Effect
	[Header("Slow Effect Options:")]
	[SerializeField] private float slowEffectTime = 2.0f;
	[SerializeField] private float slowedMovementSpeedFactor = 0.5f;
	
	private bool isSlowed = false;
	private Coroutine slowedEffectCoroutine = null;
	#endregion

	private new Rigidbody rigidbody = null;
	public AudioSource dashSound;

    public float BasicMovementSpeed { get => basicMovementSpeed; set => basicMovementSpeed = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    private void Awake()
	{
		if (animator == null)
			animator = GetComponentInChildren<Animator>();

		rigidbody = GetComponent<Rigidbody>();

		currentDashes = availableDashes;
		BasicMovementSpeed = MovementSpeed;
	}

    private void Update()
    {
		if(currentDashes != availableDashes)
		{
			dashCooldownCurrentTime -= Time.deltaTime;

			if (dashCooldownCurrentTime <= 0)
			{
				currentDashes++;

				if (currentDashes != availableDashes)
					dashCooldownCurrentTime = dashCooldown;
			}

			DashValuesChanged?.Invoke(currentDashes, (dashCooldown - dashCooldownCurrentTime) / dashCooldown);
		}

		if (dashDurationCurrentTime > 0)
			dashDurationCurrentTime -= Time.deltaTime;
		else if (isDashing)
			isDashing = false;
    }

    public void ProcessMovement(float vertical, float horizontal)
	{
		if (isDashing)
			return;

		Vector3 velocityToApply = new Vector3(horizontal, 0, vertical).normalized * MovementSpeed;

		if (isSlowed)
			velocityToApply *= slowedMovementSpeedFactor;

		rigidbody.velocity = velocityToApply;

		ProcessAnimator(vertical, horizontal);
	}

	public void ProcessDash(float vertical, float horizontal)
    {
		if (currentDashes <= 0 || isDashing)
			return;

		isDashing = true;

		if (dashSound != null)
			dashSound.Play();
		dashDurationCurrentTime = dashDuration;

		Vector3 velocityToApply = new Vector3(horizontal, 0, vertical).normalized * dashSpeed;

		if (isSlowed)
			velocityToApply *= slowedMovementSpeedFactor;

		rigidbody.velocity = velocityToApply;

		if (currentDashes != 1)
			dashCooldownCurrentTime = dashCooldown;

		dashVisualEffects?.ProcessDash();
		ProcessAnimator(vertical, horizontal);
		currentDashes--;
		DashValuesChanged?.Invoke(currentDashes, (dashCooldown - dashCooldownCurrentTime) / dashCooldown);
	}

	public void IncreaseMovementSpeed(float value)
    {
		MovementSpeed += value;
    }
	
	private void ProcessAnimator(float vertical, float horizontal)
	{
		//	bool swap = false;

		//	if (Mathf.Abs(this.transform.forward.x) > 0.5f)
		//	{
		//		float tmp = horizontal;
		//		horizontal = vertical;
		//		vertical = tmp;
		//		swap = true;
		//	}

		//	if (this.transform.forward.z < 0)
		//		if (!swap)
		//			vertical *= -1;


		//	animator.SetFloat("verticalMovement", vertical);
		//	animator.SetFloat("horizontalMovement", horizontal);
		//}
		animator?.SetBool("isMoving", new Vector3(horizontal, 0.0f, vertical) != Vector3.zero);

		int quarterIndex = (int)(character.localRotation.eulerAngles.y / 90);

		if (quarterIndex * 90 + 45 < character.localRotation.eulerAngles.y)
			quarterIndex++;

		quarterIndex = quarterIndex % 4;

		if (quarterIndex == 0 || quarterIndex == 2)
		{
			animator?.SetFloat("verticalMovement", vertical * (quarterIndex == 2 ? -1.0f : 1.0f));
			animator?.SetFloat("horizontalMovement", horizontal * (quarterIndex == 2 ? -1.0f : 1.0f));
		}
		else
		{
			animator?.SetFloat("verticalMovement", horizontal * (quarterIndex == 3 ? -1.0f : 1.0f));
			animator?.SetFloat("horizontalMovement", vertical * (quarterIndex == 3 ? 1.0f : -1.0f));
		}
	}

	public void AddSlowEffect()
	{
		if (slowedEffectCoroutine != null)
		{
			StopCoroutine(slowedEffectCoroutine);
			slowParticles?.Stop();
		}

		slowedEffectCoroutine = StartCoroutine(SlowEffectTimer());
		slowParticles?.Play();
	}

	private IEnumerator SlowEffectTimer()
	{
		isSlowed = true;
		yield return new WaitForSeconds(slowEffectTime);
		isSlowed = false;
	}
}