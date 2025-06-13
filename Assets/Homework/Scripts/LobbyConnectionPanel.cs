using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            nicknameInputField.text = string.Empty;
            _nickname = string.Empty;
            _requestedLobbyId = string.Empty;
            enterLobbyButt.interactable = false;
            enterMainLobbyButt.interactable = false;
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
        private void InputNicknameHandler(string value)
        {
            _nickname = value;
            LobbyManager.Nickname = _nickname;
            bool isNameValid = _nickname.Length >= 4;
            enterMainLobbyButt.interactable = isNameValid;
            InputFieldHandler(_requestedLobbyId);
        }

        private void InputFieldHandler(string value)
        {
            _requestedLobbyId = value;
            bool isValid = _nickname.Length >= 4 && _requestedLobbyId.Length >= 4;
            enterLobbyButt.interactable = isValid;
        }
        private void OnPushEnterLobby()
        {
            LobbyManager.EnterLobby.Invoke(_requestedLobbyId, _nickname);
        }
        private void PushEnterMainLobbyHandler()
        {
            LobbyManager.EnterLobby.Invoke(LobbyManager.MainLobbyID, _nickname);
        }
    }
}
