using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base para interacoes com objetos no mundo 2D.
/// Gerencia a deteccao da jogadora e a exibicao de avisos baseados no estado da UI.
/// </summary>
public class ObjectInteraction : MonoBehaviour
{
    [Header("Settings object interactable")]
    public GameObject interactionNotice; //Aviso visual
    public GameObject challengePanel; //Painel que sera aberto

    protected bool playerIsHere;

    protected void Start()
    {
        interactionNotice.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;
            //So mostra o aviso se a caminho estiver livre (sem outros paineis abertos)
            if(!IsAnyPanelOpen() && interactionNotice != null)
            {
                interactionNotice.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;
            if(interactionNotice != null)
            {
                interactionNotice.SetActive(false);
            }
        }
    }

    protected virtual void Update()
    {
        //Se houver qualquer painel aberto (mesmo que nao seja deste objeto), esconde o aviso
        if(IsAnyPanelOpen())
        {
            if(interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);
            return;
        }

        //Se a player estiver aqui e a UI estiver limpa, reexibe o aviso
        if(playerIsHere && interactionNotice != null && !interactionNotice.activeSelf)
        {
            interactionNotice.SetActive(true);
        }

        //Executa a interacao ao pressionar a tecla de interacao
        if(playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    /// <summary>
    /// Consulta o CanvasManager para verificar se ha alguma interface bloqueando a visao.
    /// </summary>
    private bool IsAnyPanelOpen()
    {
        if(CanvasManager.Instance == null) return false;

        foreach(GameObject panel in CanvasManager.Instance.allPanels)
        {
            if(panel != null && panel.activeSelf) return true;
        }
        return false;
    }

    /// <summary>
    /// Logica de interacao.
    /// </summary>
    protected virtual void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false);
        CanvasManager.Instance.OpenPanel(challengePanel.name);
    }
}