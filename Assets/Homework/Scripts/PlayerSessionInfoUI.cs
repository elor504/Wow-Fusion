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

        private PlayerRef _playerRef;
        private bool _isReady;

        public PlayerRef GetPlayerRef => _playerRef;


        public void ShowPlayer(PlayerRef playerRef,string name)
        {
            _playerRef = playerRef;
            playerNameText.text = name;
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