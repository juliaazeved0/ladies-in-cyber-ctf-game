using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerVisuals : MonoBehaviour
{
    [Header("Configurações Visuais")]
    public SpriteRenderer lockImage;

    public Sprite openLockSprite;
    public Sprite closedLockSprite;

    private void Start()
    {
        Lock();
    }

    public void Lock()
    {
        if(lockImage != null && closedLockSprite != null)
        {
            lockImage.sprite = closedLockSprite;
        }
    }

    public void Unlock()
    {
        if(lockImage != null && openLockSprite  != null)
        {
            lockImage.sprite = openLockSprite;
        }
    }
}
