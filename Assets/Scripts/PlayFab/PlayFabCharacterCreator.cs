using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayFabCharacterCreator
{

    public static event Action<GrantCharacterToUserResult> OnGrantedCharacter;
    public static event Action<UpdateCharacterDataResult> OnUpdatedCharacter;
    public static event Action<ListUsersCharactersResult> OnGetCharacterList;

    public static event Action<PlayFabError> OnFailedToGrantCharacter;

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
    public static void RequestCharacterCreation(string characterName,string className)
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "Main",
            ItemId = className,
            VirtualCurrency = "PC", // your currency code
            Price = 0 // or whatever price you set in catalog
        }, result =>
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest()
            {
                CharacterName = characterName,
                ItemId = className

            }, SuccessfullyGrantedCharacter, OnPlayFabError);

        }, error =>
        {
            Debug.LogError("Purchase failed: " + error.GenerateErrorReport());
        });

      
    }

    private static void SuccessfullyGrantedCharacter(GrantCharacterToUserResult result)
    {
        OnGrantedCharacter?.Invoke(result);
    }
    #endregion

    public static bool TryToGetCharacterDatas(string playFabID, out List<CharacterData> characterDatas)
    {
        characterDatas = new List<CharacterData>();
        ListUsersCharactersRequest listUsersCharactersRequest = new ListUsersCharactersRequest()
        {
            PlayFabId = playFabID
        };
        PlayFabClientAPI.GetAllUsersCharacters(listUsersCharactersRequest, GetCharacterDataList, OnPlayFabError);


        if (characterDatas.Count == 0)
        {
            return false;
        }


        return true;
    }
    private static void GetCharacterDataList(ListUsersCharactersResult result)
    {
        OnGetCharacterList?.Invoke(result);
    }


    private static void OnPlayFabError(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }
}
