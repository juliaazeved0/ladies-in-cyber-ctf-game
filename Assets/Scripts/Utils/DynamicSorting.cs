using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ajusta dinamicamente a ordem de renderizacao com base na posicao Y.
/// Isso permite que a jogadora passe por tras de objetos mais altos e na frente de objetos mais baixos.
/// </summary>
public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer == null)
        {
            Debug.LogWarning("DynamicSorting: No SpriteRenderer found on " + gameObject.name + ". Script will not work.");
            enabled = false;
        }
    }

    /// <summary>
    /// Usado para garantir que o sorting seja atualizado apos todos os movimentos do frame.
    /// </summary>
    void LateUpdate()
    {
        if(spriteRenderer != null)
        {
            //Multiplica por -100 para que quanto mais baixo o objeto (menor Y), maior o sorting order.
            //Garante que quem está "mais perto" da câmera (embaixo) apareça na frente.
            spriteRenderer.sortingOrder = (int)(transform.position.y * -100);
        }
    }
}