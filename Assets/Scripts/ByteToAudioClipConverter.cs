using UnityEngine;
using System;

public class ByteToAudioClipConverter : MonoBehaviour
{
    // This method takes a byte array as input and returns an AudioClip.
    public AudioClip ConvertByteToAudioClip(byte[] audioData, int sampleRate, int channels)
    {
        // Calculate the length of the AudioClip in seconds
        float lengthInSeconds = (float)audioData.Length / (float)(sampleRate * (int)channels * sizeof(short));

        // Create a new AudioClip
        AudioClip audioClip = AudioClip.Create("AudioClip", audioData.Length / sizeof(float), (int)channels, sampleRate, false);

        // Set the raw audio data to the AudioClip
        audioClip.SetData(ToFloatArray(audioData), 0);

        return audioClip;
    }

    // Helper method to convert a byte array to a float array (required by AudioClip.SetData)
    private float[] ToFloatArray(byte[] byteArray)
    {
        float[] floatArray = new float[byteArray.Length / sizeof(float)];

        for (int i = 0; i < floatArray.Length; i++)
        {
            floatArray[i] = BitConverter.ToSingle(byteArray, i * sizeof(float));
        }

        return floatArray;
    }
}
