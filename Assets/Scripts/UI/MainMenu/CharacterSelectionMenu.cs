
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionMenu : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button createCharacterButton;
    [SerializeField] private List<CharacterButton> characterButtonsGO;

    private List<CharacterData> _charactersData = new List<CharacterData>();


    private void OnEnable()
    {
        PlayFabCharacterCreator.OnGetCharacterList += GetCharacterListHander;
        createCharacterButton.onClick.AddListener(mainMenu.ShowCharacterCreation);
    }
    private void OnDisable()
    {
        PlayFabCharacterCreator.OnGetCharacterList -= GetCharacterListHander;
        createCharacterButton.onClick.RemoveListener(mainMenu.ShowCharacterCreation);
    }

    public void LoadCharacterDatas()
    {
        if (PlayFabCharacterCreator.TryToGetCharacterDatas(PlayFabAuthenticator.GetPlayFabPlayerID, out var characterDatas))
        {

        }
    }

    public void ShowPanel()
    {
        LoadCharacterDatas();
        panel.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
    }

    private void GetCharacterData(string id)
    {
        GetCharacterDataRequest request = new GetCharacterDataRequest()
        {
            CharacterId = id
        };
        PlayFabClientAPI.GetCharacterData(request, GetCharacterDataResult, FailedToLoad);
    }
    private void GetCharacterListHander(ListUsersCharactersResult result)
    {
        foreach (var button in characterButtonsGO)
        {
            button.HideButton();
        }

        foreach (var character in result.Characters)
        {
            GetCharacterData(character.CharacterId);
        }

    }



    private void GetCharacterDataResult(GetCharacterDataResult result)
    {
        string json = result.Data["CharacterData"].Value;
        CharacterData charData = JsonUtility.FromJson<CharacterData>(json);

        _charactersData.Add(charData);

        int index = _charactersData.IndexOf(charData);
        characterButtonsGO[index].ShowButton(charData.CharacterName, charData);
    }
    private void FailedToLoad(PlayFabError error)
    {
        Debug.Log($"[Character Selection Menu] failed to load character data {error}");
    }


}
