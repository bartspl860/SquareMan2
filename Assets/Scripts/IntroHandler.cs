using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroHandler : MonoBehaviour
{

    [SerializeField] private RectTransform videoScreen;
    [FormerlySerializedAs("next_scene_path")] [SerializeField] private String nextScenePath;
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
        if(Input.anyKey)SceneManager.LoadScene(nextScenePath);
    }
    
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScenePath);
    }
}
