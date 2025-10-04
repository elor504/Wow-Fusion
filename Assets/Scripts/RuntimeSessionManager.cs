using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Client related class
/// </summary>
public class RuntimeSessionManager
{
	private static NetworkRunner _myRunner;
	private static List<INetworkRunnerCallbacks[]> addedCallBacks = new();
	public static CharactersList CharactersList = new CharactersList();
	public static PlayerCharacter LocalCharacter;
	public static EntityManager EntityManager = new EntityManager();
	public static FusionManager FusionManager;

	public static Party LocalParty;


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
			UnityEngine.Object.Destroy(_myRunner);

		addedCallBacks.Clear();
		GameObject runnerObj = UnityEngine.Object.Instantiate(new GameObject());
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
		GameObject runnerObj = UnityEngine.Object.Instantiate(new GameObject());
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
		// RefreshNetworkRunner(true);
		// SceneManager.LoadScene("Login_Scene");
		Debug.Log("[RuntimeSessionManager] Return to login menu is not implemented");
	}
	#endregion
	public static bool ComparePlayerRef(PlayerRef playerRef)
	{
		if (LocalCharacter == null)
			return false;

		return LocalCharacter.Object.InputAuthority == playerRef;
	}
	public static bool ComparePlayerCharacter(PlayerCharacter characterToCompare) => characterToCompare.CharacterName.Equals(LocalCharacter.CharacterName);
}

[Serializable]
public class EntityManager
{
	public List<ITargetableEntity> TargetableEntities = new();

	public bool TryGetEnemyByNetworkID(NetworkId networkID, out ITargetableEntity enemyResult)
	{
		enemyResult = null;
		foreach (var enemy in TargetableEntities)
		{
			if (enemy.GetNetworkId() == networkID)
			{
				enemyResult = enemy;
				return true;
			}
		}
		return false;
	}
	public void AddEnemyToList(ITargetableEntity enemy)
	{
		TargetableEntities.Add(enemy);
	}
	public void RemoveEnemyFromList(ITargetableEntity enemy)
	{
		TargetableEntities.Remove(enemy);
	}

}