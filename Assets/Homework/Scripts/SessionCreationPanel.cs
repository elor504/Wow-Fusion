using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
namespace Homework
{
    public class SessionCreationPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private TMP_InputField sessionNameInput;
        [SerializeField] private TMP_InputField maxPlayerInput;
        [SerializeField] private Button openSessionButton;
        [SerializeField] private Button returnButton;

        private string _sessionName = string.Empty;
        private int _currentPlayerAmount;

        private const int minLobbyNameLength = 4;
        private const int maxPlayers = 20;
        private const int minPlayers = 3;


        private void OnEnable()
        {
            sessionNameInput.onValueChanged.AddListener(SessionNameInputHandler);

            _currentPlayerAmount = minPlayers;
            maxPlayerInput.text = minPlayers.ToString();
            maxPlayerInput.onValueChanged.AddListener(MaxPlayerInputHandler);

            openSessionButton.onClick.AddListener(OpenSessionButton);
            returnButton.onClick.AddListener(ReturnToSessionList);
        }

        private void OnDisable()
        {
            sessionNameInput.onValueChanged.RemoveListener(SessionNameInputHandler);
            maxPlayerInput.onValueChanged.RemoveListener(MaxPlayerInputHandler);

            openSessionButton.onClick.RemoveListener(OpenSessionButton);
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
        private void SessionNameInputHandler(string lobbyName)
        {
            _sessionName = lobbyName;
        }

        private void OpenSessionButton()
        {
            if (_sessionName.Length < minLobbyNameLength)
            {
                Debug.LogError("session name is too short (needs to be 4 or higher");
                return;
            }
            LobbyManager.EnterSession(_sessionName,_currentPlayerAmount);
        }

        private void MaxPlayerInputHandler(string input)
        {
            if (input.IsNullOrEmpty())
                return;

            int amount = int.Parse(input);
            if (amount > maxPlayers)
            {
                amount = maxPlayers;
            }
            else if (amount < minPlayers)
            {
                amount = minPlayers;
            }
            _currentPlayerAmount = amount;
            maxPlayerInput.text = _currentPlayerAmount.ToString();
        }


        private void ReturnToSessionList()
        {
            uiManager.ChangeToSessionList();
        }

    }
}