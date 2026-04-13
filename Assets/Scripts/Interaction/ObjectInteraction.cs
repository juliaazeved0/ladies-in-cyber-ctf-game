using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Settings object interactable")]
    protected bool playerIsHere;
    public GameObject interactionNotice;
    public GameObject challengePanel;

    protected void Start()
    {
        interactionNotice.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;
            // Só mostra o aviso se nenhum painel estiver aberto
            if (!IsAnyPanelOpen() && interactionNotice != null)
            {
                interactionNotice.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;
            if (interactionNotice != null)
            {
                interactionNotice.SetActive(false);
            }
        }
    }

    protected virtual void Update()
    {
        // Bloqueia a tecla E e esconde o aviso se qualquer painel estiver aberto.
        // Cobre: challengePanel, terminalPanel, notas, steg e qualquer outro painel de UI.
        if (IsAnyPanelOpen())
        {
            if (interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);
            return;
        }

        // Reexibe o aviso se o player estiver na área e nenhum painel estiver aberto
        if (playerIsHere && interactionNotice != null && !interactionNotice.activeSelf)
        {
            interactionNotice.SetActive(true);
        }

        if (playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // Retorna true se qualquer painel registrado no CanvasManager estiver ativo.
    private bool IsAnyPanelOpen()
    {
        if (CanvasManager.Instance == null) return false;

        foreach (GameObject panel in CanvasManager.Instance.allPanels)
        {
            if (panel != null && panel.activeSelf) return true;
        }
        return false;
    }

    protected virtual void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false);
        CanvasManager.Instance.OpenPanel(challengePanel.name);
    }
}
