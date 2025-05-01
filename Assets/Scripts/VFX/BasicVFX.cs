using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicVFX : MonoBehaviour
{
    [SerializeField] private string vfxID;
    [SerializeField] private ParticleSystemMediator particleSystemMediator;

    public event Action<BasicVFX> OnLifeEnded;
    
    public string GetVfxID => vfxID;

    private bool _isActive;
    public bool IsActive => _isActive;


    private void OnEnable()
    {
        particleSystemMediator.OnParticleSystemStoppedEvent += OnVFXLifeEnded;
    }

    private void OnDisable()
    {
        particleSystemMediator.OnParticleSystemStoppedEvent -= OnVFXLifeEnded;
    }

    public virtual void StopParticleSystem()
    {
        particleSystemMediator.GetParticleSystem.Stop();
    }
    
    public virtual void InitVFX(Vector3 position)
    {
        transform.position = position;
        _isActive = true;
        gameObject.SetActive(true);
        particleSystemMediator.GetParticleSystem.Play();
    }

    public virtual void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public virtual void OnVFXLifeEnded()
    {
        _isActive = false;
        gameObject.SetActive(false);
        OnLifeEnded?.Invoke(this);
    }
}