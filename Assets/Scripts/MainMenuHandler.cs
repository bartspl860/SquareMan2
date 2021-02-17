using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private String path;
    public void StartGame()
    {
        SceneManager.LoadScene(path);
    }
}
