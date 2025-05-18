using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charClassLevel;

    public Button GetButton => button;

    public void ShowButton(string characterName,CharacterData characterData)
    {
        charName.text = characterName;

        ClassType className = (ClassType)characterData.ClassType;
        charClassLevel.text = $"{className} ({characterData.CharacterLevel})";

        gameObject.SetActive(true);
    }
    public void HideButton()
    {
        gameObject.SetActive(false);
    }

}
