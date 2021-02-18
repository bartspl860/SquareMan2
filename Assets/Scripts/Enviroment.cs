using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

[System.Serializable]
public class Platform
{
    [FormerlySerializedAs("Name")] public String name;
    [FormerlySerializedAs("Start")] public Vector2 start;
    [FormerlySerializedAs("End")] public Vector2 end;
    [FormerlySerializedAs("HorizontalTurn")] [HideInInspector]public String horizontalTurn;
    [FormerlySerializedAs("VerticalTurn")] [HideInInspector]public String verticalTurn;
    [FormerlySerializedAs("Speed")] public float speed;
    [FormerlySerializedAs("PlatformRigidbody2D")] [HideInInspector] public Rigidbody2D platformRigidbody2D;
    [FormerlySerializedAs("PlatformTransform")] [HideInInspector] public Transform platformTransform;
    public Platform(
        String name, 
        String horizontalTurn,
        String verticalTurn,
        Rigidbody2D platformRigidbody2D,
        Transform platformTransform,
        Vector2 start, 
        Vector2 end, 
        float speed
    )
    {
        this.name = name;
        this.start = start;
        this.end = end;
        this.speed = speed;
        this.horizontalTurn = horizontalTurn;
        this.verticalTurn = verticalTurn;
        this.platformRigidbody2D = platformRigidbody2D;
        this.platformTransform = platformTransform;
    }
}
public class Enviroment : MonoBehaviour
{
    [FormerlySerializedAs("t_player")] [SerializeField] private Transform tPlayer;
    [FormerlySerializedAs("b2d_player")] [SerializeField] private BoxCollider2D b2dPlayer;
    [SerializeField] private LayerMask hurt;
    [FormerlySerializedAs("Teleport")] [SerializeField] private LayerMask teleport;
    [FormerlySerializedAs("TeleportInfo")] [SerializeField] private GameObject teleportInfo;
    public Movement movement;
    public Animation animation;
    [FormerlySerializedAs("CameraMove")] public CameraMove cameraMove;

    [SerializeField] List<Vector3> teleportCordinates;
    [FormerlySerializedAs("teleportButton_prefab")] [SerializeField] private GameObject teleportButtonPrefab;
    [SerializeField] private GameObject teleportMenuObj;
    private List<Transform> _rotatingSpikesTransforms = new List<Transform>();
    public int rotationSpeed;

    private int _rotationHandler = 0;

    [SerializeField] private SpriteRenderer timeStopEffect;
    private float _timeStopEffetHandler = 0f;
    
    
    [Header("Platforms Info")]
    [SerializeField]
    List<Platform> platforms = new List<Platform>();
    [SerializeField] private GameObject platformObj;
    
    [Header("Respawn")]
    //Respawn
    [SerializeField] private Transform respawn;

    [Header("Audio")] 
    public AudioHandler audioHandler;
    //---------------------------------------------
    public AudioSource envAudioSource;
    public AudioSource audioSource;
    public AudioSource skillAudioSource;
    public AudioClip damageClip;
    public AudioClip stopTime;
    public AudioClip stopTimeAfter;
    public AudioClip menuNavigateClip;
    public AudioClip teleportClip;


    [Header("End Lvl")] 
    [SerializeField] private GameObject finish;

    private Collider2D finishCol2D;
    private Transform finishTr;


    private void Start()
    {
        //finish reference
        finishCol2D = finish.GetComponent<Collider2D>();
        finishTr = finish.GetComponent<Transform>();
        
        //time effect turn on
        timeStopEffect.enabled = true;
        
        //rotating spikes
        GameObject[] rotatingSpikes = GameObject.FindGameObjectsWithTag("RotatingSpikes");
        foreach (GameObject v in rotatingSpikes)
        {
            _rotatingSpikesTransforms.Add(v.transform);
        }

        //platform feature

        foreach (Platform var in platforms)
        {
            GameObject temp = Instantiate(platformObj);
            
            temp.GetComponent<Transform>().position = new Vector3(var.start.x, var.start.y, 0f);
            var.platformRigidbody2D = temp.GetComponent<Rigidbody2D>();
            var.platformTransform = temp.GetComponent<Transform>();

            temp.GetComponentInChildren<TMP_Text>().text = var.name; 
        }

    }

