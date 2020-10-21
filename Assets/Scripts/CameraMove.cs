using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera main_camera;
    [SerializeField] private Transform t_player;
    [SerializeField] private Transform t_camera;
    [SerializeField] private Rigidbody2D rb2d_camera;


    void Start()
    {
        
    }
    void FixedUpdate()
    {
        float distance = Vector3.Distance(t_camera.position, t_player.position);
        float speed = distance * 10;
        Vector3 dir = t_player.position - t_camera.position;
        rb2d_camera.velocity = dir * speed * Time.deltaTime;

        float delta = Input.GetAxisRaw("Mouse ScrollWheel");

        main_camera.orthographicSize += delta * -10;
        main_camera.orthographicSize = Mathf.Clamp(main_camera.orthographicSize, 2f, 10f);

    }
}
