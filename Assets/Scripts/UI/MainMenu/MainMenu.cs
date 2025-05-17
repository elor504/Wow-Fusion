using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private LoginMenu loginMenu;
    [SerializeField] private CharacterCreationMenu characterCreationMenu;
    [SerializeField] private CharacterSelectionMenu characterSelectionMenu;

    private MainMenuState _currentState = MainMenuState.Nothing;


    private void Awake()
    {
        loginMenu.HideLoginMenu();
        characterCreationMenu.HidePanel();
        characterSelectionMenu.HidePanel();

        ShowLoginMenu();
    }
    private void OnEnable()
    {
        PlayFabAuthenticator.OnSuccessfullyLoggedIn += OnLogged;
    }
    private void OnDisable()
    {
        PlayFabAuthenticator.OnSuccessfullyLoggedIn -= OnLogged;
    }

    public void ShowLoginMenu()
    {
        ChangeState(MainMenuState.Login);
    }

    public void OnLogged(GetPhotonAuthenticationTokenResult result)
    {
        ShowCharacterSelection();
    }
    public void ShowCharacterCreation()
    {
        ChangeState(MainMenuState.CharacterCreation);
    }
    public void ShowCharacterSelection()
    {
        ChangeState(MainMenuState.CharacterSelection);
    }

    public void ChangeState(MainMenuState state)
    {
        ExitState();
        _currentState = state;
        EnterState();
    }

    private void EnterState()
    {
        switch (_currentState)
        {
            case MainMenuState.Login:
                loginMenu.ShowLoginMenu();
                break;
            case MainMenuState.Register:
                break;
            case MainMenuState.CharacterCreation:
                characterCreationMenu.ShowPanel();
                break;
            case MainMenuState.CharacterSelection:
                characterSelectionMenu.ShowPanel();
                break;
        }
    }
    private void ExitState()
    {
        switch (_currentState)
        {
            case MainMenuState.Login:
                loginMenu.HideLoginMenu();
                break;
            case MainMenuState.Register:
                break;
            case MainMenuState.CharacterCreation:
                characterCreationMenu.HidePanel();
                break;
            case MainMenuState.CharacterSelection:
                characterSelectionMenu.HidePanel();
                break;
        }
    }  
}
public enum MainMenuState
{
    Nothing,
    Login,
    Register,
    CharacterCreation,
    CharacterSelection,
}