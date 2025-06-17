using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Homework
{
    public class SessionUI : MonoBehaviour
    {
        [SerializeField] private Transform playerInfoParent;
        [SerializeField] private PlayerSessionInfoUI playerInfoPF;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Button startButton;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button readyButton;

        private List<PlayerSessionInfoUI> _playerSessionInfos = new List<PlayerSessionInfoUI>();
        private int _currentSelectedChar;

        public static Action<Dictionary<PlayerRef, bool>> UpdateSessionInfo;

        private void OnEnable()
        {
            if (!LobbyManager.Instance)
                return;

            if (LobbyManager.Instance.NetworkRunner.IsSharedModeMasterClient)
            {
                startButton.gameObject.SetActive(true);
                readyButton.gameObject.SetActive(false);
                startButton.onClick.AddListener(StartGame);
            }
            else
            {
                startButton.gameObject.SetActive(false);
                readyButton.gameObject.SetActive(true);
            }
            LobbyManager.OnPlayerConnection += UpdatePlayerList;
            returnButton.onClick.AddListener(ReturnToSessionList);
            readyButton.onClick.AddListener(ToggleReady);
            UpdateSessionInfo += UpdatePlayerReadyList;
        }

        private void OnDisable()
        {
            LobbyManager.OnPlayerConnection -= UpdatePlayerList;
            returnButton.onClick.RemoveListener(ReturnToSessionList);
            readyButton.onClick.RemoveListener(ToggleReady);
            UpdateSessionInfo -= UpdatePlayerReadyList;
            startButton.onClick.RemoveAllListeners();
        }

        public void ShowPanel()
        {
            gameObject.SetActive(true);
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }


        public void UpdatePlayerList(Dictionary<PlayerRef, string> playerRefs)
        {
            foreach (var info in _playerSessionInfos)
            {
                info.HidePlayer();
            }
            int index = 0;
            if (CheckIfNeedToAddNameUI(playerRefs.Count))
            {
                AddNameUI(playerRefs.Count);
            }
            foreach (var player in playerRefs)
            {
                _playerSessionInfos[index].ShowPlayer(player.Key, player.Value);
                index++;
            }
        }
        public void UpdateReadyCheck(Dictionary<PlayerRef, bool> readyCheck)
        {
            UpdateSessionInfo.Invoke(readyCheck);
        }
        private void UpdatePlayerReadyList(Dictionary<PlayerRef, bool> readyCheck)
        {
            foreach (var ready in readyCheck)
            {
                _playerSessionInfos.Find(p => p.GetPlayerRef == ready.Key).SetReady(ready.Value);
            }
        }
        private void StartGame()
        {
            //TODO: check if has enough ready votes
            LobbyManager.Instance.StartGame();
        }
        private bool CheckIfNeedToAddNameUI(int playerRefCount)
        {
            return _playerSessionInfos.Count < playerRefCount;
        }
        private void AddNameUI(int playerRefCount)
        {
            int amountMissing = playerRefCount - _playerSessionInfos.Count;
            for (int i = 0; i < amountMissing; i++)
            {
                PlayerSessionInfoUI newSessionInfo = Instantiate(playerInfoPF, playerInfoParent);
                newSessionInfo.HidePlayer();
                _playerSessionInfos.Add(newSessionInfo);
            }
        }
        private void ReturnToSessionList()
        {
            uiManager.ChangeToSessionList();
        }
        private void ToggleReady()
        {
            LobbyManager.Instance.PlayerListInstance.LobbyCheck.RPCToggleReady();
        }
    }
}