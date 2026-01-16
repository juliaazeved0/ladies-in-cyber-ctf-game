using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparenceWall : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;

    void Start()
    {
        
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
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
                tilemapRenderer.sortingOrder = 10;
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
    
            
            tilemapRenderer.sortingOrder = 0; 
        }
    }
}

