using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerVisuals : MonoBehaviour
{
    [Header("Configurações Visuais")] //Cria um título no Inspector
    public SpriteRenderer lockImage; //Referência ao componente que desenha a imagem no 2D

    //As duas imagens que representam os estados do cadeado
    public Sprite openLockSprite;
    public Sprite closedLockSprite;

    private void Start()
    {
        Lock(); //Garante que o servidor comece trancado visualmente
    }

    public void Lock()
    {
        if(lockImage != null && closedLockSprite != null) //Verifica se arrastou o SpriteRenderer e a imagem  do "cadeado fechado"
        {
            lockImage.sprite = closedLockSprite; //Troca a imagem atual pela imagem do cadeado fechado
        }
    }

    public void Unlock()
    {
        if(lockImage != null && openLockSprite  != null) //Verifica se arrastou o SpriteRenderer e a imagem do "cadeado aberto"
        {
            lockImage.sprite = openLockSprite; //Troca a imagem atual pela imagem do cadeado aberto
        }
    }
}