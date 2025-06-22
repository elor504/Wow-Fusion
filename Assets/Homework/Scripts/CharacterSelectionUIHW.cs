
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homework
{
    public class CharacterSelectionUIHW : MonoBehaviour
    {
        [SerializeField] private List<Button> characterButtons;
        [SerializeField] private Button selectCharacterButton;
        [SerializeField] private TextMeshProUGUI selectCharacterButtonTest;

        private int currentLocalSelectedIndex = -1;
        private int previousIndex = -1;
        private void OnEnable()
        {
            CharacterSelectionManager.OnSelectedCharacter += UpdateCharactersButtons;
            selectCharacterButton.onClick.AddListener(ClickOnSelectCharacter);
        }
        private void OnDisable()
        {
            CharacterSelectionManager.OnSelectedCharacter -= UpdateCharactersButtons;
            selectCharacterButton.onClick.RemoveListener(ClickOnSelectCharacter);
        }

        public void ClickCharacterButtonHandler(int index)
        {
            previousIndex = currentLocalSelectedIndex;
            currentLocalSelectedIndex = index;
            var characters = GameManagerHW.CharacterSelectionManager.GetCharacterSelectionList;
            if (previousIndex != -1 && !characters[previousIndex].IsSelected)
                characterButtons[previousIndex].interactable = true;
            characterButtons[currentLocalSelectedIndex].interactable = false;

            Debug.Log($"On click character: {index}");
            UpdateCharacterSelectButton();
        }
        public void ClickOnSelectCharacter()
        {
            GameManagerHW.CharacterSelectionManager.RPCSetCharacterSelection(currentLocalSelectedIndex);
            UpdateCharactersButtons();
            //spawn
        }

        private void UpdateCharactersButtons()
        {
            var characters = GameManagerHW.CharacterSelectionManager.GetCharacterSelectionList;
            for (int i = 0; i < characters.Count; i++)
            {
                characterButtons[i].interactable = !characters[i].IsSelected;
            }
            UpdateCharacterSelectButton();
        }
        private void UpdateCharacterSelectButton()
        {
            var characters = GameManagerHW.CharacterSelectionManager.GetCharacterSelectionList;
            if (characters[currentLocalSelectedIndex].IsSelected)
            {
                selectCharacterButton.interactable = false;
                selectCharacterButtonTest.text = "Unavailable";
            }
            else
            {
                selectCharacterButton.interactable = true;
                selectCharacterButtonTest.text = "Select character";
            }
        }
    }
}