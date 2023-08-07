using UnityEngine;
using GoogleTextToSpeech.Scripts.Data;
using TMPro;

namespace GoogleTextToSpeech.Scripts.Example
{
    public class TextToSpeechExample : MonoBehaviour
    {
        [SerializeField] private VoiceScriptableObject voice;
        private TextToSpeech textToSpeech;
        private AudioSource audioSource;
        [SerializeField] private TextMeshProUGUI aiInput;

        private void Start()
        {
            audioSource = GetComponentInParent<AudioSource>();
        }

        public void PressBtn()
        {
            textToSpeech = FindObjectOfType<TextToSpeech>();
            textToSpeech.GetSpeechAudioFromGoogle(aiInput.text, voice, OnAudioClipReceived, OnErrorReceived);
        }

        private void OnErrorReceived(BadRequestData badRequestData)
        {
            Debug.Log($"Error {badRequestData.error.code} : {badRequestData.error.message}");
        }

        private void OnAudioClipReceived(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}
