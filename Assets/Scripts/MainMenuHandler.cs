using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private String path;


    private void Start()
    {
        AudioManager.instance.PlaySound("Background Music");
    }

    public void StartGame()
    {
        StartCoroutine(WaitToStart());
    }

    public void ExitGame()
    {
        StartCoroutine(WaitToExit());
    }

    IEnumerator WaitToStart()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.PlaySound("Click");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(path);
    }
    IEnumerator WaitToExit()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.PlaySound("Click");
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
