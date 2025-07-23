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


    private IEnumerator loadingCouru;


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
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

        //Spawn player
        var spawnTask = GameTest.GetMyRunner().SpawnAsync(playerPF);

        while (spawnTask.IsQueued)
        {
            yield return null;
        }
        GameTest.LocalCharacter = spawnTask.Object.GetComponent<PlayerCharacter>();
        //GameTest.LocalCharacter.LoadCharacterData(null);
        //place player at spawn position, or get last position and place it near the nearest spawn position
        //SpawnManager
        //wait untill the multiplayer stuff loads

        //hide loading ui

        loadingCouru = null;
    }

    


}
