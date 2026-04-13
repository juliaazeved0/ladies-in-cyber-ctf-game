using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionBoss : MonoBehaviour
{
    [Header("Settings object interactable")]
    protected bool playerIsHere; 
    public GameObject interactionNotice; 
    public GameObject challengePanel; 

    protected void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false);
        
        // Debug para verificar o estado ao iniciar a cena do Boss
        Debug.Log($"[ObjectInteractionBoss] Cena iniciada. Diálogo finalizado: {DialogueManagerBoss.dialogueBossFinished}");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
        // 1. Se o painel estiver aberto, mantém o aviso desligado
        if (challengePanel != null && challengePanel.activeSelf)
        {
            if (interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);
            return; 
        }

        // 2. Atualiza o aviso caso o diálogo termine enquanto o player está parado no collider
        UpdateInteractionNotice();

        // 3. Interação
        if (playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            // Verificação dupla: o diálogo terminou?
            if (DialogueManagerBoss.dialogueBossFinished)
            {
                Interact();
            }
            else
            {
                Debug.LogWarning("Tentativa de interagir com o PC antes de terminar o diálogo com o Boss.");
            }
        }
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
            Debug.Log($"Abrindo painel: {challengePanel.name}");
        }
        else
        {
            Debug.LogError("ERRO: CanvasManager.Instance não encontrado! Verifique se o CanvasManager está na cena ou se é um DontDestroyOnLoad.");
        }
    }
}