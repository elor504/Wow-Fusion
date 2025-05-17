using Fusion.Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

/// Document Link: https://doc.photonengine.com/fusion/current/reference/playfab


public static class PlayFabAuthenticator
{
    public static AuthenticationValues AuthValues { get; private set; }
    private static string _playFabPlayerIdCache;
    private const string _playfabTitleID = "1D28E1";

    public static event Action<RegisterPlayFabUserResult> OnSuccessfullyRegistered;

    public static event Action<string> OnFailedToLogin;
    public static event Action<string> OnFailedToRegister;


    #region Login
    public static void AuthenticateWithPlayFab(string email, string pass)
    {
        Debug.Log("PlayFab authenticating using Email...");

        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest()
        {
            Email = email,
            Password = pass,
            TitleId = _playfabTitleID

        }, RequestPhotonToken, OnPlayFabLoginError);
    }
    private static void RequestPhotonToken(LoginResult loginResult)
    {
        Debug.Log("PlayFab authenticated. Requesting photon token...");

        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = loginResult.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonAppSettings.Global.AppSettings.AppIdFusion
        }, AuthenticateWithPhoton, OnPlayFabError);
    }
    private static void OnPlayFabLoginError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
        string message = "";
        switch (obj.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                message = "Account does not exists";
                break;
            case PlayFabErrorCode.InvalidUsernameOrPassword:
                message = "Invalid username or password";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                message = "Invalid email";
                break;
            case PlayFabErrorCode.InvalidPassword:
                message = "Invalid password";
                break;
        }
        OnFailedToLogin.Invoke(message);
    }
    #endregion

    #region Register
    public static void RegisterNewAccount(string email, string pass)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
        {
            Email = email,
            Password = pass,
            RequireBothUsernameAndEmail = false,
            TitleId = _playfabTitleID
        }, RequestRegister, OnPlayFabRegisterError);
    }


    private static void RequestRegister(RegisterPlayFabUserResult result)
    {
        OnSuccessfullyRegistered?.Invoke(result);
        Debug.Log("Successfully registered");
    }

    private static void OnPlayFabRegisterError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
        string message = "";
        switch (obj.Error)
        {
            case PlayFabErrorCode.AccountAlreadyExists:
                message = "Account already exists";
                break;
        }
        OnFailedToRegister.Invoke(message);
    }
    #endregion


    private static void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult authenticationTokenResult)
    {
        Debug.Log("Photon token acquired: " + authenticationTokenResult.PhotonCustomAuthenticationToken + "  Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new AuthenticationValues() { AuthType = CustomAuthenticationType.Custom };

        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache); // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", authenticationTokenResult.PhotonCustomAuthenticationToken);


        //We finally store to use this authentication parameters throughout the entire application.
        AuthValues = customAuth;
    }

    private static void OnPlayFabError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }
    

}