using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

/// <summary>
/// Adiciona efeitos de polimento aos botoes, como uma mudanca de cor no Hover
/// e alteracao de escala no clique, utilizando as interfaces de evento do EventSystem.
/// </summary>
public class JuicyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [Tooltip("A escala do botao quando pressionado.")]
    public float scaleOnClick = 0.9f;

    [Tooltip("A cor do botao quando o mouse passa por cima.")]
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f, 1f); 

    private Vector3 originalScale;
    private Color originalColor;
    private Image buttonImage;

    void Start()
    {
        //Salva o estado inicial para poder retornar a ele depois
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        
        if(buttonImage != null)
            originalColor = buttonImage.color;
    }

    /// <summary>
    /// Evento chamado quando o cursor entra na area do botao (hover).
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(buttonImage != null)
            buttonImage.color = hoverColor; 
    }

    /// <summary>
    /// Evento chamado quando o cursor sai da area do botao.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if(buttonImage != null)
            buttonImage.color = originalColor; 
    }

    /// <summary>
    /// Evento chamado quando o botao do mouse eh pressionado sobre o objeto.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        //Feedback visual de "apertar"
        transform.localScale = new Vector3(scaleOnClick, scaleOnClick, 1f);
    }

    /// <summary>
    /// Evento chamado quando o botao do mouse eh solto.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale; 
    }
}