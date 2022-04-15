using System;
using UnityEngine;
using UnityEngine.UI;
using Audio;

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
    private Vector2 moveInput = new Vector2();
    private float elementAngle = 0f;
    public CircledMenuItem ChooseElement()
    {
        CircledMenuItem result = null;
        float angle = 0f;
        moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
        moveInput.y = Input.mousePosition.y - (Screen.height / 2f);
        moveInput.Normalize();

        if (Vector2.Distance(new Vector2(Screen.width / 2f, Screen.height / 2f), Input.mousePosition) > Screen.height / 3f)
            return result;           

        if (moveInput != Vector2.zero)
        {
            angle = Mathf.Atan2(moveInput.y, moveInput.x) / Mathf.PI;
            angle *= 180f;
            if (angle < 0) angle += 360f;            
        }

        var elem_id = (int)Math.Floor(angle / elementAngle);

        for (int i = 0; i < dividersRef.Length; i++)
        {
            if(i == elem_id || i == (elem_id + 1) % dividersRef.Length)
            {
                dividersRef[i].color = Color.red;
            }
            else
            {
                dividersRef[i].color = Color.white;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            result = items[elem_id];
            AudioManager.instance.PlaySound("Navigate1");
        }  
        
        return result;
    }
    private Image[] dividersRef;
    public void Initialize()
    {
        dividersRef = new Image[items.Length];
        elementAngle = 360f / items.Length;
        const float RADIAN = 0.0174533f;

        for (int i = 0; i < items.Length; i++)
        {
            var angle = i * elementAngle;
            var menuElement = Instantiate(MenuElementPrefab, Background.transform);
            var menuElementRect = menuElement.GetComponent<RectTransform>();

            if (items.Length > 1)
            {
                var positionFix = elementAngle / 2f;
                menuElementRect.anchoredPosition =
                    new Vector2(
                        100f * (float)Math.Cos((angle + positionFix) * RADIAN),
                        100f * (float)Math.Sin((angle + positionFix) * RADIAN)
                        );

                var divider = Instantiate(DividerPrefab, Background.transform);
                divider.transform.Rotate(0f, 0f, angle, Space.Self);
                dividersRef[i] = divider.GetComponent<Image>();
            }
            else
            {
                menuElementRect.anchoredPosition = Vector2.zero;
            }
            menuElementRect.sizeDelta = new Vector2(
                (float)(items[i].Width * 1f / Math.Log(items.Length + 3f, 3f) + 6f) * 2f,
                (float)(items[i].Height * 1f / Math.Log(items.Length + 3f, 3f) + 6f) * 2f
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
    private int abilityID;
    public int AbilityID { get => abilityID; }
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get => icon; set => icon = value; }
    public float Width { get => icon.rect.width; }
    public float Height { get => icon.rect.height; }
    [SerializeField]
    private string name;
    public string Name { get => name; set => name = value; }
    [SerializeField]
    private string description;
    public string Description { get; set; }
    public override string ToString()
    {
        return $"{Name} {Width}px x {Height}px";
    }
    [SerializeField]
    private bool holdAbility;
    public bool HoldAbility { get => holdAbility; }
}
