using UnityEngine;

/// <summary>
/// Rola verticalmente o painel de créditos até o final do conteúdo.
/// </summary>
public class CreditsScroller : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("RectTransform do painel que contém a imagem dos créditos.")]
    [SerializeField] private RectTransform creditsRect;

    [Header("Scroll Settings")]
    [Tooltip("Velocidade da rolagem dos créditos.")]
    [SerializeField] private float scrollSpeed = 100f;

    [Header("State Control")]
    [Tooltip("Define se os créditos estão rolando.")]
    [SerializeField] private bool canScroll = true;

    private float stopPositionY;

    private void Start()
    {
        //Tratamento de erro
        if(creditsRect == null)
        {
            Debug.LogError(
                $"{gameObject.name} está sem referência ao RectTransform de créditos!"
            );

            canScroll = false;
            return;
        }

        CalculateScrollPositions();
    }

    private void Update()
    {
        if(!canScroll) return;

        //Move os creditos para cima
        creditsRect.anchoredPosition += Vector2.up * (scrollSpeed * Time.deltaTime);

        //Verifica se chegou ao final
        if(creditsRect.anchoredPosition.y >= stopPositionY)
        {
            //Garante que pare exatamente no final
            creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, stopPositionY);

            canScroll = false;

            Debug.Log("Fim da rolagem atingido!");
        }
    }

    //Calcula a posicao inicial e final da rolagem
    private void CalculateScrollPositions()
    {
        Canvas parentCanvas = creditsRect.GetComponentInParent<Canvas>();

        if(parentCanvas == null)
        {
            Debug.LogError("Não foi encontrado um Canvas para os créditos!");
            canScroll = false;
            return;
        }

        RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

        float canvasHeight = canvasRect.rect.height;
        float creditsHeight = creditsRect.rect.height;

        //Quanto a imagem precisa subir para mostrar seu final
        float scrollDistance = creditsHeight - canvasHeight;

        if(scrollDistance <= 0)
        {
            Debug.LogWarning("A imagem dos créditos não é maior que a área visível.");

            canScroll = false;
            return;
        }

        //Comeca mostrando o topo da imagem
        creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, -scrollDistance);

        //Termina quando chegar em Y = 0
        stopPositionY = 0f;
    }

    //Reinicia a rolagem dos creditos
    public void ResetScroll()
    {
        if(creditsRect == null) return;

        CalculateScrollPositions();

        canScroll = true;
    }
}