    private int rotationCoin = 0;
    void FixedUpdate()
    {
        
        //coin floating
        finishTr.eulerAngles = new Vector3(0f, ++rotationCoin, 0f);
        
        
        //collision with obstacle
        if (b2dPlayer.IsTouchingLayers(hurt) || (tPlayer.position.y < -10 || Input.GetKey("escape")))
        {
            audioHandler.StartAudioClip(envAudioSource,damageClip);
            tPlayer.position = respawn.position;
        }

        //teleports
        if (b2dPlayer.IsTouchingLayers(teleport))//is near teleport
        {
            if(!Input.GetKey(KeyCode.E)) teleportInfo.SetActive(true);
            else
            {
                teleportInfo.SetActive(false);
                audioHandler.StartAudioClip(envAudioSource,teleportClip);
                TeleportMenu();
            }
        }
        else
        {
            teleportInfo.SetActive(false);
            animation.srPlayer.enabled = true;
        }
    
        //spikes rotation
        foreach (Transform v in _rotatingSpikesTransforms)
        {
            v.eulerAngles = new Vector3(0f, 0f, -_rotationHandler*Time.fixedDeltaTime);
            _rotationHandler+=rotationSpeed;
        }
        
        //time stop effect
        if (Input.GetKey(movement.action) && movement.eqControl == 4)
        {
            
            if(_timeStopEffetHandler<100f) _timeStopEffetHandler += 1.1f;
            timeStopEffect.size = new Vector2(_timeStopEffetHandler, _timeStopEffetHandler);
        }
        else
        {
            if (_timeStopEffetHandler > 3f) _timeStopEffetHandler -= 3.1f;
            else _timeStopEffetHandler = 0f;
            timeStopEffect.size = new Vector2(_timeStopEffetHandler, _timeStopEffetHandler);
        }

        
        //moving platform
        foreach (Platform var in platforms)
        {
            Vector2 platformTPos = var.platformTransform.position;


            float actualSpeed = var.speed;
            if (Input.GetKey(movement.action) && movement.eqControl == 4)
            {
                actualSpeed = var.speed * 0.25f;
            }
            
            
            if ((int)var.start.y == (int)var.end.y)
            {
                if (var.horizontalTurn == "Right")
                {
                    var.platformRigidbody2D.MovePosition(new Vector2(
                        var.platformTransform.position.x+actualSpeed*Time.fixedDeltaTime,
                        platformTPos.y));
                }
                else
                {
                    var.platformRigidbody2D.MovePosition(new Vector2(
                        var.platformTransform.position.x-actualSpeed*Time.fixedDeltaTime,
                        platformTPos.y));
                }
                if (var.platformTransform.position.x > var.end.x)
                {
                    var.horizontalTurn = "Left";
                }
                else if (var.platformTransform.position.x < var.start.x)
                {
                    var.horizontalTurn = "Right";
                }
            }
            if ((int)var.start.x == (int)var.end.x)
            {
                if (var.horizontalTurn == "Up")
                {
                    var.platformRigidbody2D.MovePosition(new Vector2(
                        var.platformTransform.position.x,
                        platformTPos.y+actualSpeed*Time.fixedDeltaTime));
                }
                else
                {
                    var.platformRigidbody2D.MovePosition(new Vector2(
                        var.platformTransform.position.x,
                        platformTPos.y-actualSpeed*Time.fixedDeltaTime));
                }
                if (var.platformTransform.position.y > var.end.y)
                {
                    var.horizontalTurn = "Down";
                }
                else if (var.platformTransform.position.y < var.start.y)
                {
                    var.horizontalTurn = "Up";
                }
            }
            
            
        }
    }
    private void TeleportMenu()
    {
        Time.timeScale = 0f;
        animation.srPlayer.enabled = false;
        teleportMenuObj.SetActive(true);
    }

    private void TpTo(Vector3 cordinates)
    {
        teleportMenuObj.SetActive(false);
        Time.timeScale = 1f;
        animation.srPlayer.enabled = true;
        movement.tPlayer.position = cordinates;
        cameraMove.transform.position = cordinates - new Vector3(0f,0f,10f);
        audioHandler.StartAudioClip(envAudioSource,teleportClip);
    }

    public void TpObstaclePlatform()
    {
        TpTo(teleportCordinates[1]);
    }

    public void TpStartPlatform()
    {
        TpTo(teleportCordinates[0]);
    }

    public void MenuClickClip()
    {
        audioHandler.StartAudioClip(envAudioSource,menuNavigateClip);
    }
}
