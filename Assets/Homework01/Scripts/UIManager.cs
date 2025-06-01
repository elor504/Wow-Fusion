using UnityEngine;
using WebSocketSharp;

namespace Homework01
{
    public class UIManager : MonoBehaviour
    {
        ///Task for chen:
        ///Find easter eggs in the homework 01 scripts (could be also in assets :D)
        ///If you find all of them i will give you a gift that you want
        [Header("References")]
        [SerializeField] private LobbyConnectionPanel lobbySelectionPanel;
        [SerializeField] private SessionListUI sessionListPanel;
        [SerializeField] private SessionCreationPanel sessionCreationPanel;
        [SerializeField] private SessionUI sessionUI;
        [Header("Panels")]
        [SerializeField] private GameObject loadingLobbyPanel;

        private UIState _currentState = UIState.Uninitialized;

        private string _lobbyID;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnEnable()
        {
            SessionManager.OnStartLoadingLobby += ChangeToLoadingState;
            SessionManager.OnFinishedLoadingLobby += ChangeToSessionList;

            SessionManager.OnGameStarted += ChangeToSessionState;
        }
        private void OnDisable()
        {
            SessionManager.OnStartLoadingLobby -= ChangeToLoadingState;
            SessionManager.OnFinishedLoadingLobby -= ChangeToSessionList;

            SessionManager.OnGameStarted -= ChangeToSessionState;
        }

        public void Init()
        {
            lobbySelectionPanel.HidePanel();
            loadingLobbyPanel.SetActive(false);
            sessionListPanel.HidePanel();
            sessionUI.HidePanel();
            sessionCreationPanel.HidePanel();

            sessionListPanel.Init();
        }

        private void ChangeState(UIState state)
        {
            Debug.Log($"Change state to: {state}");
            ExitState();
            Debug.Log("Exit");
            _currentState = state;
            EnterState();
            Debug.Log("Enter");
        }

        private void ExitState()
        {
            switch (_currentState)
            {
                case UIState.Uninitialized:
                    Init();
                    break;
                case UIState.LobbySelection:
                    lobbySelectionPanel.HidePanel();
                    break;
                case UIState.connectingToLobby:
                    loadingLobbyPanel.SetActive(false);
                    break;
                case UIState.SessionCreation:
                    sessionCreationPanel.HidePanel();
                    break;
                case UIState.SessionList:
                    sessionListPanel.HidePanel();
                    break;
                case UIState.Session:
                    sessionUI.HidePanel();
                    break;
            }
        }
        private void EnterState()
        {
            switch (_currentState)
            {
                case UIState.LobbySelection:
                    lobbySelectionPanel.ShowPanel();
                    break;
                case UIState.connectingToLobby:
                    loadingLobbyPanel.SetActive(true);
                    break;
                case UIState.SessionCreation:
                    sessionCreationPanel.ShowPanel();
                    break;
                case UIState.SessionList:
                    sessionListPanel.ShowPanel(_lobbyID);
                    break;
                case UIState.Session:
                    sessionUI.ShowPanel();
                    break;
            }
        }

        public void ChangeToLobbySelection()
        {
            ChangeState(UIState.LobbySelection);
        }
        private void ChangeToLoadingState()
        {
            ChangeState(UIState.connectingToLobby);
        }
        public void ChangeToSessionList(string lobbyID = "")
        {
            if (!lobbyID.IsNullOrEmpty())
                _lobbyID = lobbyID;
            ChangeState(UIState.SessionList);
        }

        public void ChangeToSessionCreation()
        {
            ChangeState(UIState.SessionCreation);
        }

        private void ChangeToSessionState()
        {
            ChangeState(UIState.Session);
        }

        private enum UIState
        {
            Uninitialized = 0,
            LobbySelection = 1,
            connectingToLobby = 2,
            SessionList = 3,
            SessionCreation = 4,
            Session = 5,
            EasterEgg = 69
        }
    }
}