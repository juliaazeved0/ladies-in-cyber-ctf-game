using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBossInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey;
    public Image balloonNPC;

    [Header("Interaction")]
    public GameObject interactionNotice;

    private bool playerIsHere = false;
    private bool isCompleted = false;
    private bool isTalking = false; // NOVA VARIÁVEL: Controla se o diálogo está em curso

    [Header("Systems (Assign one or both)")]
    public DialogueManagerBoss dialogueManagerBoss;

    [Header("Nodes")]
    public DialogueNodeBoss firstNodeBoss;

    void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);
        if (balloonNPC != null) balloonNPC.gameObject.SetActive(false);
        CheckChallengeStatus();
    }

    void Update()
    {
        // Verifica se o jogador apertou E e não completou o desafio ainda
        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            if (!isTalking)
            {
                // Se não estava falando, começa a conversa
                StartConversation();
                isTalking = true;
            }
            else
            {
                if (!dialogueManagerBoss.CurrentNodeHasOptions())
                {
                    // Se já estava falando, avança para a opção 0 (próximo node)
                    dialogueManagerBoss.ChooseOption(0);
                }
                else
                {
                    Debug.Log("Escolha uma opção no mouse para continuar!");
                }
                
            }
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
            if (!isCompleted && interactionNotice != null)
            {
                interactionNotice.SetActive(true);
                if (balloonNPC != null) balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;
            isTalking = false; // RESET: Se o jogador sair de perto, a conversa reseta

            if (interactionNotice != null)
            {
                interactionNotice.SetActive(false);
                if (balloonNPC != null) balloonNPC.gameObject.SetActive(false);
            }

            // Opcional: Fecha o painel de diálogo se o jogador se afastar
            if (dialogueManagerBoss != null) dialogueManagerBoss.panelDialogue.SetActive(false);
        }
    }

    public void CheckChallengeStatus()
    {
        if (!string.IsNullOrEmpty(uniqueSaveKey))
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
    }
}