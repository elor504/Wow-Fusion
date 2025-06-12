using Fusion;
using Homework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SessionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sessionNameText;
    [SerializeField] private TextMeshProUGUI sessionPlayerCountText;

    [SerializeField] private Button sessionButton;


    private SessionInfo _sessionInfo;

    public void ShowButton(SessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;

        sessionNameText.text = _sessionInfo.Name;
        sessionPlayerCountText.text = _sessionInfo.PlayerCount + "/" + _sessionInfo.MaxPlayers;
        sessionButton.onClick.AddListener(OnClickButton);
        gameObject.SetActive(true);
    }

    public void HideButton()
    {
        sessionButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    private void OnClickButton()
    {
        LobbyManager.EnterSession(_sessionInfo.Name,_sessionInfo.MaxPlayers);
    }

}
