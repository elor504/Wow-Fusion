using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemMediator : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    public ParticleSystem GetParticleSystem => particleSystem;
    
    public event Action OnParticleSystemStoppedEvent;
    
    private void OnParticleSystemStopped()
    {
        OnParticleSystemStoppedEvent?.Invoke();
    }
}
