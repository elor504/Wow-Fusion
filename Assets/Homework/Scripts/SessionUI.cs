using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Homework01
{
    public class SessionUI : MonoBehaviour
    {
        [SerializeField] private Transform playerInfoParent;
        [SerializeField] private PlayerSessionInfoUI playerInfoPF;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Button returnButton;

        private List<PlayerSessionInfoUI> _playerSessionInfos;


        private int _currentSelectedChar;


        private void OnEnable()
        {
            SessionManager.OnPlayerConnection += UpdatePlayerList;
            returnButton.onClick.AddListener(ReturnToSessionList);
        }

        private void OnDisable()
        {
            SessionManager.OnPlayerConnection -= UpdatePlayerList;
            returnButton.onClick.RemoveListener(ReturnToSessionList);
        }

        public void ShowPanel()
        {
            gameObject.SetActive(true);
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }

        public void UpdatePlayerList(List<PlayerRef> playerRefs)
        {
            foreach (var info in _playerSessionInfos)
            {
                info.HidePlayer();
            }
            int index = 0;
            if(CheckIfNeedToAddNameUI(playerRefs.Count))
            {
                AddNameUI(playerRefs.Count);
            }
            foreach (var player in playerRefs)
            {
                _playerSessionInfos[index].ShowPlayer(player.PlayerId.ToString());
                index++;
            }
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
    }
}