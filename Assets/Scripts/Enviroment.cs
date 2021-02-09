using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Platform
{
    public String Name;
    public Vector2 Start;
    public Vector2 End;
    public String HorizontalTurn;
    public String VerticalTurn;
    public float Speed;
    public Transform PlatformTransform;
    public Platform(
        String name, 
        String horizontalTurn,
        String verticalTurn,
        Transform platformTransform, 
        Vector2 start, 
        Vector2 end, 
        float speed
        )
    {
        Name = name;
        Start = start;
        End = end;
        Speed = speed;
        HorizontalTurn = horizontalTurn;
        VerticalTurn = verticalTurn;
        PlatformTransform = platformTransform;
    }
}
public class Enviroment : MonoBehaviour
{
    [SerializeField] private Transform t_player;
    [SerializeField] private BoxCollider2D b2d_player;
    [SerializeField] private LayerMask hurt;
    [SerializeField] private LayerMask Teleport;
    [SerializeField] private GameObject TeleportInfo;
    public Movement movement;
    public Animation animation;
    public CameraMove CameraMove;

    [SerializeField] List<Vector3> teleportCordinates;
    private bool disablePressE = false;
    [SerializeField] private GameObject teleportButton_prefab;
    [SerializeField] private GameObject teleportMenuObj;
    private List<Transform> rotatingSpikesTransforms = new List<Transform>();
    public int rotationSpeed;

    private int rotationHandler = 0;

    [SerializeField] private SpriteRenderer timeStopEffect;
    private float timeStopEffetHandler = 0f;
    
    List<Platform> Platforms = new List<Platform>();
    [SerializeField] private GameObject PlatformObj;

    [Header("Platforms Info")] 
    [SerializeField] private String[] Name;
    [SerializeField] private Vector2[] StartCords;
    [SerializeField] private Vector2[] EndCords;
    [SerializeField] private float[] Speed;
    



    private void Start()
    {
        GameObject[] RotatingSpikes = GameObject.FindGameObjectsWithTag("RotatingSpikes");
        foreach (GameObject v in RotatingSpikes)
        {
            rotatingSpikesTransforms.Add(v.transform);
        }

        //platform feature
        bool condition = Name.Length != StartCords.Length ||
                         Name.Length != EndCords.Length ||
                         Name.Length != Speed.Length;

        if (condition)
        {
            Debug.LogError("Wrong amount of platform elements!");
            Time.timeScale = 0f;
        }
        
        for (int i = 0; i < Name.Length; i++)
        {
            GameObject temp = Instantiate(PlatformObj);

            temp.GetComponent<Transform>().position = new Vector3(StartCords[i].x,StartCords[i].y,0f);
            temp.GetComponentInChildren<Canvas>();
            temp.GetComponentInChildren<GameObject>().GetComponentInChildren<TextMeshPro>().text = Name[i];

            Platform platform =
                new Platform(
                    Name[i],
                    "Right",
                    "Up",
                    temp.GetComponent<Transform>(),
                    StartCords[i], 
                    EndCords[i], 
                    Speed[i]
                );
            Platforms.Add(platform);
        }
        /*
        GameObject[] MovingPlatforms = GameObject.FindGameObjectsWithTag("MovingGround");
        foreach (GameObject v in MovingPlatforms)
        {
            
        }*/
    }

    void FixedUpdate()
    {
        
        if (t_player.position.y < -10 || Input.GetKey("escape"))
        {
            t_player.position = new Vector3(0f, 1.7f, 0);
        }

        //collision with obstacle
        if (b2d_player.IsTouchingLayers(hurt))
        {
            t_player.position = new Vector3(0f, 1.7f, 0f);
        }

        if (b2d_player.IsTouchingLayers(Teleport))//is near teleport
        {
            if(!disablePressE) TeleportInfo.SetActive(true);
            
            if (Input.GetKey(KeyCode.E))
            {
                TeleportInfo.SetActive(false);
                disablePressE = true;
                teleportMenu();
            }
        }
        else
        {
            TeleportInfo.SetActive(false);
            disablePressE = false;
            animation.sr_player.enabled = true;
        }
    
        //spikes rotation
        foreach (Transform v in rotatingSpikesTransforms)
        {
            v.eulerAngles = new Vector3(0f, 0f, -rotationHandler*Time.fixedDeltaTime);
            rotationHandler+=rotationSpeed;
        }
        
        //time stop effect
        if (Input.GetKey(movement.action) && movement.eq_control == 4)
        {
            timeStopEffect.enabled = true;
            if(timeStopEffetHandler<100f) timeStopEffetHandler += 1.1f;
            timeStopEffect.size = new Vector2(timeStopEffetHandler, timeStopEffetHandler);
        }
        else
        {
            timeStopEffetHandler = 0f;
            timeStopEffect.enabled = false;
        }
        
        //moving platform
        foreach (Platform var in Platforms)
        {
            if (var.Start.x != var.End.x)
            {
                if (var.PlatformTransform.position.x >= var.End.x)
                {
                    var.HorizontalTurn = "Left";
                }
                else if(var.PlatformTransform.position.x <= var.Start.x)
                {
                    var.HorizontalTurn = "Right";
                }
       
                if (var.HorizontalTurn == "Right")
                {
                    var.PlatformTransform.position = new Vector2(var.PlatformTransform.position.x+(var.Speed*Time.fixedDeltaTime), 
                        var.PlatformTransform.position.y);
                }
                else
                {
                    var.PlatformTransform.position = new Vector2(var.PlatformTransform.position.x-(var.Speed*Time.fixedDeltaTime), 
                        var.PlatformTransform.position.y);
                }
            }

            if (var.Start.y != var.End.y)
            {
                if (var.PlatformTransform.position.y >= var.End.y)
                {
                    var.VerticalTurn = "Down";
                }
                else if(var.PlatformTransform.position.y <= var.Start.y)
                {
                    var.VerticalTurn = "Up";
                }
       
                if (var.VerticalTurn == "Up")
                {
                    var.PlatformTransform.position = new Vector2(var.PlatformTransform.position.x, 
                        var.PlatformTransform.position.y+(var.Speed*Time.fixedDeltaTime));
                }
                else
                {
                    var.PlatformTransform.position = new Vector2(var.PlatformTransform.position.x, 
                        var.PlatformTransform.position.y-(var.Speed*Time.fixedDeltaTime));
                }
            }
        }
    }
    private void teleportMenu()
    {
        Time.timeScale = 0f;
        animation.sr_player.enabled = false;
        teleportMenuObj.SetActive(true);
    }

    private void tpTo(Vector3 cordinates)
    {
        teleportMenuObj.SetActive(false);
        Time.timeScale = 1f;
        animation.sr_player.enabled = true;
        disablePressE = false;
        movement.t_player.position = cordinates;
        CameraMove.transform.position = cordinates - new Vector3(0f,0f,10f);
    }

    public void tpGunLVL()
    {
        tpTo(teleportCordinates[1]);
    }

    public void tpStartPlatform()
    {
        tpTo(teleportCordinates[0]);
    }
}
