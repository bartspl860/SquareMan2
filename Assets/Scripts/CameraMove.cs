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
    void FixedUpdate()
    {
        //camera movement
        var playerPosition = tPlayer.position;
        var cameraPosition = tCamera.position;
        
        float distance = Vector3.Distance(cameraPosition, playerPosition);
        float speed = distance * 10f;
        Vector3 dir = playerPosition - cameraPosition;
        rb2dCamera.velocity = dir * (speed * Time.deltaTime);

        
    }
    private void Update()
    {
        //camera zoom
        float delta = Input.GetAxisRaw("Mouse ScrollWheel");
        var orthographicSize = mainCamera.orthographicSize;
        orthographicSize += delta * -10f;
        mainCamera.orthographicSize = orthographicSize;
        mainCamera.orthographicSize = Mathf.Clamp(orthographicSize, 2f, 10f);
    }
}
