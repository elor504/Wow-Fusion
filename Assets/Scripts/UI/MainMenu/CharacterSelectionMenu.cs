
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionMenu : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button createCharacterButton;
    [SerializeField] private Button playerGameButton;
    [SerializeField] private GameObject characterLoading;
    [SerializeField] private GameObject characterList;
    [SerializeField] private List<CharacterButton> characterButtonsGO;


    [Header("Visual")]
    [SerializeField] private GameObject characterVisualGO;
    [SerializeField] private CharacterHairMeshes characterVisual;
    [SerializeField] private CharacterVisualSO visualSO;
    private List<CharacterData> _charactersData = new List<CharacterData>();
    [Header("Equipment")]
    [SerializeField] private CharacterVis characterEquipment;
    [SerializeField] private List<EquipmentDataSO> equipmentsData;


    private CharacterData _currentSelectedCharacter;
    private void Awake()
    {
        characterEquipment.Init();
    }
    private void OnEnable()
    {
        createCharacterButton.onClick.AddListener(mainMenu.ShowCharacterCreation);
    }
    private void OnDisable()
    {
        createCharacterButton.onClick.RemoveListener(mainMenu.ShowCharacterCreation);
    }

    public void LoadCharacterDatas()
    {
        PlayFabCharacterCreator.TryToGetCharacterDatas(PlayFabAuthenticator.GetPlayFabPlayerID, GetCharacterListHander);
    }
    public void ShowCharacter(int index)
    {
        createCharacterButton.interactable = false;
        _currentSelectedCharacter = _charactersData[index];
        var characterVisualData = _currentSelectedCharacter.CharacterVisualData;
        var characterEquipmentData = _currentSelectedCharacter.CharacterEquipmentData;
        Color hairColor = visualSO.GetHairColorByType(characterVisualData.HairColor);
        characterVisual.ChangeHairMeshesColor(hairColor);

        int enumLength = Enum.GetNames(typeof(EquipmentType)).Length;
        for (int i = 0; i < enumLength; i++)
        {
            var type = (EquipmentType)i;
            EquipableItemData equipmentData = characterEquipmentData.GetEquipableDataByType(type);
            if (!equipmentData.IsEmpty())
                characterEquipment.UpdateVisual(type, equipmentData);
        }

        if (!characterVisualGO.activeInHierarchy)
            characterVisualGO.SetActive(true);

        playerGameButton.interactable = true;
    }
    public void ShowPanel()
    {
        characterLoading.SetActive(true);
        characterList.SetActive(false);
        LoadCharacterDatas();
        panel.SetActive(true);
        characterVisualGO.SetActive(false);
        playerGameButton.onClick.AddListener(PlayAsCharacter);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
        characterVisualGO.SetActive(false);
        playerGameButton.onClick.RemoveListener(PlayAsCharacter);
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
        characterLoading.SetActive(false);
        characterList.SetActive(true);
        createCharacterButton.interactable = result.Characters.Count < 3;
    }


    private void PlayAsCharacter()
    {
        RuntimeSessionManager.FusionManager.SetSelectedCharacterData(_currentSelectedCharacter);
        RuntimeSessionManager.FusionManager.ConnectToMainCity();
    }

    private void GetCharacterDataResult(GetCharacterDataResult result)
    {
        string json = result.Data["CharacterData"].Value;
        CharacterData charData = JsonUtility.FromJson<CharacterData>(json);

        _charactersData.Add(charData);

        int index = _charactersData.IndexOf(charData);
        characterButtonsGO[index].ShowButton(charData.CharacterName, charData);
        characterButtonsGO[index].GetButton.onClick.RemoveAllListeners();
        characterButtonsGO[index].GetButton.onClick.AddListener(() => ShowCharacter(index));
    }
    private void FailedToLoad(PlayFabError error)
    {
        Debug.Log($"[Character Selection Menu] failed to load character data {error}");
    }

    private string SerializeCharacterData()
    {
        return JsonUtility.ToJson(_charactersData);
    }

}
