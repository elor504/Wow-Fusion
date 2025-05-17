
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairMeshes : MonoBehaviour
{
    [SerializeField] private List<SkinnedMeshRenderer> hairMeshes;

    public void ChangeHairMeshesColor(Color color)
    {
    foreach (var hairMesh in hairMeshes)
    {
            hairMesh.material.color = color;
    }
    }

}
