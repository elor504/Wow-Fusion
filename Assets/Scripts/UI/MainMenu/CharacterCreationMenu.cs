using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationMenu : MonoBehaviour
{
	[Header("Reference")]
	[SerializeField] private MainMenu menu;
	[SerializeField] private GameObject panel;
	[Header("Scriptable objects data")]
	[SerializeField] private List<BaseClassData> classesData;
	[SerializeField] private CharacterVisualSO visualSO;
	[SerializeField] private List<EquipmentDataSO> equipmentsData;

	[Header("Inputfield Reference")]
	[SerializeField] private TMP_InputField characterNameInput;

	[Header("Buttons References")]
	[SerializeField] private Button createCharacterButton;

	[Header("Class buttons")]
	[SerializeField] private Button mageButton;
	[SerializeField] private Button warriorButton;
	[SerializeField] private Button rangerButton;

	[Header("Class info ui")]
	[SerializeField] private TextMeshProUGUI className;
	[SerializeField] private TextMeshProUGUI classDescription;


	[Header("Character visual")]
	[SerializeField] private CharacterHairMeshes characterHairRenderer;
	[SerializeField] private PlayerEquipment equipmentVisual;
	///Character visual customization?

	private HairColorType hairColorType;


	private bool _spamPreventation;
	private string _characterName;
	private ClassType _currentSelectedClass;


	[Header("Testing")]
	public HairColorType startHairToTest = HairColorType.Ginger;


	private void OnEnable()
	{
		characterNameInput.onValueChanged.AddListener(UpdateCharacterNameInput);

		createCharacterButton.onClick.AddListener(OnClickCreateButton);

		///Will be a dream to make it more flexible but its good for now :D
		mageButton.onClick.AddListener(ClickMageButtonHandler);


		PlayFabCharacterCreator.OnGrantedCharacter += GrantedCharacterHandler;
		PlayFabCharacterCreator.OnUpdatedCharacter += CharacterUpdateHandler;
	}
	private void OnDisable()
	{
		characterNameInput.onValueChanged.RemoveListener(UpdateCharacterNameInput);

		createCharacterButton.onClick.RemoveListener(OnClickCreateButton);

		mageButton.onClick.RemoveListener(ClickMageButtonHandler);

		PlayFabCharacterCreator.OnGrantedCharacter -= GrantedCharacterHandler;
		PlayFabCharacterCreator.OnUpdatedCharacter -= CharacterUpdateHandler;
	}

	public void ShowPanel()
	{
		SelectHairColor(startHairToTest);
		characterNameInput.text = "";
		_characterName = "";
		HandleIfCanClickCreateButton();

		panel.SetActive(true);
	}
	public void HidePanel()
	{
		panel.SetActive(false);
	}


	public void OnSelectedClassButton(ClassType selectedClass)
	{
		if (TryGetClassDataByClassType(selectedClass, out var data))
		{
			_currentSelectedClass = selectedClass;
			className.text = data.ClassName;
			classDescription.text = data.ClassDescription;

			var equipments = GetClassStartEquipment(_currentSelectedClass);
			int enumLength = Enum.GetNames(typeof(EquipmentType)).Length;
			for (int i = 0; i < enumLength; i++)
			{
				var type = (EquipmentType)i;
				EquipableItemData equipable = equipments.GetEquipableDataByType(type);
				if (equipable != null)
				{
					Mesh[] meshes = GetEquipmentMesh(equipable.ItemName);
					if (meshes != null)
					{
						equipmentVisual.UpdateVisual(type, meshes);
					}
					else
					{
						Debug.Log($"Meshes do not existed at equipment: {equipable.ItemName}");
					}
				}
			}
		}
		else
		{
			className.text = "";
			classDescription.text = "";
		}
	}


	private void SelectHairColor(HairColorType selectedHairColor)
	{
		hairColorType = selectedHairColor;
		characterHairRenderer.ChangeHairMeshesColor(visualSO.GetHairColorByType(hairColorType));
	}

	private void ClickMageButtonHandler()
	{
		OnSelectedClassButton(ClassType.Mage);
	}
	private bool TryGetClassDataByClassType(ClassType type, out BaseClassData data)
	{
		data = classesData.Find(c => c.GetClassData.GetClassID == type);
		if (!data)
		{
			Debug.LogError($"[Character Creation Menu] Failed to load class data: {type.ToString()}");
		}
		return data;
	}


	private void HandleIfCanClickCreateButton()
	{
		if (string.IsNullOrEmpty(_characterName) || _characterName.Length < 4)
		{
			createCharacterButton.interactable = false;
		}
		else
		{
			createCharacterButton.interactable = true;
		}
	}

	private void OnClickCreateButton()
	{
		//if (_spamPreventation) return;

		//_spamPreventation = true;

		PlayFabCharacterCreator.RequestCharacterCreation(_characterName, _currentSelectedClass.ToString());

	}
	private void UpdateCharacterNameInput(string input)
	{
		_characterName = input;
		HandleIfCanClickCreateButton();
	}

	private StatContainer GetClassBasicStat(ClassType type)
	{
		return classesData.Find(c => c.GetClassData.GetClassID == type).ClassBaseStats;
	}

	private CharacterEquipmentData GetClassStartEquipment(ClassType type)
	{
		var equipments = classesData.Find(c => c.GetClassData.GetClassID == type).StartingEquipment;

		var equipmentData = new CharacterEquipmentData();
		foreach (var equipment in equipments)
		{
			equipmentData.TryToEquip(equipment.GetEquipableItem(), out _);
		}
		return equipmentData;
	}
	private Mesh[] GetEquipmentMesh(string id)
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
	private void GrantedCharacterHandler(GrantCharacterToUserResult result)
	{
		CharacterVisualData visualData = new CharacterVisualData(startHairToTest);
		StatContainer baseClassStat = GetClassBasicStat(_currentSelectedClass);
		CharacterEquipmentData equipmentData = GetClassStartEquipment(_currentSelectedClass);
		CharacterData newCharData = new CharacterData(_characterName, 1, _currentSelectedClass, baseClassStat, visualData, equipmentData);

		PlayFabCharacterCreator.UpdateCharacterData(result.CharacterId, newCharData);
	}
	private void CharacterUpdateHandler(UpdateCharacterDataResult result)
	{
		menu.ChangeState(MainMenuState.CharacterSelection);
	}
}

















































///Easter Egg 1#