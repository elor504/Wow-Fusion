using System.Collections.Generic;
using UnityEngine;

public abstract class BasePoolSystem<T> : MonoBehaviour where T : MonoBehaviour
{
    private protected List<T> _objectPool = new List<T>();

    public abstract void InitPool();

    public abstract bool TryToRespawnObject(T objPrefab,Vector3 position,out T instantiatedObject);
    public abstract T GetAvailableObjectFromPool(T objID, Vector3 position);
}