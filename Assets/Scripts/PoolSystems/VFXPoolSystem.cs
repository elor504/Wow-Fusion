using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPoolSystem : BasePoolSystem<BasicVFX>
{
    private static VFXPoolSystem _instance;
    public static VFXPoolSystem Instance => _instance;
    
    private void Awake()
    {
        InitPool();
    }
    
    public override void InitPool()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public override bool TryToRespawnObject(BasicVFX objPrefab, Vector3 position,out BasicVFX instantiatedObject)
    {
        instantiatedObject   = Instantiate(objPrefab,position,Quaternion.identity,transform);
        return instantiatedObject;
    }
    
    public override BasicVFX GetAvailableObjectFromPool(BasicVFX objPrefab, Vector3 position)
    {
        foreach (var vfx in _objectPool)
        {
            if (vfx.GetVfxID == objPrefab.GetVfxID && !vfx.IsActive)
            {
                vfx.OnLifeEnded += OnVFXLifeEnded;
                return vfx;
            }
        }

        if (TryToRespawnObject(objPrefab, position, out var instantiatedObject))
        {
            _objectPool.Add(instantiatedObject);
            instantiatedObject.OnLifeEnded += OnVFXLifeEnded;
            return instantiatedObject;
        }
        Debug.LogError("Attempting to spawn VFX but it return as null");
        return null;
    }

    private void OnVFXLifeEnded(BasicVFX vfx)
    {
        vfx.OnLifeEnded -= OnVFXLifeEnded;
        
        vfx.SetParent(transform);
    }
}
