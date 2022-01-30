using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVisualEffect : MonoBehaviour
{
	[SerializeField] private Object[] componentsToDestroy = null;
	[SerializeField] private Animator animator = null;
	[SerializeField] private Material disappearMaterial = null;
	[SerializeField] private ParticleSystem dashParticles = null;

	[SerializeField] private float disappearingSpeed = 1.0f;
	[SerializeField] private float lifetime = 2.0f;

	private bool isInitialized = false;

	public void PrepareClone()
	{
		foreach (Object obj in componentsToDestroy)
			Destroy(obj);

		Destroy(animator);

		SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
		{
			Material[] newMaterials = new Material[skinnedMeshRenderer.materials.Length];

			for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
				newMaterials[i] = disappearMaterial;

			skinnedMeshRenderer.materials = newMaterials;
		}

		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			Material[] newMaterials = new Material[meshRenderer.materials.Length];

			for (int i = 0; i < meshRenderer.materials.Length; i++)
				newMaterials[i] = disappearMaterial;

			meshRenderer.materials = newMaterials;
		}

		isInitialized = true;
		Destroy(this.gameObject, lifetime);
	}

	Renderer[] renderers = null;

	private void Update()
	{
		if (isInitialized)
		{
			if (renderers == null)
				renderers = GetComponentsInChildren<Renderer>();

			foreach (Renderer renderer in renderers)
			{
				if (renderer.GetComponent<ParticleSystem>() != null)
					continue;

				renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				renderer.receiveShadows = false;

				foreach (Material material in renderer.materials)
					material.SetFloat("_GlowPower", renderer.material.GetFloat("_GlowPower") + Time.deltaTime * disappearingSpeed);
			}
		}
	}

	public void ProcessDash()
	{
		GameObject dummy = Instantiate(this.gameObject, this.transform.position, this.transform.rotation);
		dummy.GetComponent<DashVisualEffect>().PrepareClone();

		dashParticles?.Play();
	}
}
