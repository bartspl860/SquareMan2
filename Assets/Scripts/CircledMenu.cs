using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CircledMenu : MonoBehaviour
{
    [Header("Functional")]
    [SerializeField]
    private Canvas content;
    public Canvas Content { get => content; set => content = value; }
    [SerializeField]
    private CanvasRenderer background;
    public CanvasRenderer Background { get => background; set => background = value; }
    [SerializeField]
    private GameObject dividerPrefab;
    public GameObject DividerPrefab { get => dividerPrefab; set => dividerPrefab = value; }
    [SerializeField]
    private GameObject menuElementPrefab;
    public GameObject MenuElementPrefab { get => menuElementPrefab; set => menuElementPrefab = value; }



    //private for methodes
    [SerializeField]
    private CircledMenuItem[] items;
    //private Vector2 moveInput = new Vector2();
    

    //Methodes/Properties

    private bool active = false;
    public bool Active 
    { 
        get => active;
        set
        {
            active = value;
            Content.gameObject.SetActive(value);
        }
    }
    public void Initialize()
    {
        /*
        moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
        moveInput.y = Input.mousePosition.y - (Screen.height / 2f);
        moveInput.Normalize();

        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
            angle *= 180f;
            if (angle < 0)
            {
                angle += 360f;
            }

            Debug.Log(angle);
        }
        */

        const float RADIAN = 0.0174533f;
        for (int i = 0; i < items.Length; i++)
        {
            var angle = i * 360f / items.Length;

            var menuElement = Instantiate(MenuElementPrefab, Background.transform);
            var menuElementRect = menuElement.GetComponent<RectTransform>();

            if (items.Length > 1)
            {
                var positionFix = (items.Length % 2 == 0) ? (360f / items.Length / 2f) : (360f / items.Length);
                menuElementRect.anchoredPosition =
                    new Vector2(
                        100f * (float)Math.Cos((angle - positionFix) * RADIAN),
                        100f * (float)Math.Sin((angle - positionFix) * RADIAN)
                        );

                var divider = Instantiate(DividerPrefab, Background.transform);
                divider.transform.Rotate(0f, 0f, angle, Space.Self);
            }
            else
            {
                menuElementRect.anchoredPosition = Vector2.zero;
            }
            
            menuElementRect.sizeDelta = new Vector2(
                (float)(items[i].Width * 1f/Math.Log(items.Length+3f, 3f) + 6f) * 2f,
                (float)(items[i].Height * 1f/Math.Log(items.Length+3f, 3f) + 6f) * 2f
                );

            var menuElementImage = menuElementRect.GetComponent<Image>();
            menuElementImage.sprite = items[i].Icon;
            menuElementImage.color = Color.white;          
        }        
    }
}

[Serializable]
public class CircledMenuItem
{
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get => icon; set => icon = value; }
    public float Width { get => icon.rect.width; }
    public float Height { get => icon.rect.height; }
    public string Name { get => icon.name; }
    public override string ToString()
    {
        return $"{Name} {Width}px x {Height}px";
    }
}
