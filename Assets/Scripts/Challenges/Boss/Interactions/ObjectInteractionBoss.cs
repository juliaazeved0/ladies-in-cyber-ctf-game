using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerencia deteccao de proximidade da jogadora, aviso 
/// visual e interacao e abertura do painel de desafio.
/// </summary>
public class ObjectInteractionBoss : MonoBehaviour
{
    [Header("Object Interaction Settings")]
    [Tooltip("Indica se a jogadora esta dentro da area de interacao do objeto.")]
    protected bool playerIsHere;

    [Header("UI References")]
    [Tooltip("Mensagem exibida a jogadora indicando que ela pode interagir com o objeto.")]
    public GameObject interactionNotice;

    [Tooltip("Painel que sera aberto apois a jogadora interagir com o objeto.")]
    public GameObject challengePanel;

    //Esconde o aviso e abre o painel do desafio, desativando o minimapa
    protected virtual void Start()
    {
        if(interactionNotice != null) interactionNotice.SetActive(false);

        Debug.Log($"Cena iniciada. Diálogo finalizado: {DialogueManagerBoss.dialogueBossFinished}");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;

            UpdateInteractionNotice();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;

            if(interactionNotice != null) interactionNotice.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        if(IsAnyPanelOpen())
        {
            if(interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);

            return;
        }

        UpdateInteractionNotice();

        if(playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            if(DialogueManagerBoss.dialogueBossFinished)
            {
                Interact();
            }
            else
            {
                Debug.LogWarning("Tentativa de interagir com o PC antes de terminar o diálogo com o Boss.");
            }
        }
    }

    private bool IsAnyPanelOpen()
    {
        if(CanvasManager.Instance == null || CanvasManager.Instance.allPanels == null) return false;

        foreach(GameObject panel in CanvasManager.Instance.allPanels)
        {
            if(panel != null && panel.activeInHierarchy) return true;
        }

        return false;
    }

    private void UpdateInteractionNotice()
    {
        if(playerIsHere && DialogueManagerBoss.dialogueBossFinished)
        {
            if(interactionNotice != null && !interactionNotice.activeSelf)
                interactionNotice.SetActive(true);
        }
    }

    protected virtual void Interact()
    {
        if(interactionNotice != null) interactionNotice.SetActive(false);

        if(challengePanel == null)
        {
            Debug.LogError("ERRO: Challenge Panel não foi configurado no Inspector!", this);
            return;
        }

        if(CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ToggleMiniMap(false);

            CanvasManager.Instance.OpenPanel(challengePanel.name);

            Debug.Log($"Abrindo painel: {challengePanel.name}");
        }
        else
        {
            Debug.LogError("ERRO: CanvasManager.Instance não encontrado!");
        }
    }
}