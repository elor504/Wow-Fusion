using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LobbyRequestUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyInputField;
    [SerializeField] private Button enterLobbyButt;

    private string _requestedLobbyId;

    private void OnEnable()
    {
        lobbyInputField.onValueChanged.AddListener(InputFieldHandler);
        enterLobbyButt.onClick.AddListener(OnPushEnterLobby);
    }
    private void OnDisable()
    {
        lobbyInputField.onValueChanged.RemoveListener(InputFieldHandler);
        enterLobbyButt.onClick.RemoveListener(OnPushEnterLobby);
    }

    private void InputFieldHandler(string value)
    {
        _requestedLobbyId = value;
    }

    private void OnPushEnterLobby()
    {

    }

}
