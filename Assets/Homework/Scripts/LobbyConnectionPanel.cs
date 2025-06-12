using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Homework
{
    public class LobbyConnectionPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private TMP_InputField lobbyInputField;
        [SerializeField] private Button enterLobbyButt;
        [SerializeField] private Button enterMainLobbyButt;

        private string _requestedLobbyId;
        private string _nickname;

        private void OnEnable()
        {
            nicknameInputField.onValueChanged.AddListener(InputNicknameHandler);
            lobbyInputField.onValueChanged.AddListener(InputFieldHandler);
            enterLobbyButt?.onClick.AddListener(OnPushEnterLobby);
            enterMainLobbyButt?.onClick.AddListener(PushEnterMainLobbyHandler);
        }

        private void OnDisable()
        {
            nicknameInputField.onValueChanged.RemoveListener(InputNicknameHandler);
            lobbyInputField.onValueChanged.RemoveListener(InputFieldHandler);
            enterLobbyButt?.onClick.RemoveListener(OnPushEnterLobby);
            enterMainLobbyButt?.onClick.RemoveListener(PushEnterMainLobbyHandler);
        }

        public void ShowPanel()
        {
            gameObject.SetActive(true);
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
        private void InputNicknameHandler(string value)
        {
            _nickname = value;
        }

        private void InputFieldHandler(string value)
        {
            _requestedLobbyId = value;
        }
        private void OnPushEnterLobby()
        {
            LobbyManager.EnterLobby.Invoke(_requestedLobbyId,_nickname);
        }
        private void PushEnterMainLobbyHandler()
        {
            LobbyManager.EnterLobby.Invoke(LobbyManager.MainLobbyID, _nickname);
        }
    }
}
