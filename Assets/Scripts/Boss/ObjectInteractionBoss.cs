using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionBoss : MonoBehaviour
{
    [Header("Settings object interactable")]
    protected bool playerIsHere;
    public GameObject interactionNotice;
    public GameObject challengePanel;

    protected virtual void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);

        Debug.Log($"[ObjectInteractionBoss] Cena iniciada. Diálogo finalizado: {DialogueManagerBoss.dialogueBossFinished}");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && DialogueManagerBoss.dialogueBossFinished)
        {
            playerIsHere = true;
            UpdateInteractionNotice();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;
            if (interactionNotice != null) interactionNotice.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        // 1. Se qualquer painel de UI estiver aberto, bloqueia toda interação com E
        //    Isso cobre: challengePanel, terminalPanel, panelNotes, steg, etc.
        if (IsAnyPanelOpen())
        {
            if (interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);
            return;
        }

        // 2. Atualiza o aviso caso o diálogo termine enquanto o player está no collider
        UpdateInteractionNotice();

        // 3. Interação com tecla E
        if (playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueManagerBoss.dialogueBossFinished)
            {
                Interact();
            }
            else
            {
                Debug.LogWarning("[ObjectInteractionBoss] Tentativa de interagir com o PC antes de terminar o diálogo com o Boss.");
            }
        }
    }

    // Retorna true se qualquer painel registrado no CanvasManager estiver ativo.
    // Cobre challengePanel, terminalPanel e qualquer outro painel de desafio.
    private bool IsAnyPanelOpen()
    {
        if (CanvasManager.Instance == null) return false;

        foreach (GameObject panel in CanvasManager.Instance.allPanels)
        {
            if (panel != null && panel.activeSelf) return true;
        }
        return false;
    }

    private void UpdateInteractionNotice()
    {
        if (playerIsHere && DialogueManagerBoss.dialogueBossFinished)
        {
            if (interactionNotice != null && !interactionNotice.activeSelf)
                interactionNotice.SetActive(true);
        }
    }

    protected virtual void Interact()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);

        if (CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ToggleMiniMap(false);
            CanvasManager.Instance.OpenPanel(challengePanel.name);
            Debug.Log($"[ObjectInteractionBoss] Abrindo painel: {challengePanel.name}");
        }
        else
        {
            Debug.LogError("[ObjectInteractionBoss] ERRO: CanvasManager.Instance não encontrado!");
        }
    }
}
