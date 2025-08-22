using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DungeonInstance : NetworkBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _serverRunner;
    private Party dungeonParty;
    private List<PlayerCharacter> _characters = new List<PlayerCharacter>();
    [SerializeField] private List<string> playerNicknames;
    [SerializeField] private List<DragonActor> dragonActors = new List<DragonActor>();
    [SerializeField] private bool isDungeonActive;
    
    public bool IsDungeonActive => isDungeonActive;
    
    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasStateAuthority)
        {
            _serverRunner = Object.Runner;
        }
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        if (Object.HasStateAuthority)
        {
            _serverRunner.RemoveCallbacks(this);
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        foreach (var actor in dragonActors)
        {
          actor.UpdateActor();  
        }
    }

    public void StartDungeon(Party party)
    {
        dungeonParty = party;
        if (Object.HasStateAuthority)
        {
            _serverRunner.AddCallbacks(this);
        }
        _characters = dungeonParty.GetPartyCharacters(_serverRunner);
        foreach (var character in _characters)
        {
            playerNicknames.Add(character.CharacterName);
        }
    }
    public void OnDungeonClosed()
    {
        if (Object.HasStateAuthority)
        {
            _serverRunner.RemoveCallbacks(this);
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) return;
        //TODO: Handle if a player has a dungeon active
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) return;
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
   
    }

    #region  unused
    
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }


    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    #endregion
}
