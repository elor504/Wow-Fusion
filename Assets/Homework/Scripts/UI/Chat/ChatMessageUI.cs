using TMPro;
using UnityEngine;

public class ChatMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI senderName;
    [SerializeField] private TextMeshProUGUI messageText;


    public void SetMessage(string senderName, string message, Color textColor)
    {
        SetColor(textColor);
        this.senderName.text = senderName;
        messageText.text = message;
    }


    private void SetColor(Color color)
    {
        senderName.color = color;
        messageText.color = color;
    }
}
