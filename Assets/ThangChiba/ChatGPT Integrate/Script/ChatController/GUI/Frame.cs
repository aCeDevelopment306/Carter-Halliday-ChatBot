using UnityEngine;

namespace ThangChibaGPT
{
    public class Frame : MonoBehaviour
    {
        public GameObject messagePrefab;
        public GameObject aiMessagePrefab;

        public Message AddChatMessage(string content, string role)
        {
            Debug.Log("Adding chat message: " + content + " - " + role);

            var newMessage = Instantiate(messagePrefab, gameObject.transform);
            if (newMessage == null)
            {
                Debug.LogError("messagePrefab is not instantiated correctly.");
                return null;
            }

            Message message = newMessage.GetComponent<Message>();
            if (message == null)
            {
                Debug.LogError("Message component is missing in the instantiated messagePrefab.");
                return null;
            }

            message.SetContent(content);
            message.SetAvatar(role);
            return message; // Return the Message component, not the GameObject
        }

        public Message AddAIChatMessage(string content, string role)
        {
            Debug.Log("Adding AI chat message: " + content + " - " + role);
            var newMessage = Instantiate(aiMessagePrefab, gameObject.transform);
            if (newMessage == null)
            {
                Debug.LogError("aiMessagePrefab is not instantiated correctly.");
                return null;
            }

            Message message = newMessage.GetComponent<Message>();
            if (message == null)
            {
                Debug.LogError("Message component is missing in the instantiated aiMessagePrefab.");
                return null;
            }

            message.SetContent(content);
            message.SetAvatar(role);
            return message; // Return the Message component, not the GameObject
        }

    }
}
