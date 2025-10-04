using Fusion;
using System.Collections;
using UnityEngine;

public class LoadingScreen : NetworkBehaviour
{
	private static LoadingScreen _instance;
	public static LoadingScreen Instance => _instance;

	[SerializeField] private LoadingScreenUI loadingScreenUI;


	private const string OPEN_WORLD_SCENE = "OpenWorldScene";
	private const string LOBBY_SCENE = "LobbyScene";
	private const string DUNGEON_SCENE = "DungeonScene";


	private bool _isLoadingScreenActive;
	private IEnumerator _loadingCouru;

	public bool IsLoadingScreenActive => _isLoadingScreenActive;
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void LoadIntoOpenWorld()
	{
		if (_loadingCouru != null)
			StopCoroutine(_loadingCouru);

		_loadingCouru = LoadOpenWorld();
		StartCoroutine(_loadingCouru);
	}
	private IEnumerator LoadOpenWorld()
	{
		_isLoadingScreenActive = true;
		//show loading ui
		//wait untill the scene loads
		Debug.Log("[LoadingScreen,Client] starting to load open world");
		loadingScreenUI.ShowLoadingScreenUI();
		loadingScreenUI.UpdateLoadingProgressText("Scene loading", 0);
		loadingScreenUI.UpdateLoadingBar(0, 0, 1);
		yield return new WaitForSeconds(1f);

		RuntimeSessionManager.GetMyRunner().StartGame(new StartGameArgs
		{
			CustomLobbyName = ServerHandler.CUSTOM_LOBBY_NAME,
			SessionName = "Lobby_0",
			GameMode = GameMode.Client,
			PlayerCount = 20
		});



		while (GameManager.Instance == null)
		{
			yield return null;
		}
		yield return null;


		while (RuntimeSessionManager.LocalCharacter == null)
		{
			yield return null;
		}
		RuntimeSessionManager.LocalCharacter.InputManager.DenyInput();
		yield return null;

		//place player at spawn position, or get last position and place it near the nearest spawn position
		//check spawn positions
		RuntimeSessionManager.LocalCharacter.transform.position = Vector3.zero;
		RuntimeSessionManager.LocalCharacter.gameObject.SetActive(true);
		yield return null;
		RuntimeSessionManager.LocalCharacter.UpdateCharacterName(RuntimeSessionManager.FusionManager.SelectedCharacterData.CharacterName);
		yield return null;
		RuntimeSessionManager.LocalCharacter.UpdateCharacterVisualData(RuntimeSessionManager.FusionManager.SelectedCharacterData.SerializeVisual());
		yield return null;
		RuntimeSessionManager.LocalCharacter.UpdateCharacterEquipmentData(RuntimeSessionManager.FusionManager.SelectedCharacterData.CharacterEquipmentData);
		yield return null;
		//camera
		if (PlayerCamera.Instance)
		{
			PlayerCamera.Instance.InitCamera(RuntimeSessionManager.LocalCharacter.transform);
		}
		else
		{
			Debug.LogError("Player camera does not exists in this scene");
		}


		//wait untill the multiplayer stuff loads
		yield return new WaitForSeconds(1f);

		//hide loading ui
		loadingScreenUI.HideLoadingScreenUI();
		yield return null;
		RuntimeSessionManager.LocalCharacter.InputManager.AllowInput();

		_loadingCouru = null;
		_isLoadingScreenActive = false;
	}
}

