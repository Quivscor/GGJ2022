using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMaterialReplacer : MonoBehaviour
{
	public Material disappearMaterial;

	List<Material[]> oldSkinnedMaterials = new List<Material[]>();
	List<Material[]> oldMaterials = new List<Material[]>();
	bool materialsSet = false;

    public void UseNewMat()
    {
		SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
		{
			Material[] newMaterials = new Material[skinnedMeshRenderer.materials.Length];

			for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
				newMaterials[i] = disappearMaterial;

			if(!materialsSet)
				oldSkinnedMaterials.Add(skinnedMeshRenderer.materials);
			skinnedMeshRenderer.materials = newMaterials;
		}

		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			Material[] newMaterials = new Material[meshRenderer.materials.Length];

			for (int i = 0; i < meshRenderer.materials.Length; i++)
				newMaterials[i] = disappearMaterial;

			if(!materialsSet)
				oldMaterials.Add(meshRenderer.materials);
			meshRenderer.materials = newMaterials;
		}
		materialsSet = true;
	}

	public void RevertMat()
    {
		if (!materialsSet)
			return;
		SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		for(int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
			skinnedMeshRenderers[i].materials = oldSkinnedMaterials[i];
        }
		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
		for(int i = 0; i < meshRenderers.Length; i++)
        {
			meshRenderers[i].materials = oldMaterials[i];
        }
	}

}
