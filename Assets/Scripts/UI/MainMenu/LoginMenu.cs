using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
	[Header("Reference")]
	[SerializeField] private MainMenu menu;
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

	private const string PlayerEmailPref = "Email";
	private const string PlayerPasswordPref = "Password";

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
		panel.SetActive(true);
		if (PlayerPrefs.HasKey(PlayerEmailPref))
		{
			emailInputField.text = PlayerPrefs.GetString(PlayerEmailPref);
			_emailInput = PlayerPrefs.GetString(PlayerEmailPref);
		}
		if(PlayerPrefs.HasKey(PlayerPasswordPref)) 
		{
			passwordInputField.text = PlayerPrefs.GetString(_passwordInput);
			_passwordInput = PlayerPrefs.GetString(PlayerPasswordPref);
		}

	}
	public void HideLoginMenu()
	{
		panel.SetActive(false);
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

		PlayerPrefs.SetString(PlayerEmailPref, _emailInput);
		PlayerPrefs.SetString(PlayerPasswordPref, _passwordInput);


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
