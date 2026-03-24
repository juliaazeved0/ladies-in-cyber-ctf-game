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
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;

            // SÓ mostra o aviso (Ex: "Aperte E") se o diálogo com o Boss já terminou
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
        // Se o painel já estiver aberto, não faz nada
        if (challengePanel != null && challengePanel.activeSelf)
        {
            return;
        }

        // NOVA TRAVA: Verifica se o jogador está aqui, apertou E E SE o diálogo terminou
        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && DialogueManagerBoss.dialogueBossFinished)
        {
            Interact();
        }

        // OPCIONAL: Se o jogador estiver na área e o diálogo acabar enquanto ele está lá, 
        // ativa o aviso automaticamente
        if (playerIsHere && DialogueManagerBoss.dialogueBossFinished && interactionNotice != null && !interactionNotice.activeSelf)
        {
            interactionNotice.SetActive(true);
        }
    }

    protected virtual void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false);
        CanvasManager.Instance.OpenPanel(challengePanel.name);
    }
}