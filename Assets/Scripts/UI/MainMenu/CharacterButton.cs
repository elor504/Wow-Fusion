using TMPro;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charClassLevel;


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
