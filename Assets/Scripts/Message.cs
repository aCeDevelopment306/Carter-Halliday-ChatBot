using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleTextToSpeech.Scripts.Example;

namespace ThangChibaGPT
{
    public class Message : MonoBehaviour
    {
        [SerializeField] public Sprite avatarUser;
        [SerializeField] public Sprite avatarAssistant;
        [SerializeField] public Image showAvatar;
        [SerializeField] public TextMeshProUGUI messageContent; // Add the TextMeshProUGUI component in the Inspector
        public bool isAI;

        public void SetContent(string content)
        {
            // Set the content of the message
            messageContent.text = content;

        }

        public void SetAvatar(string role)
        {
            showAvatar.sprite = role == "user" ? avatarUser : avatarAssistant;
        }
    }
}
