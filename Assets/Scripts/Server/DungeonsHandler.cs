using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.SceneManagement;

public class DungeonsHandler : MonoBehaviour
{
    [SerializeField] private List<DungeonSession> dungeonSessions;

    public void CreateNewSession(Dictionary<PlayerRef, PlayerCharacter> charactersDic)
    {
        int sessionsAmount = dungeonSessions.Count;
        //TODO: this will cause issues
        var NewDungeonSession = new DungeonSession(ServerHandler.DUNGEON_SESSION_NAME + sessionsAmount, charactersDic);
        dungeonSessions.Add(NewDungeonSession);
        NewDungeonSession.StartDungeonSession();
    }
}
[Serializable]
public class DungeonSession
{
    
    [SerializeField] private Dictionary<PlayerRef, PlayerCharacter> characters;
    [SerializeField] private string SessionName;
    [SerializeField] private NetworkRunner runner;

    //HoldScore
    //Handle Enemies

    public DungeonSession(string sessionName, Dictionary<PlayerRef, PlayerCharacter> charactersDic)
    {
        SessionName = sessionName;
        characters = charactersDic;
    }


    public void StartDungeonSession()
    {
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Dungeon.unity");
        var gameArg = new StartGameArgs
        {
            CustomLobbyName = ServerHandler.CUSTOM_LOBBY_NAME,
            SessionName = SessionName,
            GameMode = GameMode.Server,
            PlayerCount = 5,
            Scene = SceneRef.FromIndex(sceneIndex),
            OnGameStarted = OnDungeonStarted
        };

        var newGO = new GameObject(SessionName);
        runner = newGO.AddComponent<NetworkRunner>();
        runner.StartGame(gameArg);

    }

    private void OnDungeonStarted(NetworkRunner runner)
    {
        foreach (var character in characters)
        {
            character.Value.RPC_TestSwitchSession(SessionName);
        }
    }

}

