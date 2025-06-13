
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Homework
{
    public class GameplayUIManagerHW : MonoBehaviour
    {

        [SerializeField] private List<Button> characterButtons;


        private void OnEnable()
        {
            CharacterSelectionManager.OnSelectedCharacter += UpdateButtons;
        }

        private void OnDisable()
        {
            CharacterSelectionManager.OnSelectedCharacter -= UpdateButtons;
        }


        public void ClickCharacterButtonHandler(int index)
        {
            Debug.Log($"On click character: {index}");
            GameManagerHW.Instance.CharacterSelectionManager.RPCSetCharacterSelection(index);
            UpdateButtons();
        }
        private void UpdateButtons()
        {
            var characters = GameManagerHW.Instance.CharacterSelectionManager.GetCharacterSelectionList;
            for (int i = 0; i < characters.Count; i++)
            {
                characterButtons[i].interactable = !characters[i].IsSelected;
            }
        }
    }
}