using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuClick;
    [SerializeField] private String path;


    public void StartGame()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        audioSource.loop = false;
        audioSource.volume = 0.2f;
        AudioHandler.Instance.StartAudioClip(audioSource,menuClick);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(path);
    }
}
