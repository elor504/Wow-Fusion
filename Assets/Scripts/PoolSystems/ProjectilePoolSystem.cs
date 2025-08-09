using Fusion;
using UnityEngine;


public class ProjectilePoolSystem : BasePoolSystem<BaseProjectile>
{
    private static ProjectilePoolSystem _instance;
    public static ProjectilePoolSystem Instance => _instance;


    public override void Spawned()
    {
        base.Spawned();
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

    public override bool TryToRespawnObject(BaseProjectile objPrefab, Vector3 position, out BaseProjectile instantiatedObject)
    {
        instantiatedObject = Instantiate(objPrefab, position, Quaternion.identity, transform);
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

    public void RequestSpawnAndInitProjectile(string projectileID, Vector3 spawnPosition, NetworkId caster, NetworkId target, int damage, float speed)
    {
        RPC_SpawnAndInitProjectile(projectileID, spawnPosition, caster, target, damage, speed);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SpawnAndInitProjectile(string projectileID, Vector3 spawnPosition, NetworkId caster, NetworkId target, int damage, float speed)
    {
        var projectilePF = DataBankSO.Instance.GetProjectile(projectileID);

        var casterEntity = Object.Runner.FindObject(caster).GetComponent<ITargetableEntity>();
        var targetEntity = Object.Runner.FindObject(target).GetComponent<ITargetableEntity>();

        if (TryToRespawnObject(projectilePF, spawnPosition, out var spawnProjectile))
        {
            spawnProjectile.InitProjectile(spawnPosition, casterEntity, targetEntity, damage, speed);
        }
        else
        {
            Debug.LogError("Failed to spawn and init projectile");
        }
    }
}