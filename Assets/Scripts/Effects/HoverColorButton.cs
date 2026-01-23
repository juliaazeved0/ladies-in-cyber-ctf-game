using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

public class JuicyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configurações")]
    public float scaleOnClick = 0.9f; 
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f, 1f); 

    private Vector3 originalScale;
    private Color originalColor;
    private Image buttonImage;

    void Start()
    {
        
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        
        if (buttonImage != null)
            originalColor = buttonImage.color;
    }

  
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.color = hoverColor; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.color = originalColor; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector3(scaleOnClick, scaleOnClick, 1f); // Diminui
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale; 
    }
}