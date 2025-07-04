using Fusion;
using Homework;
using TMPro;
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
        sessionButton.interactable = CanBeInteracted(sessionInfo);

        gameObject.SetActive(true);
    }
   
    public void HideButton()
    {
        sessionButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
    private bool CanBeInteracted(SessionInfo sessionInfo)
    {
        if (sessionInfo.PlayerCount == sessionInfo.MaxPlayers)
        {
            return false;
        }

        return true;
    }
    private void OnClickButton()
    {
        LobbyManager.EnterSession(_sessionInfo.Name, _sessionInfo.MaxPlayers);
    }
}
