using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("Player variables")]
    [SerializeField] private Rigidbody2D rb2d_player;
    public Transform t_player;
    [SerializeField] private float velocity;
    [SerializeField] private float jump;
    [SerializeField] private float block;

    //check ground
    [Header("Check Ground")]
    [SerializeField] private BoxCollider2D ground_checker;
    [SerializeField] private LayerMask ground;

    //equipment
    [Header("Equipment")]
    [SerializeField] private Sprite[] equipment;
    [SerializeField] private SpriteRenderer show_eq;
    [SerializeField] private RectTransform show_eq_RectTransform;
    public int eq_control = 0;


    [Header("Circle Menu")]
    //circled menu
    [SerializeField] 
    private GameObject circled_menu;    
    private Vector2 moveInput;
    [SerializeField]
    private GameObject highlight;

    private RectTransform menuPiece;
    private Vector2 dir;
    private Vector2 jumpVector2;
    
    //time control
    public Enviroment Enviroment;
    
    //animation
    public Animation Animation;


    private void Start()
    {
        menuPiece =  highlight.GetComponent<RectTransform>();
        dir = transform.right * velocity * Time.deltaTime;
    }

    void Update()
    {
        if (Input.GetKey(eq)) circled_menu.SetActive(true);
        else circled_menu.SetActive(false);

        show_eq.sprite = equipment[eq_control];
        show_eq_RectTransform.localScale = new Vector3(150f, 150f, 1f);

        if (circled_menu.activeInHierarchy)
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
                        eq_control = 1;
                    }
                }
                if (angle > 90f && angle < 180f)
                {
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 90f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        eq_control = 2;
                    }
                }
                if (angle > 180f && angle < 270f)
                {
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 0f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        eq_control = 3;
                    }
                }
                if (angle > 270f && angle < 360f)
                {
                    menuPiece.eulerAngles = new Vector3(0f, 0f, 270f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        eq_control = 4;
                    }
                }
                //Debug.Log(angle);
            }            
        }
        switch (eq_control)
        {
            case 1: sprint(); break;
            case 2: highJump(); break;
            case 3: gravityControl(); break;
            case 4: timeControl(); break;
        }
    }
    void FixedUpdate()
    {
        
        if (Input.GetKey(right))
        {
            rb2d_player.AddForce(dir);           
        }
        if (Input.GetKey(left))
        {
            rb2d_player.AddForce(-dir);            
        }
      
        if (ground_checker.IsTouchingLayers(ground))
        {           
            if (Input.GetKey(space))
            {
                rb2d_player.AddForce(transform.up * jump * Time.deltaTime, ForceMode2D.Impulse);                    
            }         
        }       

        if(rb2d_player.velocity.x > block)
        {
            rb2d_player.AddForce(-dir);
        }
        if(rb2d_player.velocity.x < -block)
        {
            rb2d_player.AddForce(dir);
        }        
    }

    void sprint()
    {        
        jump = 450f;
        if (ground_checker.IsTouchingLayers(ground))
        {
            //sprint
            if (Input.GetKey(action))
            {
                block = 8f;
            }
            else
            {
                block = 5f;
            }
        }
    }
    void highJump()
    {        
        block = 5f;
        if (ground_checker.IsTouchingLayers(ground))
        {
            //high jump
            if (Input.GetKey(action))
            {
                jump = 900f;
            }
            else
            {
                jump = 450f;
            }
        }
    }
    void gravityControl()
    {        
        block = 5f;
        jump = 450f;
        if (ground_checker.IsTouchingLayers(ground))
        {
            //gravity control
            if (Input.GetKeyDown(action))
            {
                rb2d_player.gravityScale *= -1;
                if (rb2d_player.gravityScale > 0)
                {
                    t_player.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else
                {
                    t_player.rotation = Quaternion.Euler(180f, 0f, 0f);
                }
            }
        }
    }
    void timeControl()
    {        
        block = 5f;
        jump = 450f;
        //time control
        if (Input.GetKey(action))
        {
            Enviroment.rotationSpeed = 10;
        }
        else
        {
            Enviroment.rotationSpeed = 50;
        }
    }
}
