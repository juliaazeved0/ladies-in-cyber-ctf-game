using UnityEngine;

/// <summary>
/// Ajusta dinamicamente a ordem de renderizacao do SpriteRenderer
/// com base na posicao do objeto no eixo Y, criando um efeito de
/// profundidade 2D.
/// </summary>
public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    //Obtem a referencia do SpriteRenderer e valida sua existencia na inicializacao
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Tratamento de erro
        if(spriteRenderer == null)
        {
            Debug.LogWarning($"[DynamicSorting] Erro: Nenhum SpriteRenderer encontrado em {gameObject.name}. O script sera desativado automaticamente.");
            enabled = false;
        }
    }

    //Atualiza o Sorting Order a cada frame final para refletir movimentacoes recentes de fisica ou animacao
    void LateUpdate()
    {
        //Se o componente por algum motivo for removido em runtime, evita NullReferenceException
        if(spriteRenderer != null)
        {
            //Converte a posicao Y em uma ordem inteira. Objetos mais abaixo na tela (Y menor) terao um Sorting Order maior (ficam na frente)
            spriteRenderer.sortingOrder = (int)(transform.position.y * -100);
        }
    }
}