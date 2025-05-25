using Fusion;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header(("References"))]
    [SerializeField] private NetworkRunner networkRunner;
    [SerializeField] private LobbyManager lobbyManager;
    [Header("Game Settings")]
    [SerializeField] private GameMode gameMode;

    [SerializeField] private string sessionName;
    [ContextMenu("Start Game")]
    public void StartGame()
    {
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            OnGameStarted = OnGameStarted
        });


    }

    private void OnGameStarted(NetworkRunner obj)
    {
        Debug.Log("Game Started!");
        lobbyManager.Init(obj);
    }

}
