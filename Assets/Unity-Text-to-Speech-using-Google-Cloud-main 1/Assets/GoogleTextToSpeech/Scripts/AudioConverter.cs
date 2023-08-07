using System;
using System.Collections;
using System.IO;
using GoogleTextToSpeech.Scripts.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace GoogleTextToSpeech.Scripts
{
    public class AudioConverter : MonoBehaviour
    {
        private const string Mp3FileName = "audio.mp3";

        public static void SaveTextToMp3(AudioData audioData)
        {
            var bytes = Convert.FromBase64String(audioData.audioContent);
            File.WriteAllBytes(Application.temporaryCachePath + "/" + Mp3FileName, bytes);
        }

        public void LoadClipFromMp3(Action<AudioClip> onClipLoaded)
        {
            StartCoroutine(LoadClipFromMp3Cor(onClipLoaded));
        }

        private IEnumerator LoadClipFromMp3Cor(Action<AudioClip> onClipLoaded)
        {
            string filePath = "file://" + Application.temporaryCachePath + "/" + Mp3FileName;

            using var webRequest = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onClipLoaded.Invoke(DownloadHandlerAudioClip.GetContent(webRequest));
            }
            else
            {
                Debug.LogError("Error loading audio clip: " + webRequest.error);
            }
        }
    }
}
