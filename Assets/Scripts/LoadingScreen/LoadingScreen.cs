using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : NetworkBehaviour
{
    private static LoadingScreen _instance;
    public static LoadingScreen Instance => _instance;

    [SerializeField] private LoadingScreenUI loadingScreenUI;
    [SerializeField] private PlayerCharacter playerPF;


    private const string OPEN_WORLD_SCENE = "OpenWorldScene";
    private const string LOBBY_SCENE = "LobbyScene";
    private const string DUNGEON_SCENE = "DungeonScene";


    private IEnumerator loadingCouru;


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
        if (loadingCouru != null)
            StopCoroutine(loadingCouru);

        loadingCouru = LoadOpenWorld();
        StartCoroutine(loadingCouru);
    }

    private IEnumerator LoadOpenWorld()
    {
        //show loading ui
        //wait untill the scene loads
        Debug.Log("[LoadingScreen,Client] starting to load open world");
        loadingScreenUI.ShowLoadingScreenUI();
		loadingScreenUI.UpdateLoadingProgressText("Scene loading", 0);
		loadingScreenUI.UpdateLoadingBar(0,0,1);
		yield return new WaitForSeconds(1f);
        //Change scene
        var loadSceneTask = SceneManager.LoadSceneAsync(1);

        while(!loadSceneTask.isDone)
        {
            loadingScreenUI.UpdateLoadingProgressText("Scene loading", loadSceneTask.progress * 100);
            loadingScreenUI.UpdateLoadingBar(loadSceneTask.progress * 100, 0, 100);
            yield return null;
        }

        yield return null;


        while(GameManager.Instance == null)
        {
            yield return null;
        }
        GameManager.Instance.InputManager.EnableDenyInput();
        //Spawn player
        if (GameTest.LocalCharacter == null)
        {
            //var spawnTask = serverRunner.SpawnAsync(playerPF);
            while (GameTest.LocalCharacter == null)
            {
                yield return null;
            }
        }
        //place player at spawn position, or get last position and place it near the nearest spawn position
        //check spawn positions
        GameTest.LocalCharacter.transform.position = Vector3.zero;
        GameTest.LocalCharacter.gameObject.SetActive(true);
        //camera
        if (PlayerCamera.Instance)
        {
            PlayerCamera.Instance.InitCamera(GameTest.LocalCharacter.transform);
        }
        else{
            Debug.LogError("Player camera does not exists in this scene");
        }

        //GameTest.LocalCharacter.LoadCharacterData(null);

        //wait untill the multiplayer stuff loads

        //hide loading ui
        yield return new WaitForSeconds(1f);
        loadingScreenUI.HideLoadingScreenUI();
        yield return null;
        GameManager.Instance.InputManager.DisableDenyInput();

        loadingCouru = null;
    }




}
