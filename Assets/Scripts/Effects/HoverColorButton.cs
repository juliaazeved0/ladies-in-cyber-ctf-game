using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

/// <summary>
/// Adiciona feedback visual a um botao de UI: muda a cor ao passar
/// o mouse por cima e reduz levemente a escala ao ser pressionado,
/// simulando um efeito tatil de clique.
/// </summary>
public class JuicyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [Tooltip("A escala do botao quando pressionado.")]
    public float scaleOnClick = 0.9f;

    [Tooltip("A cor do botao quando o mouse passa por cima.")]
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f, 1f); 

    //Guarda os valores originais para restaurar corretamente apo o clique terminar
    private Vector3 originalScale;
    private Color originalColor;
    private Image buttonImage;

    void Start()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        
        if(buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
        else
        {
            //Avisa caso o objeti nao tenha um Imagem, ja que sem ele o efeito de hover nao tera efeito nenhum
            Debug.LogWarning($"{gameObject.name} não possui um componente Image. O efeito de hover não será aplicado!");
        }
    }

    //Chamado quando o mouse entra na area do botao
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(buttonImage != null)
            buttonImage.color = hoverColor; 
    }

    //Chamado quando o mouse sai da area do botao
    public void OnPointerExit(PointerEventData eventData)
    {
        if(buttonImage != null)
            buttonImage.color = originalColor; 
    }

    //Chamado quando o botao do mouse eh pressionado sobre esse objeto
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector3(scaleOnClick, scaleOnClick, 1f);
    }

    //Chamado quando o botao do mouse eh solto
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale; 
    }
}