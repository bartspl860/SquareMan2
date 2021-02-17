using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public void startAudioClip(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
