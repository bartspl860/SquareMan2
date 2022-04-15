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
    [SerializeField]
    private SpriteRenderer timeStopCircle;
    public SpriteRenderer TimeStopCircle { get => timeStopCircle;}

    [Header("Variables")]
    [SerializeField]
    private float baseMoveForce;    
    private float moveforce;
    public float MoveForce
    {
        get => moveforce;
        private set
        {
            if (value < 0f)
                moveforce = 0f;
            else
                moveforce = value;

        }
    }
    [SerializeField]
    private float baseMaxVelocity;    
    private float maxvelocity;
    public float MaxVelocity
    {
        get => maxvelocity;
        private set
        {
            if (value < 0f)
                maxvelocity = 0f;
            else
                maxvelocity = value;
        }
    }
    [SerializeField]
    private float baseJumpForce;    
    private float jumpforce;
    public float JumpForce
    {
        get => jumpforce;
        private set
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

    public void StopPlayer()
    {
        if(GroundChecker.IsTouchingLayers(GroundLayer))
            Rigidbody.velocity = Vector2.zero;
    }

    private AbilitiesController allAbilities = new AbilitiesController(4); 
    public AbilitiesController AllAbilities { get => allAbilities; }

    public void Initialize()
    {
        JumpForce = baseJumpForce;
        MaxVelocity = baseMaxVelocity;
        MoveForce = baseMoveForce;

        allAbilities.AddActive(HighJump, 1);
        allAbilities.AddReverse(ReverseHighJump, 1);
        allAbilities.AddActive(UpsideDown, 0);
        allAbilities.AddReverse(ReverseUpsideDown, 0);
        allAbilities.AddActive(Sprint, 2);
        allAbilities.AddReverse(SprintOFF, 2);
        allAbilities.AddActive(TimeStop, 3);
        allAbilities.AddReverse(TimeStopOFF, 3);
    }

    //ID0
    public void UpsideDown()
    {
        Rigidbody.gravityScale *= -1;
        Transform.rotation = Quaternion.Euler(Rigidbody.gravityScale > 0 ? 0f : 180f, 0f, 0f);
    }
    public void ReverseUpsideDown()
    {

    }
    //ID1
    public void HighJump()
    {
        JumpForce = 900f;
    }
    public void ReverseHighJump()
    {
        JumpForce = baseJumpForce;
    }
    //ID2
    public void Sprint()
    {
        MoveForce = 1200f;
        MaxVelocity = 8f;
    }
    public void SprintOFF()
    {
        MoveForce = baseMoveForce;
        MaxVelocity = baseMaxVelocity;
    }
    //ID3
    private int counter = 0;
    IEnumerator TimeStopCircleGettingBigger(bool bigger = true)
    {
        int end = 350;

        if (bigger)
        {
            TimeStopCircle.enabled = true;
            while (counter < end)
            {
                TimeStopCircle.size = new Vector2(counter / 10f, counter / 10f);
                yield return new WaitForSecondsRealtime(0.01f);
                counter++;
            }
        }
        else
        {
            while (counter > 0)
            {
                TimeStopCircle.size = new Vector2(counter / 10f, counter / 10f);
                yield return new WaitForSecondsRealtime(0.005f);
                counter-=2;
            }
            counter = 0;
            TimeStopCircle.enabled = false;
        }        
    }
    Coroutine ienumerator_obj;
    public void TimeStop()
    {
        AudioManager.instance.PlaySound("TimeStopStart");
        ienumerator_obj = StartCoroutine(TimeStopCircleGettingBigger());
    }
    public void TimeStopOFF()
    {
        AudioManager.instance.StopSound("TimeStopStart");
        AudioManager.instance.PlaySound("TimeStopEnd");
        StopCoroutine(ienumerator_obj);
        StartCoroutine(TimeStopCircleGettingBigger(false));
    }
}

public class AbilitiesController
{    
    private CircledMenuItem activeAction;
    public CircledMenuItem ActiveAction { get => activeAction; set => activeAction = value; }
    private Action[] activeAbilities;
    private Action[] nonActiveAbilities;
    public void DoAction() {
        if(activeAction != null)
        {
            activeAbilities[ActiveAction.AbilityID]();
        }        
    }
    public void ReverseAction()
    {
        if(activeAction != null)
            nonActiveAbilities[activeAction.AbilityID]();
    }
    public AbilitiesController(int number)
    {
        activeAbilities = new Action[number];
        nonActiveAbilities = new Action[number];
    }
    public void AddActive(Action action, int id)
    {
        activeAbilities[id] += action;
    }
    public void AddReverse(Action action, int id)
    {
        nonActiveAbilities[id] += action;
    }    
}
