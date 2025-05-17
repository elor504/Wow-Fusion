using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private LoginMenu loginMenu;

    private MainMenuState _currentState = MainMenuState.Nothing;


    private void Awake()
    {
        ShowLoginMenu();
    }
    public void ShowLoginMenu()
    {
        ChangeState(MainMenuState.Login);
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
            case MainMenuState.CharacterSelection:
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
            case MainMenuState.CharacterSelection:
                break;
        }
    }  
}
public enum MainMenuState
{
    Nothing,
    Login,
    Register,
    CharacterSelection
}