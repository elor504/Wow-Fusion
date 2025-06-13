using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Homework
{
    public class PlayerSessionInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private Image readyImage;

        [SerializeField] private Sprite readySprite;
        [SerializeField] private Sprite notReadySprite;

        private PlayerRef _playerMultiplayerData;
        private bool _isReady;

        public PlayerRef GetPlayerData => _playerMultiplayerData;

        public void ShowPlayer(PlayerRef playerData)
        {
            _playerMultiplayerData = playerData;
            playerNameText.text = _playerMultiplayerData.PlayerId.ToString();
            gameObject.SetActive(true);
        }
        public void HidePlayer()
        {
            gameObject.SetActive(false);
        }


        public void SetReady(bool isReady)
        {
            _isReady = isReady;
            readyImage.sprite = _isReady ? readySprite : notReadySprite;
        }
    }
}