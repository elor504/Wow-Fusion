using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTest
{
    private static NetworkRunner _myRunner;
    private static List<INetworkRunnerCallbacks[]> addedCallBacks = new();


    public static PlayerCharacter LocalCharacter;
    public static FusionManager FusionManager;


    #region network runner
    public static void RefreshNetworkRunner(bool destory = false)
    {
        if (_myRunner != null)
        {
            if (addedCallBacks != null)
            {
                for (int i = 0; i < addedCallBacks.Count; i++)
                {
                    if (addedCallBacks[i] != null)
                    {
                        RemoveCallBacks(addedCallBacks[i]);
                    }
                }

            }
        }
        if (destory)
            Object.Destroy(_myRunner);

        addedCallBacks.Clear();
        GameObject runnerObj = Object.Instantiate(new GameObject());
        var runner = runnerObj.AddComponent<NetworkRunner>();
        runner.gameObject.name = "NetworkRunner";
        Debug.Log("Refreshed new gameobject");
        _myRunner = runner;
    }
    public static void AddCallBacks(params INetworkRunnerCallbacks[] callbacks)
    {
        addedCallBacks.Add(callbacks);
        GetMyRunner().AddCallbacks(callbacks);
    }
    public static void RemoveCallBacks(params INetworkRunnerCallbacks[] callbacks)
    {
        addedCallBacks.Remove(callbacks);
        GetMyRunner().RemoveCallbacks(callbacks);
    }
    public static NetworkRunner CreateNewRunner(params INetworkRunnerCallbacks[] callbacks)
    {
        if (_myRunner != null)
        {
            foreach (var callback in addedCallBacks)
            {
                RemoveCallBacks(callback);
            }
        }
        GameObject runnerObj = Object.Instantiate(new GameObject());
        var runner = runnerObj.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;
        runner.gameObject.name = "NetworkRunner";
        Debug.Log("Created new gameobject");
        return runner;
    }
    public static NetworkRunner GetMyRunner()
    {
        if (_myRunner == null)
        {
            _myRunner = CreateNewRunner();
        }


        return _myRunner;
    }

    public static void ReturnToLoginMenu()
    {
        //TODO: Check if already logged in playfab
        //If logged in then login or logout to prevent bugs
        RefreshNetworkRunner(true);
        SceneManager.LoadScene("Login_Scene");
    }
    #endregion

}
