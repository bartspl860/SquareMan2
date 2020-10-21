using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [SerializeField] private Transform t_player;
    [SerializeField] private BoxCollider2D b2d_player;
    [SerializeField] private LayerMask hurt;
    public Movement movement;
    void FixedUpdate()
    {
        
        if (t_player.position.y < -10 || Input.GetKey("escape"))
        {
            t_player.position = new Vector3(0f, 1.7f, 0);
        }

        //collision with obstacle
        if (b2d_player.IsTouchingLayers(hurt))
        {
            t_player.position = new Vector3(0f, 1.7f, 0);
        }

        
    }
}
