using Homework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private ChatMessageUI messageUIPF;
    [SerializeField] private Transform messageParentContent;
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private Button sendMessageButton;
    [SerializeField] private int maxMessages = 6;
    [SerializeField] private Color whisperColor;
    private Color defaultColor = Color.black;
    private List<ChatMessageUI> _messagesInstances = new List<ChatMessageUI>();
    private List<MessageInfo> _messageInfos = new List<MessageInfo>();

    private string _message;

    private void Awake()
    {
        GameManagerHW.ChatManager = this;
        messageInput.onValueChanged.AddListener(UpdateMessageInput);
        sendMessageButton.onClick.AddListener(SendMessageAll);
    }


    private void UpdateMessageInput(string value)
    {
        _message = value;
    }

    public void SendMessageAll()
    {
        if(_message.IsNullOrEmpty())
        {
            return;
        }


        if (_message.StartsWith("/w"))
        {
            Debug.Log($"[Client] attempting to whisper to someone");

            string[] split = _message.Split(new[] { ' ' }, 3);//thx chatgpt

            var targetName = split[1];
            var message = split[2];
            var senderName = PlayerList.Instance.GetLocalPlayerName();
            string hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(whisperColor);
            var messageToSend = $"<color=#{hexColor}>[Whisper]</color> " + message;
            GameManagerHW.Instance.RPC_SendPrivateMessage(senderName, messageToSend, defaultColor, targetName);

            var localMessage = $"<color=#{hexColor}>[{targetName}]</color> " + message;
            MessageInfo info;
            info.Message = localMessage;
            info.SenderName = senderName;
            info.TextColor = defaultColor;
            AddMessage(info);
        }
        else
        {
            MessageInfo info;
            info.SenderName = PlayerList.Instance.GetLocalPlayerName();
            info.Message = _message;
            info.TextColor = Color.black;
            Debug.Log($"[Client] attempting to send a message to all");
            GameManagerHW.Instance.RPC_SendMessageToAll(info.SenderName, info.Message, defaultColor);
        }
        messageInput.text = "";
    }

    public void AddMessage(MessageInfo message)
    {
        if (_messagesInstances.Count < maxMessages)
        {
            _messageInfos.Add(message);
            var messageInstance = Instantiate(messageUIPF, messageParentContent);
            messageInstance.SetMessage(message.SenderName, message.Message, message.TextColor);
            _messagesInstances.Add(messageInstance);
        }
        else if (_messageInfos.Count == maxMessages)
        {
            _messageInfos.RemoveAt(0);
            _messageInfos.Add(message);
            for (int i = 0; i < _messageInfos.Count; i++)
            {
                _messagesInstances[i].SetMessage(_messageInfos[i].SenderName, _messageInfos[i].Message, _messageInfos[i].TextColor);
            }
        }
    }


}

public struct MessageInfo
{
    public string SenderName;
    public string Message;
    public Color TextColor;
}
