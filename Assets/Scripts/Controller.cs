using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;


public class Controller : MonoBehaviour
{
    //keycodes
    [Header("Keycodes")]
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode action = KeyCode.LeftShift;
    public KeyCode circledMenuBtn = KeyCode.Tab;

    //player instance
    [Header("Player")]
    public Player player;

    //CircledMenu instance
    public CircledMenu circledMenu;

    private void Start()
    {
        circledMenu.Initialize();
        player.Initialize();
    }

    [SerializeField]
    private SpriteRenderer pickedSkill;
    CircledMenuItem pickedElement = null;
    private bool isNotMoving;

    void FixedUpdate()
    {
        Debug.Log(player.Velocity);
        isNotMoving = true;
        if (Input.GetKey(left))
        {
            player.GoLeft(out bool result);
            isNotMoving = false;
        }
        if (Input.GetKey(right))
        {
            player.GoRight(out bool result);
            isNotMoving = false;
        }
        if (Input.GetKey(jump))
        {
            player.Jump(out bool result);
            isNotMoving = false;
        }
        if (isNotMoving)
        {
            player.StopPlayer();
        }
        
        if (Input.GetKey(circledMenuBtn))
        {
            circledMenu.Active = true;
            pickedElement = circledMenu.ChooseElement();
            if (pickedElement != null)
            {
                pickedSkill.sprite = pickedElement.Icon;
                player.AllAbilities.ActiveAction = pickedElement;
            }
        }
        else
        {
            circledMenu.Active = false;
        }

        if (player.AllAbilities.ActiveAction != null 
            && player.AllAbilities.ActiveAction.HoldAbility)
        {
            if (Input.GetKey(action))
            {
                player.AllAbilities.DoAction();
            }
            else
            {
                player.AllAbilities.ReverseAction();
            }
        }
    }

    public void Update()
    {
        if (player.AllAbilities.ActiveAction != null
           && !player.AllAbilities.ActiveAction.HoldAbility)
        {
            if (Input.GetKeyDown(action))
            {
                player.AllAbilities.DoAction();
            }
            if (Input.GetKeyUp(action))
            {
                player.AllAbilities.ReverseAction();
            }
        }
    }
}
