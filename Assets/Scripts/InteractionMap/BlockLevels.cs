using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockLevels : MonoBehaviour
{   

    public TilemapCollider2D colliderTilemap;
   

    public void DisableBlock()
    {
        colliderTilemap.enabled = false;
        Debug.Log("disableBlock sendo chamado");
    }
}
