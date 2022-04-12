using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{   
    [Header("Functional")]
    [SerializeField] 
    private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody { get => rigidbody; private set => rigidbody = value; }
    [SerializeField] 
    private Transform transform;
    public Transform Transform { get => transform; private set => transform = value; }
    [SerializeField]
    private BoxCollider2D groundChecker;
    public BoxCollider2D GroundChecker { get => groundChecker; private set => groundChecker = value; }
    [SerializeField]
    private LayerMask groundLayer;
    public LayerMask GroundLayer { get => groundLayer; private set => groundLayer = value; }
    [Header("Appearance")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; private set => spriteRenderer = value; }
    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }
    
    [Header("Variables")]
    [SerializeField]
    private float moveforce;
    public float MoveForce
    {
        get => moveforce;
        set
        {
            if (value < 0f)
                moveforce = 0f;
            else
                moveforce = value;

        }
    }
    [SerializeField]
    private float maxvelocity;
    private float MaxVelocity
    {
        get => maxvelocity;
        set
        {
            if (value < 0f)
                maxvelocity = 0f;
            else
                maxvelocity = value;
        }
    }
    [SerializeField]
    private float jumpforce;
    public float JumpForce
    {
        get => jumpforce;
        set
        {
            if (value < 0f)
                jumpforce = 0f;
            else
                jumpforce = value;
        }
    }    
    public string Name { get; set; }

    //Methodes/Properties
    public void GoRight(out bool result)
    {
        this.SpriteRenderer.flipX = false;
        var dir = transform.right * (moveforce * Time.deltaTime);
        Rigidbody.AddForce(dir);
        if (Velocity > maxvelocity)
        {
            Rigidbody.AddForce(-dir);
            result = false;
        }
        result = true;
    }
    public void GoLeft(out bool result)
    {
        this.SpriteRenderer.flipX = true;
        var dir = transform.right * (-moveforce * Time.deltaTime);
        Rigidbody.AddForce(dir);
        if (Velocity > maxvelocity)
        {
            Rigidbody.AddForce(-dir);
            result = false;
        }
        result = true;
    }
    public void Jump(out bool result)
    {
        if (GroundChecker.IsTouchingLayers(GroundLayer))
        {
            var dir = transform.up * (JumpForce * Time.deltaTime);
            Rigidbody.AddForce(dir, ForceMode2D.Impulse);
            AudioManager.instance.PlaySound("Jump");
            result = true;
        }
        result = false;
    }
    public float HorizontalVelocity => Rigidbody.velocity.x;
    public float VerticalVelocity => Rigidbody.velocity.y;
    public float Velocity => (float)Math.Sqrt(Math.Pow(Rigidbody.velocity.x, 2) + Math.Pow(Rigidbody.velocity.y, 2));
}
