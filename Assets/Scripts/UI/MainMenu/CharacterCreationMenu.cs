using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationMenu : MonoBehaviour
{
    private enum CreationError
    {
        Available,
        NameExists,
        NameShort
    }



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
    [SerializeField] private Button checkCharacterNameValidationButton;

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

    [Header("Error")]
    [SerializeField] private TextMeshProUGUI errorText;


    private HairColorType hairColorType;

    private bool _isNameTaken;
    private bool _createCharacterInProgress;
    private string _characterName;
    private ClassType _currentSelectedClass;

    [Header("Testing")]
    public HairColorType startHairToTest = HairColorType.Ginger;

    private const int Name_Length_Min = 5;



    private void OnEnable()
    {
        characterNameInput.onValueChanged.AddListener(UpdateCharacterNameInput);

        createCharacterButton.onClick.AddListener(OnClickCreateButton);
        checkCharacterNameValidationButton.onClick.AddListener(CheckNameValidation);
        ///Will be a dream to make it more flexible but its good for now :D
        mageButton.onClick.AddListener(ClickMageButtonHandler);
        warriorButton.onClick.AddListener(ClickWarriorButtonHandler);

        PlayFabCharacterCreator.OnGrantedCharacter += GrantedCharacterHandler;
        PlayFabCharacterCreator.OnUpdatedCharacter += CharacterUpdateHandler;
    }
    private void OnDisable()
    {
        characterNameInput.onValueChanged.RemoveListener(UpdateCharacterNameInput);

        createCharacterButton.onClick.RemoveListener(OnClickCreateButton);
        checkCharacterNameValidationButton.onClick.RemoveListener(CheckNameValidation);

        mageButton.onClick.RemoveListener(ClickMageButtonHandler);
        warriorButton.onClick.RemoveListener(ClickWarriorButtonHandler);

        PlayFabCharacterCreator.OnGrantedCharacter -= GrantedCharacterHandler;
        PlayFabCharacterCreator.OnUpdatedCharacter -= CharacterUpdateHandler;
    }

    public void ShowPanel()
    {
        SelectHairColor(startHairToTest);
        characterNameInput.text = "";
        _characterName = "";
        HandleCreationButton(CreationError.NameShort, false);
        SetErrorMessageCharacterCreation(CreationError.Available);
        ClickMageButtonHandler();
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
            Debug.Log($"[CharacterCreationMenu] There is not class data available for class {selectedClass}");
            className.text = "";
            classDescription.text = "";
        }
        HandleClassButtonInteraction();
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
    private void ClickWarriorButtonHandler()
    {
        OnSelectedClassButton(ClassType.Warrior);
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

    private void OnClickCreateButton()
    {
        if (_createCharacterInProgress) return;
        _createCharacterInProgress = true;
        createCharacterButton.interactable = false;
        PlayFabCharacterCreator.RequestCharacterCreation(_characterName, _currentSelectedClass.ToString());
    }

    private void UpdateCharacterNameInput(string input)
    {
        _characterName = input;
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

    private void HandleCreationButton(CreationError errorType, bool showMessage = true)
    {

        switch (errorType)
        {
            case CreationError.Available:
                createCharacterButton.interactable = true;
                break;
            case CreationError.NameExists:
                createCharacterButton.interactable = !_isNameTaken;
                break;
            case CreationError.NameShort:
                createCharacterButton.interactable = _characterName.Length >= Name_Length_Min;
                break;
        }

        if (showMessage)
            SetErrorMessageCharacterCreation(errorType);
    }
    private void SetErrorMessageCharacterCreation(CreationError errorType)
    {
        string message = string.Empty;
        switch (errorType)
        {
            case CreationError.NameExists:
                message = "Name already exits";
                break;
            case CreationError.NameShort:
                message = "Name is too short, Need to be longer then 4 letters";
                break;
            default:
                break;
        }
        errorText.text = message;
    }
    private void CheckNameValidation()
    {
        checkCharacterNameValidationButton.interactable = false;
        if (IsNameTooShort())
        {
            HandleCreationButton(CreationError.NameShort);
            checkCharacterNameValidationButton.interactable = true;
            return;
        }

        //CheckIfNameIsAvailable();

        HandleCreationButton(CreationError.Available);
        checkCharacterNameValidationButton.interactable = true;
    }

    private bool IsNameTooShort()
    {
        return _characterName.Length < Name_Length_Min;
    }
    private void CheckIfNameIsAvailable()
    {
        PlayFabCharacterCreator.GetCharacterPlayfabData(_characterName, CheckIfCharacterExists);
    }
    private void CheckIfCharacterExists(GetCharacterDataResult result)
    {
        if (result != null)
        {
            _isNameTaken = true;
            HandleCreationButton(CreationError.NameExists);
            //There is a character existed with this nickname
            return;
        }
        _isNameTaken = false;
        //The name is available
        HandleCreationButton(CreationError.Available);
    }


    private void HandleClassButtonInteraction()
    {
        mageButton.interactable = _currentSelectedClass != ClassType.Mage;
        warriorButton.interactable = _currentSelectedClass != ClassType.Warrior;
        rangerButton.interactable = _currentSelectedClass != ClassType.Ranger;
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

}

















































///Easter Egg 1#