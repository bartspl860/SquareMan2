using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [SerializeField] private Transform t_player;
    [SerializeField] private BoxCollider2D b2d_player;
    [SerializeField] private LayerMask hurt;
    [SerializeField] private LayerMask Teleport;
    [SerializeField] private GameObject TeleportInfo;
    public Movement movement;
    public Animation animation;

    [SerializeField] List<Vector3> teleportCordinates;
    private bool disablePressE = false;
    [SerializeField] private GameObject teleportButton_prefab;
    [SerializeField] private GameObject teleportMenuObj;

    private void Start()
    {

    }

    void FixedUpdate()
    {
        
        if (t_player.position.y < -10 || Input.GetKey("escape"))
        {
            t_player.position = new Vector3(0f, 1.7f, 0);
        }

        //collision with obstacle
        if (b2d_player.IsTouchingLayers(hurt))
        {
            t_player.position = new Vector3(0f, 1.7f, 0f);
        }

        if (b2d_player.IsTouchingLayers(Teleport))//is near teleport
        {
            if(!disablePressE) TeleportInfo.SetActive(true);
            
            if (Input.GetKey(KeyCode.E))
            {
                TeleportInfo.SetActive(false);
                disablePressE = true;
                teleportMenu();
            }
        }
        else
        {
            TeleportInfo.SetActive(false);
            disablePressE = false;
            animation.sr_player.enabled = true;
        }
        
    }
    private void teleportMenu()
    {
        Time.timeScale = 0f;
        animation.sr_player.enabled = false;
        teleportMenuObj.SetActive(true);
    }

    private void tpTo(Vector3 cordinates)
    {
        teleportMenuObj.SetActive(false);
        Time.timeScale = 1f;
        animation.sr_player.enabled = true;
        disablePressE = false;
        movement.t_player.position = cordinates;
    }

    public void tpGunLVL()
    {
        tpTo(teleportCordinates[1]);
    }

    public void tpStartPlatform()
    {
        tpTo(teleportCordinates[0]);
    }
}
