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
    [SerializeField] private Transform t_player;
    [SerializeField] private float velocity;
    [SerializeField] private float jump;
    [SerializeField] private float block;

    //check ground
    [Header("Check Ground")]
    [SerializeField] private BoxCollider2D ground_checker;
    [SerializeField] private LayerMask ground;

    //equipment
    [Header("Equipment")]
    [SerializeField] private string[] equipment;
    [SerializeField] private TMP_Text show_eq;
    private int eq_control = 0;


    void Update()
    {
        Debug.Log(Input.mousePosition);

        if (Input.GetKeyDown(eq))
        {
            eq_control++;
            if (eq_control >= equipment.Length)
                eq_control = 0;
        }
        show_eq.text = equipment[eq_control];

        switch (eq_control)
        {
            case 1:                
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
                break;
            case 2:
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
                break;
            case 3:
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
                break;
            case 4:
                block = 5f;
                jump = 450f;
                //time control
                if (Input.GetKey(action))
                {
                    Time.timeScale = 0.7f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
                break;
        }          
    }
    void FixedUpdate()
    {
        
        if (Input.GetKey(right))
        {
            rb2d_player.AddForce(transform.right * velocity * Time.deltaTime);           
        }
        if (Input.GetKey(left))
        {
            rb2d_player.AddForce(transform.right * -velocity * Time.deltaTime);            
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
            rb2d_player.AddForce(transform.right * -velocity * Time.deltaTime);
        }
        if(rb2d_player.velocity.x < -block)
        {
            rb2d_player.AddForce(transform.right * velocity * Time.deltaTime);
        }        
    }    
}
