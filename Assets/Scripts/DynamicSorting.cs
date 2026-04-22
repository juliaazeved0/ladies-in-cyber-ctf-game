using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("DynamicSorting: No SpriteRenderer found on " + gameObject.name + ". Script will not work.");
            enabled = false; // Desabilita o script para evitar erros
        }
    }

    void LateUpdate()
    {
        if (spriteRenderer != null)
        {
            // Ajusta a ordem de renderização baseada na posição Y (sem afetar transparência)
            spriteRenderer.sortingOrder = (int)(transform.position.y * -100);
        }
    }
}
