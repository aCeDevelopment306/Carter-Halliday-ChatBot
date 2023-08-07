using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThangChibaGPT
{
    [RequireComponent(typeof(ScrollRect))]
    public class GUIChatController : AIChatController
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Frame frameChat;
        [SerializeField] private ChatGPT chatGPT;
        private Message chunkMessage;

        // Rename chatStorage to guiChatStorage
        [SerializeField] private AIChatStorage guiChatStorage;

        public override void OnSubmitChat(string content)
        {
            frameChat.AddChatMessage(content, "user");

            // Initialize the chunkMessage if it's null or create a new one if needed
            if (chunkMessage == null)
            {
                chunkMessage = frameChat.AddAIChatMessage("", "assistant");
            }

            // Set the content of the AI message to the received chunk
            if (chunkMessage != null)
            {
                chunkMessage.SetContent(""); // Use an empty string as you are updating the AI message content later in OnReceiveChunkResponse
            }
            else
            {
                Debug.LogError("chunkMessage is null.");
            }

            ScrollToBottom();
            chatGPT.Send(content, this);
        }

        public override void OnReceiveChunkResponse(string content)
        {
            Debug.Log("Received AI response chunk: " + content);

            // Make sure chunkMessage is not null before setting the content
            if (chunkMessage != null)
            {
                chunkMessage.SetContent(content); // Set the content of the message to the received chunk
            }
            else
            {
                Debug.LogError("chunkMessage is null.");
            }

            ScrollToBottom();
            chatGPT.Send(content, this);
        }

        private void ScrollToBottom()
        {
            // Set the vertical normalized position to 0
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
