using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBossInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey;
    public Image balloonNPC;

    [Header("Dynamic Variables")]
    public PulseOutline pulseObjectInitial;

    [Header("Interaction")]
    public GameObject interactionNotice;

    private bool playerIsHere = false;
    private bool isCompleted = false;

    [Header("Systems (Assign one or both)")]
    public SimpleDialogue simpleDialogue;        // Sistema antigo (não mexer)
    public DialogueManagerBoss dialogueManagerBoss; // Sistema novo do Boss

    [Header("Nodes")]
    //public NPCDialogueNode firstNodeNormal;   // Nó para o sistema antigo
    public DialogueNodeBoss firstNodeBoss;     // Nó para o sistema do Boss

    void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);
        if (balloonNPC != null) balloonNPC.gameObject.SetActive(false);
        CheckChallengeStatus();
    }

    void Update()
    {
        // Verifica se o player apertou E e se nenhum dos dois painéis de diálogo está aberto
        bool isAnyDialogueActive = (simpleDialogue != null && simpleDialogue.panelDialogue.activeSelf) ||
                                   (dialogueManagerBoss != null && dialogueManagerBoss.panelDialogue.activeSelf);

        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted && !isAnyDialogueActive)
        {
            StartConversation();
        }
    }

    private void StartConversation()
    {
        // PRIORIDADE 1: Se tiver um nó de Boss e um Manager de Boss, usa eles
        if (dialogueManagerBoss != null && firstNodeBoss != null)
        {
            if (CanvasManager.Instance != null) CanvasManager.Instance.ToggleMiniMap(false);

            // Configura o nó no manager e inicia
            dialogueManagerBoss.firstNode = firstNodeBoss;
            dialogueManagerBoss.StartDialogue();
        }
        // PRIORIDADE 2: Se não for boss, usa o sistema comum (SimpleDialogue)
        //else if (simpleDialogue != null && firstNodeNormal != null)
        {
            simpleDialogue.pulsingObject = pulseObjectInitial;
            if (CanvasManager.Instance != null) CanvasManager.Instance.ToggleMiniMap(false);

            //simpleDialogue.StartDialogue(firstNodeNormal);
        }
    }

    // --- Métodos de Trigger (Mantidos iguais) ---
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
            if (interactionNotice != null)
            {
                interactionNotice.SetActive(false);
                if (balloonNPC != null) balloonNPC.gameObject.SetActive(false);
            }
        }
    }

    public void CheckChallengeStatus()
    {
        if (!string.IsNullOrEmpty(uniqueSaveKey))
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
    }
}