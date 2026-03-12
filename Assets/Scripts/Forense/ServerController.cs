using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : LockObjectInteraction
{
    [Header("Objetos dos Cadeados")] //Título no Inspector para organizar e permite que arraste objetos
    public SpriteRenderer lockClosed; //Imagem do cadeado fechado
    public SpriteRenderer lockOpened; //Imagem do cadeado aberto

    void Start()
    {
        isUnlocked = false;

        if (lockClosed != null) lockClosed.gameObject.SetActive(true);
        if(lockOpened != null) lockOpened.gameObject.SetActive(false);

        if(challengePanel != null) challengePanel.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void UnlockByHacking() //Função chamada pelo ManagerPanels
    {
        isUnlocked = true;

        //Troca os cadeados visualmente
        if (lockClosed != null) lockClosed.gameObject.SetActive(false);
        if (lockOpened != null) lockOpened.gameObject.SetActive(true);
    }

    protected override void Interact()
    {
        // Se a jogadora interagir E o hack do PC já estiver liberado
        if (isUnlocked)
        {
            if (challengePanel != null)
            {
                challengePanel.SetActive(true);

                // Desativa o aviso visual "Aperte E" enquanto ela faz o desafio
                if (interactionNotice != null) interactionNotice.SetActive(false);
            }

        }
    }
}