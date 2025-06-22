using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace Homework
{
    public class NetworkRunnerInjector : MonoBehaviour
    {
        private static NetworkRunnerInjector _instance;
        public static NetworkRunnerInjector Instance => _instance;

        [SerializeField] private NetworkRunner runner;

        private List<INetworkRunnerRequired> _objToInject = new List<INetworkRunnerRequired>();


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(_instance.gameObject);
            }
        }

        public void AddInjector(INetworkRunnerRequired obj)
        {
            obj.InjectRunner(runner);
            _objToInject.Add(obj);
        }
        public void RemoveInjected(INetworkRunnerRequired obj)
        {
            _objToInject.Remove(obj);
        }
    }
}