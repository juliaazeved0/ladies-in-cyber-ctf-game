using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        originalLayerName = tilemapRenderer.sortingLayerName;
        originalOrder = tilemapRenderer.sortingOrder;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color c = tilemap.color;
            if(c.a != 0.5f) 
            {
                c.a = 0.5f;
                tilemap.color = c;
                tilemapRenderer.sortingLayerName = "Objetos a frente"; 
            }

            // A MÁGICA: A parede copia a ordem da player e soma +1.
            // Ela ganha da player, mas não esmaga os móveis da frente!
            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                tilemapRenderer.sortingOrder = playerSprite.sortingOrder + 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color c = tilemap.color;
            c.a = 1f;
            tilemap.color = c;
            tilemapRenderer.sortingLayerName = originalLayerName;
            tilemapRenderer.sortingOrder = originalOrder; 
        }
    }
}