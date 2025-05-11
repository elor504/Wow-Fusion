using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class CharacterVFXVisual : MonoBehaviour
{
    [SerializeField] private List<VFXVisualPosition> visualPositions;

    public bool TryGetVisualPositionParent(string id, out Transform parent)
    {
        foreach (var position in visualPositions)
        {
            if (position.ID == id)
            {
                parent = position.parent;
                return true;
            }
        }
        Debug.LogError($"There is no visual position with the id: {id}");
        parent = null;
        return false;
    }
    
    
}

[Serializable]
public class VFXVisualPosition
{
public string ID;
public Transform parent;
}