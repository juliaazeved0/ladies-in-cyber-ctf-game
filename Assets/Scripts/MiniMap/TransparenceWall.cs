using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Gerencia a transparencia de paredes (Tilemaps) quando a jogadora entra em sua area.
/// Ajusta a Sorting Layer e a Order para permitir que a jogadora seja vista atraves da parede.
/// </summary>
public class TransparenceWall : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;

    private string originalLayerName;
    private int originalOrder;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();

        //Armazena os valores originais para restaura-los ao sair do gatilho
        originalLayerName = tilemapRenderer.sortingLayerName;
        originalOrder = tilemapRenderer.sortingOrder;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Color c = tilemap.color;

            if(c.a != 0.5f) 
            {
                c.a = 0.5f;
                tilemap.color = c;
                tilemapRenderer.sortingLayerName = "Objetos a frente"; //Altera a camada configurada para objetos a frente
            }

            //A parede copia a ordem da player e soma +1. Isso garante que a parede fique acima da player, mas translucida
            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();

            if(playerSprite != null)
            {
                tilemapRenderer.sortingOrder = playerSprite.sortingOrder + 1;
            }
        }
    }

    /// <summary>
    /// Restaura as configuracoes originais de cor e renderizacao do Tilemap.
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Color c = tilemap.color;
            c.a = 1f;
            tilemap.color = c;

            tilemapRenderer.sortingLayerName = originalLayerName;
            tilemapRenderer.sortingOrder = originalOrder; 
        }
    }
}