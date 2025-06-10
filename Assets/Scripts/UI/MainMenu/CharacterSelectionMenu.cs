
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
	[SerializeField] private List<CharacterButton> characterButtonsGO;
	[Header("Visual")]
	[SerializeField] private GameObject characterVisualGO;
	[SerializeField] private CharacterHairMeshes characterVisual;
	[SerializeField] private CharacterVisualSO visualSO;
	private List<CharacterData> _charactersData = new List<CharacterData>();
	[Header("Equipment")]
	[SerializeField] private PlayerEquipment characterEquipment;
	[SerializeField] private List<EquipmentDataSO> equipmentsData;

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
	public void ShowCharacter(int index)
	{
		var characterData = _charactersData[index];
		var characterVisualData = characterData.CharacterVisualData;
		var characterEquipmentData = characterData.CharacterEquipmentData;
		Color hairColor = visualSO.GetHairColorByType(characterVisualData.HairColor);

		characterVisual.ChangeHairMeshesColor(hairColor);

		int enumLength = Enum.GetNames(typeof(EquipmentType)).Length;
		for (int i = 0; i < enumLength; i++)
		{
			var type = (EquipmentType)i;
			EquipableItemData equipmentData = characterEquipmentData.GetEquipableDataByType(type);
			if (!equipmentData.IsEmpty())
				characterEquipment.UpdateVisual(type, GetEquipmentMeshes(equipmentData.ItemName));
		}


		if (!characterVisualGO.activeInHierarchy)
			characterVisualGO.SetActive(true);
	}
	public void ShowPanel()
	{
		LoadCharacterDatas();
		panel.SetActive(true);
		characterVisualGO.SetActive(false);
	}
	public void HidePanel()
	{
		panel.SetActive(false);
		characterVisualGO.SetActive(false);
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
	private Mesh[] GetEquipmentMeshes(string id)
	{
		foreach (var equipment in equipmentsData)
		{
			if (equipment.EquipmentName == id)
			{
				return equipment.EquipmentMeshes;
			}
		}

		return null;
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


}
