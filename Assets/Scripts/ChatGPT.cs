using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ThangChibaGPT
{
    [System.Serializable]
    public class ChunkResponse
    {
        public string message;
        public string role;
    }

    public class ChatGPT : MonoBehaviour
    {
        [SerializeField]
        private string endPoint = "https://api.openai.com/v1/chat/completions";
        [SerializeField]
        private string accessToken = "Bearer sk-";
        [SerializeField]
        private string accessKey = "freetoken";
        [SerializeField]
        private EGPTModel egptModel = EGPTModel.GPT35Turbo;

        [SerializeField] GUIChatController guiChatController;

        private const int MaxRequestsPerMinute = 100;
        private const float RequestCooldownSeconds = 60f / MaxRequestsPerMinute;

        private float requestCooldown;
        private Dictionary<string, string> responseCache = new Dictionary<string, string>();

        private void Start()
        {
            guiChatController = FindObjectOfType<GUIChatController>();
        }
        public void Send(string chatContent, GUIChatController controller)
        {
            if (Time.time < requestCooldown)
            {
                Debug.LogWarning("Request throttled. Please wait before sending another request.");
                return;
            }

            var sendMessages = new List<AIMessage>(controller.chatStorage.trains);
            sendMessages.AddRange(controller.chatStorage.messages.TakeLast(controller.chatStorage.maxSendCount).ToList());

            // The error is likely happening in the following line (line 55)
            if (responseCache.TryGetValue(chatContent, out string cachedResponse))
            {
                controller.OnReceiveChunkResponse(cachedResponse);
            }
            else
            {
                StartCoroutine(ChatWithAI(chatContent, controller, sendMessages));
            }
        }
        private IEnumerator ChatWithAI(string chatContent, GUIChatController controller, List<AIMessage> sendMessages)
        {
            // Check if the request is still on cooldown
            if (Time.time < requestCooldown)
            {
                Debug.LogWarning("Request throttled. Please wait before sending another request.");
                yield break;
            }

            var requestBody = new AIRequestBody
            {
                model = GetModelName(egptModel),
                messages = sendMessages,
                temperature = guiChatController.chatStorage.temperature,
                max_tokens = guiChatController.chatStorage.maxTokens
            };
            var bodyJsonString = JsonUtility.ToJson(requestBody);
            var bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            var downloadHandlerBuffer = new DownloadHandlerBuffer();

            using (var request = new UnityWebRequest(endPoint, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = downloadHandlerBuffer;
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", accessToken);
                request.SetRequestHeader("AccessKey", accessKey);
                Debug.Log("Sending request: " + request.url);
                yield return request.SendWebRequest();

                // Check for rate limit response (429) and handle retry using exponential backoff
                int maxRetryAttempts = 5; // Maximum number of retry attempts
                int retryAttempt = 0;
                int baseWaitTime = 1; // Initial waiting time in seconds
                while (request.responseCode == 429 && retryAttempt < maxRetryAttempts)
                {
                    retryAttempt++;
                    int waitTime = (int)Mathf.Pow(2, retryAttempt) * baseWaitTime;
                    Debug.LogWarning($"Rate limit exceeded. Waiting {waitTime} seconds before retry (Attempt {retryAttempt}/{maxRetryAttempts})...");

                    yield return new WaitForSeconds(waitTime);
                    Debug.Log("Retrying request...");
                    yield return request.SendWebRequest();
                }

                if (request.responseCode == 429)
                {
                    Debug.LogError("Too many retry attempts. Failed to get a valid response from the AI service.");
                    yield break;
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var response = System.Text.Encoding.UTF8.GetString(downloadHandlerBuffer.data);
                    Debug.Log("API response: " + response);

                    // Parse the response and extract the AI message content
                    ChunkResponse chunkResponse = JsonUtility.FromJson<ChunkResponse>(response);
                    if (!string.IsNullOrEmpty(chunkResponse.message))
                    {
                        // Add the AI message to the GUI only when a valid response is received
                        controller.OnReceiveChunkResponse(chunkResponse.message);
                    }
                    else
                    {
                        Debug.LogWarning("AI response is empty or invalid.");
                        // If the AI response is empty or invalid, you can handle this case as needed.
                        // For example, you can display an error message or a default response.
                        // You may also choose to do nothing and not add an AI chat message in this case.
                    }

                    // Store the response in the cache
                    responseCache[chatContent] = response;
                }
                else
                {
                    Debug.LogError("Error sending request: " + request.error);
                }
            }
        }




        private string GetModelName(EGPTModel model)
        {
            switch (model)
            {
                case EGPTModel.GPT35Turbo:
                    return "gpt-3.5-turbo";
                case EGPTModel.GPT4:
                    return "gpt-4";
                default:
                    throw new System.ArgumentOutOfRangeException("Invalid GPTModel value");
            }
        }
    }
}
