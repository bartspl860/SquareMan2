using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Intro_Handler : MonoBehaviour
{

    [SerializeField] private RectTransform videoScreen;
    [SerializeField] private String next_scene_path;
    [SerializeField] private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += CheckOver;
    }

    // Update is called once per frame
    void Update()
    {
        videoScreen.sizeDelta = new Vector2(Screen.width, Screen.width/1.7777f);
    }
    
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(next_scene_path);
    }
}
