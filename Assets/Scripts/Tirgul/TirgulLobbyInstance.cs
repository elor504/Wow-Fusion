using Fusion;
using UnityEngine;

public class TirgulLobbyInstance : NetworkBehaviour
{

	public override void Spawned()
	{
		base.Spawned();
		TirgulRoomManager.Instance.lobbyInstance = this;

	}
}
