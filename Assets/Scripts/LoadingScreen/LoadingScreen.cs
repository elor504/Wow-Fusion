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
		//Spawn player
		if (GameTest.LocalCharacter == null)
        {
            var spawnTask = GameTest.GetMyRunner().SpawnAsync(playerPF);

            while (spawnTask.IsQueued)
            {
                yield return null;
            }
            GameTest.LocalCharacter = spawnTask.Object.GetComponent<PlayerCharacter>();
        }
        //check spawn positions
        GameTest.LocalCharacter.transform.position = Vector3.zero;

        //GameTest.LocalCharacter.LoadCharacterData(null);
        //place player at spawn position, or get last position and place it near the nearest spawn position
        //SpawnManager
        //wait untill the multiplayer stuff loads

        //hide loading ui
        yield return new WaitForSeconds(1f);
        loadingScreenUI.HideLoadingScreenUI();
		loadingCouru = null;
    }




}
