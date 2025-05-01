using System;
using UnityEngine;


public class ProjectilePoolSystem : BasePoolSystem<BaseProjectile>
{
    private static ProjectilePoolSystem _instance;
    public static ProjectilePoolSystem Instance => _instance;
    
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

    public override bool TryToRespawnObject(BaseProjectile objPrefab, Vector3 position,out BaseProjectile instantiatedObject)
    {
        instantiatedObject   = Instantiate(objPrefab,position,Quaternion.identity,transform);
        return instantiatedObject;
    }

    public override BaseProjectile GetAvailableObjectFromPool(BaseProjectile objPrefab, Vector3 position)
    {
        foreach (var projectile in _objectPool)
        {
            if (projectile.ProjectileID == objPrefab.ProjectileID && !projectile.IsActive)
            {
                return projectile;
            }
        }

        if (TryToRespawnObject(objPrefab, position, out var instantiatedObject))
        {
            _objectPool.Add(instantiatedObject);
            return instantiatedObject;
        }
        Debug.LogError("Attempting to spawn projectile but it return as null");
        return null;
    }
}