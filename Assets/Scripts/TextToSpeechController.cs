using System;
using Google.Cloud.TextToSpeech.V1;

public class TextToSpeechCli
{
    private readonly Google.Cloud.TextToSpeech.V1.TextToSpeechClient _client;

    public TextToSpeechCli()
    {
        // Set up authentication using the environment variable GOOGLE_APPLICATION_CREDENTIALS.
        _client = Google.Cloud.TextToSpeech.V1.TextToSpeechClient.Create();
    }

    public byte[] ConvertTextToSpeech(string text, string voiceName = "en-US-Wavenet-D", SsmlVoiceGender voiceGender = SsmlVoiceGender.Neutral, string languageCode = "en-US")
    {
        var input = new SynthesisInput
        {
            Text = text
        };

        var voiceSelection = new VoiceSelectionParams
        {
            LanguageCode = languageCode,
            Name = voiceName
        };

        var audioConfig = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Linear16
        };

        SynthesizeSpeechResponse response = _client.SynthesizeSpeech(input, voiceSelection, audioConfig);

        return response.AudioContent.ToByteArray();
    }
}
