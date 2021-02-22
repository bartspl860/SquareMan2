using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text thanks;
    [SerializeField] private Image logo;
    [SerializeField] private TMP_Text getReady;


    private void Start()
    {
        thanks.color = new Color(255, 255, 255, 0);
        logo.color = new Color(255, 255, 255, 0);
        getReady.color = new Color(255, 255, 255, 0);
        StartCoroutine(FadeInTextAndLogo());
    }

    
    private bool fade1 = false;
    private bool fade2 = false;
    private bool fade3 = false;

    private float transparent1 = 0f;
    private float transparent2 = 0f;
    private float transparent3 = 0f;
    void FixedUpdate()
    {
        if (fade1 && transparent1 <=1f)
        {
            thanks.color = new Color(1f, 1f, 1f, transparent1);
            transparent1+=0.01f;
        }

        if (fade2 && transparent2 <=1f)
        {
            logo.color = new Color(1f, 1f, 1f, transparent2);
            transparent2+=0.01f;
        }

        if (fade3 && transparent3 <=1f)
        {
            getReady.color = new Color(1f, 1f, 1f, transparent3);
            transparent3+=0.01f;
        }

        if (transparent3 >= 1f)
        {
            if (Input.anyKey)
            {
                Application.Quit();
            }
        }
    }
    IEnumerator FadeInTextAndLogo()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.PlaySound("Win");
        yield return new WaitForSeconds(1);
        fade1 = true;
        yield return new WaitForSeconds(2);
        fade2 = true;
        yield return new WaitForSeconds(2);
        fade3 = true;
    }
}
