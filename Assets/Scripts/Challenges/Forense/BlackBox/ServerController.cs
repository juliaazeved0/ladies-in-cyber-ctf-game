using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : LockObjectInteraction //J� possui todas as fun��es de intera��o do LockObjectInteraction
{
    [Header("Objetos dos Cadeados")] //T�tulo visual no Inspector e permite que arraste objetos
    public SpriteRenderer lockClosed; //Imagem do cadeado fechado
    public SpriteRenderer lockOpened; //Imagem do cadeado aberto

    [Header("Estado Final")]
    public bool finalizado = false; //Vari�vel para indicar se o desafio do servidor foi conclu�do
    public GameObject panelConnect; //Painel que aparece quando os cabos precisam ser conectados

    void Start()
    {
        isUnlocked = false; //O servidor come�a bloqueado
        finalizado = false; //Garante que o desafio do servidor tamb�m come�a n�o finalizado

        //Verifica se o objeto foi arrastado no Inspector. Se foi, ativa o cadeado fechado. Ou seja, o servidor come�a visualmente bloqueado
        if (lockClosed != null) lockClosed.gameObject.SetActive(true);

        //Desativa o cadeado aberto. Assim, s� aparece o cadeado fechado no in�cio
        if(lockOpened != null) lockOpened.gameObject.SetActive(false);

        //Painel que o desafio aparece quando o servidor � desbloqueado, sendo desativado no in�cio
        if(challengePanel != null) challengePanel.gameObject.SetActive(false);

        //Garante que o painel de conex�o de cabos comece escondido
        if (panelConnect != null) panelConnect.SetActive(false);
    }

    protected override void Update()
    {
        base.Update(); //Detecta a tecla E, apari��o do modal e a l�gica de proximidade
    }

    public void UnlockByHacking() //Fun��o chamada pelo ManagerPanels
    {
        isUnlocked = true; //Marca o servidor como desbloqueado depois de ser acessado no PC

        if (lockClosed != null) lockClosed.gameObject.SetActive(false); //Esconde o cadeado fechado

        if (lockOpened != null) lockOpened.gameObject.SetActive(true); //Mostrado o cadeado aberto
    }

    protected override void Interact() //Fun��o chamada quando a jogadora pressiona a tecla E perto do objeto
    {
        if (finalizado) //A partir disso, quando a jogadora interagir novamente, mostra apenas o painel de conex�o com os cabos
        {
            if(panelConnect != null)
            {
                panelConnect.SetActive(true); //Mostra o painel de conex�o dos cabos

                if (interactionNotice != null) interactionNotice.SetActive(false); //Esconde o modal
            }
            return; //Interrompe a fun��o, nada mais abaixo � executado
        }
        
        //S� permite a intera��o se o servidor for hackeado pelo PC
        if (isUnlocked)
        {
            if (challengePanel != null)
            {
                challengePanel.SetActive(true); //Abre o painel do desafio do servidor

                //Desativa o aviso visual "Aperte E" enquanto ela faz o desafio
                if (interactionNotice != null) interactionNotice.SetActive(false);
            }

        }
    }

    public void ConcluirServidor()
    {
        finalizado = true; //Marca o servidor como completado
    }
}