using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Homework01
{
    public class SessionUI : MonoBehaviour
    {
        [SerializeField] private PlayerSessionInfoUI playerInfoPF;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Button returnButton;
        [SerializeField] private List<PlayerSessionInfoUI> playerSessionInfos;


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
            foreach (var info in playerSessionInfos)
            {
                info.HidePlayer();
            }

            int index = 0;
            foreach (var player in playerRefs)
            {
                playerSessionInfos[index].ShowPlayer(player.PlayerId.ToString());
                index++;
            }
        }

        private void ReturnToSessionList()
        {
            uiManager.ChangeToSessionList();
        }
    }
}