using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    //keycodes
    [Header("Keycodes")]
    public KeyCode left;
    public KeyCode right;
    public KeyCode space;
    public KeyCode action;
    public KeyCode eq;

    //player variables
    [FormerlySerializedAs("rb2d_player")]
    [Header("Player variables")]
    [SerializeField] private Rigidbody2D rb2dPlayer;
    [FormerlySerializedAs("t_player")] public Transform tPlayer;
    [SerializeField] private float velocity;
    [SerializeField] private float jump;
    [SerializeField] private float block;

    //check ground
    [FormerlySerializedAs("ground_checker")]
    [Header("Check Ground")]
    [SerializeField] private BoxCollider2D groundChecker;
    [SerializeField] private LayerMask ground;

    //equipment
    [Header("Equipment")]
    [SerializeField] private Sprite[] equipment;
    [FormerlySerializedAs("show_eq")] [SerializeField] private SpriteRenderer showEq;
    [FormerlySerializedAs("show_eq_RectTransform")] [SerializeField] private RectTransform showEqRectTransform;
    [FormerlySerializedAs("eq_control")] public int eqControl = 0;


    [FormerlySerializedAs("circled_menu")]
    [Header("Circle Menu")]
    //circled menu
    [SerializeField] 
    private GameObject circledMenu;    
    private Vector2 moveInput;
    [SerializeField]
    private GameObject highlight;

    private RectTransform menuPiece;
    private Vector2 dir;
    private Vector2 jumpVector2;
    
    //time control
    [FormerlySerializedAs("Enviroment")] public Enviroment enviroment;
    
    //animation
    [FormerlySerializedAs("Animation")] public Animation animation;
    
    //Audio
    [Header("Audio System")]

    [FormerlySerializedAs("AudioSource")] [SerializeField] private AudioSource audioSource;
    
    private AudioClip jumpClip;
    [FormerlySerializedAs("LowJumpClip")] [SerializeField] private AudioClip lowJumpClip;
    [FormerlySerializedAs("HighJumpClip")] [SerializeField] private AudioClip highJumpClip;


    private void Start()
    {
        menuPiece =  highlight.GetComponent<RectTransform>();
        dir = transform.right * (velocity * Time.deltaTime);
        jumpClip = lowJumpClip;
    }


    void Update()
    {
        circledMenu.SetActive(Input.GetKey(eq));


        showEq.sprite = equipment[eqControl];
        showEqRectTransform.localScale = new Vector3(150f, 150f, 1f);

        if (circledMenu.activeInHierarchy)
        {
            moveInput.x = Input.mousePosition.x - (Screen.width/2f);
            moveInput.y = Input.mousePosition.y - (Screen.height/2f);
            moveInput.Normalize();

            if(moveInput != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
                angle *= 180f;
                if(angle < 0)
                {
                    angle += 360f;
                }

                if(angle > 0f && angle < 90f)
                {                    
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 180f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        AudioManager.instance.PlaySound("Navigate1");
                        eqControl = 1;
                    }
                }
                if (angle > 90f && angle < 180f)
                {
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 90f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        AudioManager.instance.PlaySound("Navigate1");
                        eqControl = 2;
                    }
                }
                if (angle > 180f && angle < 270f)
                {
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 0f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        enviroment.MenuClickClip();
                        eqControl = 3;
                    }
                }
                if (angle > 270f && angle < 360f)
                {
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 270f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        AudioManager.instance.PlaySound("Navigate1");
                        eqControl = 4;
                    }
                }
            }            
        }
        switch (eqControl)
        {
            case 1: Sprint(); break;
            case 2: HighJump(); break;
            case 3: GravityControl(); break;
            case 4: TimeControl(); break;
        }
    }
    void FixedUpdate()
    {
        //Movement left and right
        if (Input.GetKey(right))
        {
            rb2dPlayer.AddForce(dir);           
        }
        if (Input.GetKey(left))
        {
            rb2dPlayer.AddForce(-dir);            
        }
        
        //Jump
        if (groundChecker.IsTouchingLayers(ground))
        {
            if (Input.GetKey(space))
            {
                rb2dPlayer.AddForce(transform.up * (jump * Time.deltaTime), ForceMode2D.Impulse);
                AudioManager.instance.PlaySound("Jump");
            }         
        }       

        //blokada prędkości
        if(rb2dPlayer.velocity.x > block)
        {
            rb2dPlayer.AddForce(-dir);
        }
        if(rb2dPlayer.velocity.x < -block)
        {
            rb2dPlayer.AddForce(dir);
        }        
    }

    void Sprint()
    {        
        jump = 450f;
        if (groundChecker.IsTouchingLayers(ground))
        {
            //sprint
            block = Input.GetKey(action) ? 8f : 5f;
        }
    }
    void HighJump()
    {        
        block = 5f;
        if (groundChecker.IsTouchingLayers(ground))
        {
            //high jump
            if (Input.GetKey(action))
            {
                jumpClip = highJumpClip;
                jump = 900f;
            }
            else
            {
                jumpClip = lowJumpClip;
                jump = 450f;
            }
        }
    }
    void GravityControl()
    {        
        block = 5f;
        jump = 450f;
        //gravity control
        
        if (!groundChecker.IsTouchingLayers(ground)) return;
        if (!Input.GetKeyDown(action)) return;
        
        var gravityScale = rb2dPlayer.gravityScale;
        gravityScale *= -1;
        rb2dPlayer.gravityScale = gravityScale;
        tPlayer.rotation = Quaternion.Euler(gravityScale > 0 ? 0f : 180f, 0f, 0f);
    }

    private bool onlyOnce = false;
    void TimeControl()
    {        
        block = 5f;
        jump = 450f;
        //time control
        if (Input.GetKey(action))
        {
            if (Input.GetKeyDown(action))
            {
                AudioManager.instance.PlaySound("TimeStopStart");
            }
                   
            
            enviroment.rotationSpeed = 10;
            onlyOnce = true;
        }
        else
        {
            if (onlyOnce)
            {
                AudioManager.instance.StopSound("TimeStopStart");
                AudioManager.instance.PlaySound("TimeStopEnd");
                onlyOnce = false;
            }
            enviroment.rotationSpeed = 50;
        }
    }
}
