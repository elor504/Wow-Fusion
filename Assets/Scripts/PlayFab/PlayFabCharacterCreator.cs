using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayFabCharacterCreator
{

    public static event Action<GrantCharacterToUserResult> OnGrantedCharacter;
    public static event Action<UpdateCharacterDataResult> OnUpdatedCharacter;

    #region Update Character Data
    public static void UpdateCharacterData(string characterName, CharacterData data)
    {
        Dictionary<string, string> characterData = new Dictionary<string, string>
        {
            { "CharacterData", JsonUtility.ToJson(data) }
        };

        UpdateCharacterDataRequest updateDataRequest = new UpdateCharacterDataRequest
        {
            CharacterId = characterName,
            Data = characterData
        };

        PlayFabClientAPI.UpdateCharacterData(updateDataRequest, SuccessfullyUpdatedData, OnPlayFabError);
    }
    private static void SuccessfullyUpdatedData(UpdateCharacterDataResult result)
    {
        OnUpdatedCharacter?.Invoke(result);
        Debug.Log("Updated successfully a character");
    }


    #endregion

    #region Character Creation
    public static void RequestCharacterCreation(string characterName)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest()
        {
            CharacterName = characterName,
            CustomTags = null
        }, SuccessfullyGrantedCharacter, OnPlayFabError);
    }

    private static void SuccessfullyGrantedCharacter(GrantCharacterToUserResult result)
    {
        OnGrantedCharacter?.Invoke(result);
    }
    #endregion
    private static void OnPlayFabError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }
}
