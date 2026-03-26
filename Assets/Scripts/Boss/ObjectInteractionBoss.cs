using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionBoss : MonoBehaviour
{
    [Header("Settings object interactable")]
    protected bool playerIsHere; //Verifica se a jogadora está dentro do collider do objeto
    public GameObject interactionNotice; //Aviso de "Pressione E"
    public GameObject challengePanel; //Painel que será aberto ao interagir

    protected void Start()
    {
        if (interactionNotice != null) interactionNotice.SetActive(false); //Garante que o aviso de "Pressione E" comece desativado
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) //Quando algo entra na área do Trigger (Collider)
    {
        if (collision.CompareTag("Player")) //Verifica se a jogadora possui a tag
        {
            playerIsHere = true;

            if (interactionNotice != null && DialogueManagerBoss.dialogueBossFinished) //Só ativa o aviso se o diálogo com o Boss já estiver terminado
            {
                interactionNotice.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) //Quando algo sai da área do Trigger
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;

            if (interactionNotice != null) //Esconde o aviso, pois a jogadora se afastou da área
            {
                interactionNotice.SetActive(false);
            }
        }
    }

    protected virtual void Update()
    {
        if (challengePanel != null && challengePanel.activeSelf) //Se o painel do desafio já estiver aberto, interrompe e a jogadora não interage novamente
        {
            return;
        }

        //Verifica se a jogadora está perto, apertou E e o diálogo com o Boss foi finalizado
        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && DialogueManagerBoss.dialogueBossFinished)
        {
            Interact();
        }

        //Se a jogadora estiver na área e o diálogo acabar, o aviso aparece imediatamente sem precisar sair e entrar novamente na área de Collider
        if (playerIsHere && DialogueManagerBoss.dialogueBossFinished && interactionNotice != null && !interactionNotice.activeSelf)
        {
            interactionNotice.SetActive(true);
        }
    }

    protected virtual void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false); //Desativa o mini-mapa para focar na interação
        CanvasManager.Instance.OpenPanel(challengePanel.name); //Abre o painel correspondente usando o nome do objeto de desafio
    }
}