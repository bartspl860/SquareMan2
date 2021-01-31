using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    private string turn = "right";
    public SpriteRenderer sr_player;
    public Sprite PlayerSprite;
    public Movement movement;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(movement.left)){
            turn = "left";
        }
        if (Input.GetKey(movement.right))
        {
            turn = "right";
        }

        if(turn == "right")
        {
            sr_player.flipX = false;
        }
        else
        {
            sr_player.flipX = true;
        }
    }
}
