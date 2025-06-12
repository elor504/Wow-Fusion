using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homework
{
    public class SessionListUI : MonoBehaviour
    {
        [SerializeField] private SessionButton buttonPF;
        [SerializeField] private Transform content;
        [Header("References")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private TextMeshProUGUI lobbyNameText;
        [SerializeField] private List<SessionButton> buttonList;
        [SerializeField] private Button createSessionButton;

        private void OnEnable()
        {

            createSessionButton.onClick.AddListener(ClickCreateSessionHandler);
        }
        private void OnDisable()
        {

            createSessionButton.onClick.RemoveListener(ClickCreateSessionHandler);
        }
        public void Init()
        {
            LobbyManager.OnSessionUpdated += UpdateSessions;
        }


        public void ShowPanel(string lobbyID)
        {
            lobbyNameText.text = "Lobby: " + lobbyID;
            gameObject.SetActive(true);
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }

        public void UpdateSessions(NetworkRunner runner, List<SessionInfo> sessionInfos)
        {
            if (NeedToFillButtons(sessionInfos.Count))
            {
                int amountNeeded = sessionInfos.Count - buttonList.Count;

                for (int i = 0; i < amountNeeded; i++)
                {
                    var newButton = Instantiate(buttonPF, content);
                    newButton.HideButton();

                    buttonList.Add(newButton);
                }
            }


            foreach (var button in buttonList)
            {
                button.HideButton();
            }
            int index = 0;
            foreach (var sessionInfo in sessionInfos)
            {
                buttonList[index].ShowButton(sessionInfo);
                index++;
            }
            Debug.Log("updated sessions: " + sessionInfos.Count);
        }

        private bool NeedToFillButtons(int currentSessionAmount)
        {
            return currentSessionAmount > buttonList.Count;
        }
        private void ClickCreateSessionHandler()
        {
            uiManager.ChangeToSessionCreation();
        }
    }
}
