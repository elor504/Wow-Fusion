using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject panel;
    [Header("Button References")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [Header("Input References")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [Header("Error text")]
    [SerializeField] private TextMeshProUGUI errorText;

    private string _emailInput;
    private string _passwordInput;

    private bool _preventDoubleRequests;



    private void OnEnable()
    {
        errorText.gameObject.SetActive(false);

        loginButton.onClick.AddListener(OnClickLoginButton);
        registerButton.onClick.AddListener(ClickRegisterButtonHandler);

        emailInputField.onValueChanged.AddListener(UpdateEmailInput);
        passwordInputField.onValueChanged.AddListener(UpdatePasswordInput);


        PlayFabAuthenticator.OnFailedToLogin += FailedToLoginHandler;
        PlayFabAuthenticator.OnFailedToRegister += FailedToRegisterHandler;
    }
    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(OnClickLoginButton);
        registerButton.onClick.RemoveListener(ClickRegisterButtonHandler);

        emailInputField.onValueChanged.RemoveListener(UpdateEmailInput);
        passwordInputField.onValueChanged.RemoveListener(UpdatePasswordInput);

        PlayFabAuthenticator.OnFailedToLogin -= FailedToLoginHandler;
        PlayFabAuthenticator.OnFailedToRegister -= FailedToRegisterHandler;
    }


    public void ShowLoginMenu()
    {
        panel.gameObject.SetActive(true);
    }
    public void HideLoginMenu()
    {
        panel.gameObject.SetActive(false);
    }



    public void FailedToLoginHandler(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
        ResetDoubleRequest();
    }
    public void FailedToRegisterHandler(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
        ResetDoubleRequest();
    }

    private void OnClickLoginButton()
    {
        if (_preventDoubleRequests) return;

        _preventDoubleRequests = true;

        PlayFabAuthenticator.AuthenticateWithPlayFab(_emailInput, _passwordInput);
    }
    private void ClickRegisterButtonHandler()
    {
        if (_preventDoubleRequests) return;

        _preventDoubleRequests = true;

        PlayFabAuthenticator.RegisterNewAccount(_emailInput, _passwordInput);
    }

    private void ResetDoubleRequest()
    {
        _preventDoubleRequests = false;
    }

    private void UpdateEmailInput(string input)
    {
        _emailInput = input;
    }
    private void UpdatePasswordInput(string input)
    {
        _passwordInput = input;
    }





}
