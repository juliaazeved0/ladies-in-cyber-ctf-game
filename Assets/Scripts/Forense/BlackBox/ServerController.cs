using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : LockObjectInteraction //Já possui todas as funções de interação do LockObjectInteraction
{
    [Header("Objetos dos Cadeados")] //Título visual no Inspector e permite que arraste objetos
    public SpriteRenderer lockClosed; //Imagem do cadeado fechado
    public SpriteRenderer lockOpened; //Imagem do cadeado aberto

    [Header("Estado Final")]
    public bool finalizado = false; //Variável para indicar se o desafio do servidor foi concluído
    public GameObject panelConnect; //Painel que aparece quando os cabos precisam ser conectados

    void Start()
    {
        isUnlocked = false; //O servidor começa bloqueado
        finalizado = false; //Garante que o desafio do servidor também começa não finalizado

        //Verifica se o objeto foi arrastado no Inspector. Se foi, ativa o cadeado fechado. Ou seja, o servidor começa visualmente bloqueado
        if (lockClosed != null) lockClosed.gameObject.SetActive(true);

        //Desativa o cadeado aberto. Assim, só aparece o cadeado fechado no início
        if(lockOpened != null) lockOpened.gameObject.SetActive(false);

        //Painel que o desafio aparece quando o servidor é desbloqueado, sendo desativado no início
        if(challengePanel != null) challengePanel.gameObject.SetActive(false);

        //Garante que o painel de conexão de cabos comece escondido
        if (panelConnect != null) panelConnect.SetActive(false);
    }

    protected override void Update()
    {
        base.Update(); //Detecta a tecla E, aparição do modal e a lógica de proximidade
    }

    public void UnlockByHacking() //Função chamada pelo ManagerPanels
    {
        isUnlocked = true; //Marca o servidor como desbloqueado depois de ser acessado no PC

        if (lockClosed != null) lockClosed.gameObject.SetActive(false); //Esconde o cadeado fechado

        if (lockOpened != null) lockOpened.gameObject.SetActive(true); //Mostrado o cadeado aberto
    }

    protected override void Interact() //Função chamada quando a jogadora pressiona a tecla E perto do objeto
    {
        if (finalizado) //A partir disso, quando a jogadora interagir novamente, mostra apenas o painel de conexão com os cabos
        {
            if(panelConnect != null)
            {
                panelConnect.SetActive(true); //Mostra o painel de conexão dos cabos

                if (interactionNotice != null) interactionNotice.SetActive(false); //Esconde o modal
            }
            return; //Interrompe a função, nada mais abaixo é executado
        }
        
        //Só permite a interação se o servidor for hackeado pelo PC
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