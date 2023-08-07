using UnityEngine;
using Google.Cloud.TextToSpeech.V1;
using Google.Protobuf;
public class TextToSpeechClient : MonoBehaviour
{
    private Google.Cloud.TextToSpeech.V1.TextToSpeechClient _client;

    private void Awake()
    {
        // Set up authentication using the environment variable GOOGLE_APPLICATION_CREDENTIALS.
        // Make sure you have your service account key JSON file set in the environment variable.
        _client = new TextToSpeechClientBuilder().Build();
    }

    public static TextToSpeechClient Create()
    {
        var clientBuilder = new TextToSpeechClientBuilder();
        return new TextToSpeechClient();
    }

   /* public byte[] ConvertTextToSpeech(string text, string voiceName = "en-US-Wavenet-D", SsmlVoiceGender voiceGender = SsmlVoiceGender.Neutral, AudioEncoding audioEncoding = AudioEncoding.Linear16, string languageCode = "en-US")
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

        SynthesizeSpeechResponse response = _client.SynthesizeSpeech(input, voiceSelection, audioConfig);

        return response.AudioContent.ToByteArray();
    }*/
}
