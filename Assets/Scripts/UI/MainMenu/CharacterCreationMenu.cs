using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationMenu : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject panel;
    [Header("Scriptable objects data")]
    [SerializeField] private List<BaseClassData> classesData;
    [SerializeField] private CharacterVisualSO visualSO;

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
    ///Character visual customization?

    private HairColorType hairColorType;


    private bool _spamPreventation;
    private string _characterName;
    private ClassType _characterSelectedClass;
    

    [Header("Testing")]
    public HairColorType startHairToTest = HairColorType.Ginger;


    private void OnEnable()
    {
        characterNameInput.onValueChanged.AddListener(UpdateCharacterInput);

       // createCharacterButton.onClick.AddListener(OnClickCreateButton);

        ///Will be a dream to make it more flexible but its good for now :D
        mageButton.onClick.AddListener(ClickMageButtonHandler);
    }
    private void OnDisable()
    {
        characterNameInput.onValueChanged.RemoveListener(UpdateCharacterInput);

      //  createCharacterButton.onClick.RemoveListener(OnClickCreateButton);

        mageButton.onClick.RemoveListener(ClickMageButtonHandler);
    }
    private void Awake()
    {
        ShowPanel();
    }
    public void ShowPanel()
    {
        panel.SetActive(true);
        SelectHairColor(startHairToTest);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
    }


    public void OnSelectedClassButton(ClassType selectedClass)
    {
        if(TryGetClassDataByClassType(selectedClass,out var data))
        {
            _characterSelectedClass = selectedClass;
            className.text = data.ClassName;
            classDescription.text = data.ClassDescription;
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
    private bool TryGetClassDataByClassType(ClassType type,out BaseClassData data)
    {
        data = classesData.Find(c => c.GetClassData.GetClassID == type);
        if(!data)
        {
            Debug.LogError($"[Character Creation Menu] Failed to load class data: {type.ToString()}");
        }
        return data;
    }

    private void OnClickCreateButton()
    {
        if (_spamPreventation) return;

        _spamPreventation = true;
    }
    private void UpdateCharacterInput(string input)
    {
        _characterName = input;
    }


}

















































///Easter Egg 1#