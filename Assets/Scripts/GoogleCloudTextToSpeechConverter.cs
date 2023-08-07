using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Google.Cloud.TextToSpeech.V1;
using Google.Protobuf;
using Google.Api.Gax;


public class GoogleCloudTextToSpeechConverter : MonoBehaviour
{
    private readonly Google.Cloud.TextToSpeech.V1.TextToSpeechClient _ttsClient;

    public GoogleCloudTextToSpeechConverter()
    {
        // Set up authentication using the environment variable GOOGLE_APPLICATION_CREDENTIALS.
        // Make sure you have your service account key JSON file set in the environment variable.
        _ttsClient = new TextToSpeechClientBuilder().Build();    }

    public byte[] ConvertTextToSpeech(string text, string voiceName = "en-US-Wavenet-D", SsmlVoiceGender voiceGender = SsmlVoiceGender.Neutral, AudioEncoding audioEncoding = AudioEncoding.Linear16, string languageCode = "en-US")
    {
        var input = new SynthesisInput
        {
            Text = text
        };

        var voiceSelection = new VoiceSelectionParams
        {
            LanguageCode = languageCode,
            Name = voiceName,
            SsmlGender = voiceGender
        };

        var audioConfig = new AudioConfig
        {
            AudioEncoding = audioEncoding
        };

        try
        {
            SynthesizeSpeechResponse response = _ttsClient.SynthesizeSpeech(input, voiceSelection, audioConfig);
            return response.AudioContent.ToByteArray();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Text to Speech Conversion Error: {ex.Message}");
            return null;
        }
    }
}
