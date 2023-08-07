using UnityEngine;
using System.Collections.Generic;

namespace ThangChibaGPT
{
    public abstract class AIChatController : MonoBehaviour
    {
        public AIChatStorage chatStorage;
        public List<AIMessage> messages = new List<AIMessage>();

        public void SetChatStorage(AIChatStorage storage)
        {
            chatStorage = storage;
        }

        public void AddMessage(string content)
        {
            if (chatStorage != null)
            {
                chatStorage.messages.Add(new AIMessage(Role.Assistant, content));
            }
            else
            {
                Debug.LogError("AIChatStorage is not set in AIChatController. Please make sure to set it before using the controller.");
            }
        }

        public virtual void OnSubmitChat(string content)
        {
            AddMessage(content);
        }

        public virtual void OnReceiveResponse(string content)
        {
            AddMessage(content);
        }

        public abstract void OnReceiveChunkResponse(string content);
    }
}
