using Fusion;
using UnityEngine;

public class TirgulRoomManager : MonoBehaviour
{
	[SerializeField] private NetworkRunner networkRunner;
	[Header("Game Settings")]
	[SerializeField] private GameMode gameMode;
	[SerializeField] private string sessionName;


	[Header("Lobby Instance")]
	[SerializeField] private TirgulLobbyInstance lobbyPF;

	public TirgulLobbyInstance lobbyInstance;

	public static TirgulRoomManager Instance { get; private set; }

	private int _currentPlayersAmount;

	private void Awake()
	{
		Instance = this;

		networkRunner.StartGame(new StartGameArgs
		{
			GameMode = gameMode,
			SessionName = sessionName,
			OnGameStarted = OnGameStarted
		});
	}

	private void OnGameStarted(NetworkRunner obj)
	{
		UpdatePlayerAmount();
		if (!obj.IsSharedModeMasterClient)
			return;
		lobbyInstance = networkRunner.Spawn(lobbyPF);
	}


	[Rpc]
	public void UpdatePlayerAmount()
	{
		_currentPlayersAmount++;

		TirgulUI.Instance.OnPlayerAmountChanged(_currentPlayersAmount, 20);
	}
}
