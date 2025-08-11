using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentObject : MonoBehaviour
{
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] private Transform[] boness;

    private string _equipmentID;
    [SerializeField]private List<Transform> _originalBonesName = new List<Transform>();

    public string EquipmentID => _equipmentID;
    public EquipmentType EquipmentType => equipmentType;

    [ContextMenu("Test")]
    public void ShowBones()
    {
        Debug.Log("Bones amount " + skinnedMeshRenderers[0].bones.Count());
        foreach (var bone in skinnedMeshRenderers[0].bones)
        {
            Debug.Log("Bone name: " + bone.name);
        }
        _originalBonesName = skinnedMeshRenderers[0].bones.ToList();
    }
    public void Init()
    {
        _equipmentID = equipmentType.ToString();
    }
    public void Init(string id)
    {
        _equipmentID = id;
     
    }
    public void Init(string id,Transform root,Transform[] bones)
    {
        _equipmentID = id;
        List<Transform> bonesList = new List<Transform>();
        bonesList.AddRange(bones);
        List<Transform> filteredBones = new List<Transform>();
        Transform[] hierachyFilteredBones;

        foreach (var bone in bones)
        { 
            if(_originalBonesName.Any(b => b.name == bone.name))
            {
                filteredBones.Add(bone);
            }
        }
        hierachyFilteredBones = new Transform[filteredBones.Count];
        //Organize the hierachy of the bones (MY BRAIN HURTS, but no chatgpt YEET)
        foreach (var bone in filteredBones) 
        {
            Transform sameTrans = _originalBonesName.Find(b => b.name == bone.name);
            int wantedIndex = _originalBonesName.IndexOf(sameTrans);
            hierachyFilteredBones[wantedIndex] = bone;         
        }


        Debug.Log($"Filtered bone size: {hierachyFilteredBones.Length}");
        foreach (var skinnedMesh in skinnedMeshRenderers)
        { 
            skinnedMesh.bones = hierachyFilteredBones;
            skinnedMesh.rootBone = root;
        }
    }

}
