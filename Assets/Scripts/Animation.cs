using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Animation : MonoBehaviour
{

    private string _turn = "right";
    [FormerlySerializedAs("sr_player")] public SpriteRenderer srPlayer;
    [FormerlySerializedAs("PlayerSprite")] public Sprite playerSprite;
    public Movement movement;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(movement.left)){
            _turn = "left";
        }
        if (Input.GetKey(movement.right))
        {
            _turn = "right";
        }

        if(_turn == "right")
        {
            srPlayer.flipX = false;
        }
        else
        {
            srPlayer.flipX = true;
        }
    }
}
