using UnityEngine;

namespace ThangChibaGPT
{
    public class ChatManager : MonoBehaviour
    {
        public GUIChatController guiChatController; // Reference to the GUIChatController
        public ChatGPT chatGpt; // Reference to the ChatGPT class (or any other appropriate type)

        private static ChatManager instance;
        public static ChatManager Instance { get; private set; }

        // Add the ChatMode property
        public bool ChatMode { get; set; }

        private void Awake()
        {
            // Initialize the singleton instance
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            // Initialize the chatGpt reference if needed
            if (chatGpt == null)
            {
                // You can instantiate the chatGpt here or assign it from elsewhere in the code
                // Example: chatGpt = new ChatGPT();
            }
        }

        // Method to set the chat mode
        public void OnSetChatMode(bool mode)
        {
            ChatMode = mode;
        }

        public void SubmitChat(string content)
        {
            guiChatController.OnSubmitChat(content);
        }

    }
}
