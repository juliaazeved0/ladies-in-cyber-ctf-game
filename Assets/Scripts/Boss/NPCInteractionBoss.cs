using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BashTerminal;

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
    public GameObject terminalPanel;

    private bool playerIsHere = false;
    private bool isCompleted = false;
    private bool isTalking = false;

    void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);
        if (balloonNPC != null) balloonNPC.gameObject.SetActive(false);
        CheckChallengeStatus();
    }

    void Update()
    {
        // Se o Terminal estiver aberto, ignora a tecla E
        if (terminalPanel != null && terminalPanel.activeSelf)
        {
            return;
        }

        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            // Painel fechado: inicia a conversa
            if (!isTalking && !dialogueManagerBoss.panelDialogue.activeSelf)
            {
                StartConversation();
                isTalking = true;
                if (interactionNotice != null) interactionNotice.SetActive(false);
            }
            // Painel aberto: 1º E completa o texto, 2º E avança o node
            else if (dialogueManagerBoss.panelDialogue.activeSelf)
            {
                if (dialogueManagerBoss.writeMachine.IsTyping)
                {
                    dialogueManagerBoss.writeMachine.Complete();
                }
                else if (!dialogueManagerBoss.CurrentNodeHasOptions())
                {
                    dialogueManagerBoss.ChooseOption(0);
                }
                else
                {
                    Debug.Log("Escolha uma opção no mouse para continuar!");
                }
            }
        }

        // Se o diálogo fechar, reseta o isTalking
        if (isTalking && !dialogueManagerBoss.panelDialogue.activeSelf)
        {
            isTalking = false;
            if (playerIsHere && interactionNotice != null) interactionNotice.SetActive(true);
        }
    }

    private void StartConversation()
    {
        if (dialogueManagerBoss != null && firstNodeBoss != null)
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

            if (!isCompleted && !dialogueManagerBoss.panelDialogue.activeSelf)
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

            if (interactionNotice != null) interactionNotice.SetActive(false);
            if (balloonNPC != null) balloonNPC.gameObject.SetActive(false);

            if (dialogueManagerBoss != null) dialogueManagerBoss.panelDialogue.SetActive(false);
        }
    }

    public void CheckChallengeStatus()
    {
        if (!string.IsNullOrEmpty(uniqueSaveKey))
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
    }
}
