using Fusion;
using System.Collections;
using UnityEngine;

public class LoadingScreen : NetworkBehaviour
{
    private static LoadingScreen _instance;
    public static LoadingScreen Instance => _instance;

    [SerializeField] private LoadingScreenUI loadingScreenUI;
    [SerializeField] private PlayerCharacter playerPF;


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

        GameTest.GetMyRunner().StartGame(new StartGameArgs
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

        //Spawn player
        if (GameTest.LocalCharacter == null)
        {
            //var spawnTask = serverRunner.SpawnAsync(playerPF);
            while (GameTest.LocalCharacter == null)
            {
                yield return null;
            }
        }
        GameTest.LocalCharacter.InputManager.EnableDenyInput();

        //place player at spawn position, or get last position and place it near the nearest spawn position
        //check spawn positions
        GameTest.LocalCharacter.transform.position = Vector3.zero;
        GameTest.LocalCharacter.gameObject.SetActive(true);
        //camera
        if (PlayerCamera.Instance)
        {
            PlayerCamera.Instance.InitCamera(GameTest.LocalCharacter.transform);
        }
        else
        {
            Debug.LogError("Player camera does not exists in this scene");
        }

        //GameTest.LocalCharacter.LoadCharacterData(null);

        //wait untill the multiplayer stuff loads

        //hide loading ui
        yield return new WaitForSeconds(1f);
        loadingScreenUI.HideLoadingScreenUI();
        yield return null;
        GameTest.LocalCharacter.InputManager.DisableDenyInput();

        _loadingCouru = null;
        _isLoadingScreenActive = false;
    }
}
