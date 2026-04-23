using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;  // ADD THIS

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    
    private Image buttonImage;
    private Button button;
    private bool isHovered = false;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        buttonImage.sprite = normalSprite;
    }
    
    void Update()
    {
        // Check if left mouse button is pressed using new Input System
        bool isLeftMousePressed = Mouse.current != null && Mouse.current.leftButton.isPressed;
        bool isPressed = (button != null && button.interactable && 
                          isLeftMousePressed && isHovered);
        
        if (isPressed)
        {
            if (buttonImage.sprite != hoverSprite)
                buttonImage.sprite = hoverSprite;
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else if (isHovered)
        {
            if (buttonImage.sprite != hoverSprite)
                buttonImage.sprite = hoverSprite;
            transform.localScale = Vector3.one;
        }
        else
        {
            if (buttonImage.sprite != normalSprite)
                buttonImage.sprite = normalSprite;
            transform.localScale = Vector3.one;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayNewButton();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.name == "Btn_Play")
        {
            ThemePopupCreator popup = FindAnyObjectByType<ThemePopupCreator>();
            if (popup != null)
                popup.ShowPopup();
        }
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}