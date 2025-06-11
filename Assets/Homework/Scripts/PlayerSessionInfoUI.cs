using TMPro;
using UnityEngine;
namespace Homework01
{
    public class PlayerSessionInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;




        public void ShowPlayer(string name)
        {
            playerNameText.text = name;
            gameObject.SetActive(true);
        }
        public void HidePlayer()
        {
            gameObject.SetActive(false);
        }

    }
}