using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionBoss : MonoBehaviour
{
    [Header("Settings object interactable")]
    protected bool playerIsHere; // Verifica se a jogadora está dentro do collider do objeto
    public GameObject interactionNotice; // Aviso de "Pressione E"
    public GameObject challengePanel; // Painel que será aberto ao interagir

    protected void Start()
    {
        // Garante que o aviso de "Pressione E" comece desativado
        if (interactionNotice != null) interactionNotice.SetActive(false); 
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;

            // Só ativa o aviso se o diálogo com o Boss já estiver terminado
            if (interactionNotice != null && DialogueManagerBoss.dialogueBossFinished)
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
        // 1. VERIFICAÇÃO DE BLOQUEIO: Se o painel estiver aberto, ignora todo o resto
        if (challengePanel != null && challengePanel.activeSelf)
        {
            // Opcional: Esconde o "Pressione E" enquanto o painel está na tela
            if (interactionNotice != null && interactionNotice.activeSelf)
            {
                interactionNotice.SetActive(false);
            }
            return; // Sai do Update aqui, impedindo que o Input.GetKeyDown seja ouvido
        }

        // 2. LÓGICA DE EXIBIÇÃO DO AVISO:
        // Se a jogadora estiver na área e o diálogo acabar, o aviso aparece 
        // sem precisar sair e entrar novamente na área de Collider.
        if (playerIsHere && DialogueManagerBoss.dialogueBossFinished && interactionNotice != null && !interactionNotice.activeSelf)
        {
            interactionNotice.SetActive(true);
        }

        // 3. INTERAÇÃO: Só chega aqui se o painel estiver FECHADO (devido ao return acima)
        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && DialogueManagerBoss.dialogueBossFinished)
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        // Esconde o aviso ao interagir
        if (interactionNotice != null) interactionNotice.SetActive(false);

        CanvasManager.Instance.ToggleMiniMap(false); // Desativa o mini-mapa
        CanvasManager.Instance.OpenPanel(challengePanel.name); // Abre o painel correspondente
    }
}