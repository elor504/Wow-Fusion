
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homework
{
    public class UIManagerHW : MonoBehaviour
    {
        [SerializeField] private GameObject characterSelectionMenu;
        [SerializeField] private List<Button> characterButtons;
        [SerializeField] private Button selectCharacterButton;
        [SerializeField] private TextMeshProUGUI selectCharacterButtonTest;
        [SerializeField] private Button closeGameButton;
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


        private async void Start()
        {
            SetShowCloseGame(false);
            while (GameManagerHW.Instance == null)
            {
                await Task.Yield();
            }
            Init();
        }

        public void Init()
        {
            SetShowCloseGame(false);
            closeGameButton.onClick.AddListener(OnClickCloseButton);
            UpdateCharacterSelectButton();
        }

        public void SetShowCloseGame(bool isHost)
        {
            closeGameButton.gameObject.SetActive(isHost);
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
            characterSelectionMenu.SetActive(false);
            //spawn
            GameManagerHW.Instance.RPC_RequestSpawn();
            SetShowCloseGame(GameManagerHW.Instance.Runner.IsSharedModeMasterClient);
        }
        private void OnClickCloseButton()
        {
            GameManagerHW.Instance.CloseGame();
            closeGameButton.interactable = false;
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
            if (currentLocalSelectedIndex == -1)
            {
                selectCharacterButton.interactable = false;
                selectCharacterButtonTest.text = "Select a character";
                return;
            }

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