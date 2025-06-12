using Fusion;
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
        [SerializeField] private Button returnButton;
        [SerializeField] private List<Button> characterButtons;
        private List<PlayerSessionInfoUI> _playerSessionInfos = new List<PlayerSessionInfoUI>();


        private int _currentSelectedChar;


        private void OnEnable()
        {
            LobbyManager.OnPlayerConnection += UpdatePlayerList;
            returnButton.onClick.AddListener(ReturnToSessionList);
            CharacterSelectionManager.OnSelectedCharacter += UpdateButtons;
        }

        private void OnDisable()
        {
            LobbyManager.OnPlayerConnection -= UpdatePlayerList;
            returnButton.onClick.RemoveListener(ReturnToSessionList);
            CharacterSelectionManager.OnSelectedCharacter -= UpdateButtons;
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
        public void ClickCharacterButtonHandler(int index)
        {
            Debug.Log($"On click character: {index}");
            LobbyManager.Instance.GetCharacterSelectionManager.RPCSetCharacterSelection(index);
            UpdateButtons();
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


      

        private void UpdateButtons()
        {
            var characters = LobbyManager.Instance.GetCharacterSelectionManager.GetCharacterSelectionList;
            for (int i = 0; i < characters.Count; i++)
            {
                characterButtons[i].interactable = !characters[i].IsSelected;
            }
        }
    }
}