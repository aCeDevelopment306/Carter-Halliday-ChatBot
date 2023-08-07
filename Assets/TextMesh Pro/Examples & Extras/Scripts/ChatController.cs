using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ThangChibaGPT;
using TMPro;
public class ChatController : MonoBehaviour
{
    public TMP_InputField ChatInputField;
    public TMP_Text ChatDisplayOutput;
    public Scrollbar ChatScrollbar;
    public TextMeshProUGUI chatText;
    public GUIChatController guiChatController; // Reference to the AI chat controller

/*    void Start()
    {
        // Get the reference to the existing GUIChatController component in the scene
        guiChatController = FindObjectOfType<GUIChatController>();

        // Check if the GUIChatController reference is not null
        if (guiChatController == null)
        {
            Debug.LogError("GUIChatController reference is null in ChatController.");
        }
    }*/

    void OnEnable()
    {
        ChatInputField.onSubmit.AddListener(AddToChatOutput);
    }

    void OnDisable()
    {
        ChatInputField.onSubmit.RemoveListener(AddToChatOutput);
    }

    void AddToChatOutput(string newText)
    {
        // Clear Input Field
        ChatInputField.text = string.Empty;

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText;

        if (ChatDisplayOutput != null)
        {
            // No special formatting for first entry
            // Add line feed before each subsequent entries
            if (ChatDisplayOutput.text == string.Empty)
                ChatDisplayOutput.text = formattedInput;
            else
                ChatDisplayOutput.text += "\n" + formattedInput;
            if (chatText != null)
            {
                chatText.text += newText;
            }
        }

        // Send the user's input to the AI chat controller
        guiChatController.OnSubmitChat(newText);

        // Keep Chat input field active
        ChatInputField.ActivateInputField();

        // Set the scrollbar to the bottom when next text is submitted.
        ChatScrollbar.value = 0;
    }
}
