using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform tPlayer;
    [SerializeField] private Transform tCamera;
    [SerializeField] private Rigidbody2D rb2dCamera;


    void Start()
    {
        
    }
    void FixedUpdate()
    {
        //camera movement
        float distance = Vector3.Distance(tCamera.position, tPlayer.position);
        float speed = distance * 10f;
        Vector3 dir = tPlayer.position - tCamera.position;
        rb2dCamera.velocity = dir * (speed * Time.deltaTime);


        //camera zoom
        float delta = Input.GetAxisRaw("Mouse ScrollWheel");
        mainCamera.orthographicSize += delta * -50f;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 10f);
    }
}
