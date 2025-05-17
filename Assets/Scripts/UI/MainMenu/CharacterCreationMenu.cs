using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationMenu : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject panel;

    [Header("Inputfield Reference")]
    [SerializeField] private TMP_InputField characterNameInput;

    [Header("Buttons References")]
    [SerializeField] private Button createCharacterButton;

    private string _characterName;

    private bool _spamPreventation;

    private void OnEnable()
    {
        characterNameInput.onValueChanged.AddListener(UpdateCharacterInput);

        createCharacterButton.onClick.AddListener(OnClickCreateButton);
    }
    private void OnDisable()
    {
        characterNameInput.onValueChanged.RemoveListener(UpdateCharacterInput);

        createCharacterButton.onClick.RemoveListener(OnClickCreateButton);
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
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
