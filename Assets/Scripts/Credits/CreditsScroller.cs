using UnityEngine;

/// <summary>
/// Rola verticalmente o painel de credito ate uma posicao limite,
/// travando-o no lugar quando o final do conteudo eh alcancado.
/// </summary>
public class CreditsScroller : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("RectTransform do painel que contem o texto de creditos a ser rolado.")]
    public RectTransform creditsRect;

    [Header("Scroll Settings")]
    [Tooltip("Velocidade da rolagem dos creditos, em unidades por segundo.")]
    public float scrollSpeed = 100f;

    [Tooltip("Posicao Y (calculada no Start) em que a rolagem deve parar.")]
    public float stopPositionY = 1500f; //Valor padrao sobrescrito no Start com base na altura real do conteudo da tela

    private bool canScroll = true; //Trava a rolagem assim que o limite eh atingido

    void Start()
    {
        //Evita erro de referencia caso nao tenha sido arrastada no Inspector
        if(creditsRect == null)
        {
            Debug.LogError($"{gameObject.name} está sem referência ao RectTransform de créditos!");
            canScroll = false;
            return;
        }

        //Calcula ate onde o conteudo pode subir
        stopPositionY = creditsRect.rect.height - Screen.height;
    }
    void Update()
    {
        if(canScroll)
        {
            //Move o painel para cima continuamente
            creditsRect.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

            if(creditsRect.anchoredPosition.y >= stopPositionY)
            {
                //Trava exatamente na posicao limite, evitando ultrapassar por causa do incremento do frame anterior
                creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, stopPositionY);
                canScroll = false;
                Debug.Log("Créditos finalizados e mantidos.");
            }
        }
    }
}