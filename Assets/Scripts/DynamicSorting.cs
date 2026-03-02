using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * -100);
    }
}
