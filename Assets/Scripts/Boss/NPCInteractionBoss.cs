using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BashTerminal; // Adicionado para verificar o terminal se necessário

public class NPCBossInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey;
    public Image balloonNPC;

    [Header("Interaction")]
    public GameObject interactionNotice;

    [Header("Systems")]
    public DialogueManagerBoss dialogueManagerBoss;

    [Header("Nodes")]
    public DialogueNodeBoss firstNodeBoss;

    [Header("External Blockers")]
    public GameObject terminalPanel; // Arraste o painel do terminal aqui no Inspector

    private bool playerIsHere = false;
    private bool isCompleted = false;
    private bool isTalking = false;

    void Start()
    {
        if(interactionNotice != null) interactionNotice.SetActive(false);
        if(balloonNPC != null) balloonNPC.gameObject.SetActive(false);
        CheckChallengeStatus();
    }

    void Update()
    {
        // 1. VERIFICAÇÃO DE BLOQUEIO:
        // Se o Terminal estiver aberto, ignore completamente a tecla E do NPC
        if (terminalPanel != null && terminalPanel.activeSelf)
        {
            return;
        }

        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            // 2. VERIFICAÇÃO DO PAINEL DE DIÁLOGO:
            // Se o painel de diálogo ainda NÃO está ativo, iniciamos a conversa
            if(!isTalking && !dialogueManagerBoss.panelDialogue.activeSelf)
            {
                StartConversation();
                isTalking = true;
                
                // Esconde o "Pressione E" ao começar a falar
                if (interactionNotice != null) interactionNotice.SetActive(false);
            }
            // 3. SE O PAINEL JÁ ESTÁ ABERTO:
            // A tecla E servirá apenas para avançar o texto
            else if (dialogueManagerBoss.panelDialogue.activeSelf)
            {
                if(!dialogueManagerBoss.CurrentNodeHasOptions())
                {
                    dialogueManagerBoss.ChooseOption(0); // Avança o diálogo
                }
                else
                {
                    Debug.Log("Escolha uma opção no mouse para continuar!");
                }
            }
        }

        // 4. LÓGICA DO BALÃO/AVISO:
        // Se o diálogo fechar (pelo manager), resetamos o isTalking
        if (isTalking && !dialogueManagerBoss.panelDialogue.activeSelf)
        {
            isTalking = false;
            if (playerIsHere && interactionNotice != null) interactionNotice.SetActive(true);
        }
    }

    private void StartConversation()
    {
        if(dialogueManagerBoss != null && firstNodeBoss != null)
        {
            dialogueManagerBoss.firstNode = firstNodeBoss;
            dialogueManagerBoss.StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;
            CheckChallengeStatus();

            // Só mostra o aviso se o diálogo NÃO estiver rolando e não estiver completo
            if(!isCompleted && !dialogueManagerBoss.panelDialogue.activeSelf)
            {
                if (interactionNotice != null) interactionNotice.SetActive(true);
                if (balloonNPC != null) balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;
            isTalking = false;

            if(interactionNotice != null) interactionNotice.SetActive(false);
            if(balloonNPC != null) balloonNPC.gameObject.SetActive(false);

            if(dialogueManagerBoss != null) dialogueManagerBoss.panelDialogue.SetActive(false);
        }
    }

    public void CheckChallengeStatus()
    {
        if(!string.IsNullOrEmpty(uniqueSaveKey))
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
    }
}