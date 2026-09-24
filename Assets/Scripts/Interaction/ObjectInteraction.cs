using UnityEngine;

/// <summary>
/// Classe base para objetos interagiveis no cenario (baus, NPCs, terminais, etc.).
/// Detecta a presenca da jogadora via trigger 2D, exibe um aviso de interacao
/// e dispara a logica de interacao (abrir painel de desafio) ao pressionar E,
/// desde que nenhum outro painel esteja aberto no momento.
/// </summary>
public class ObjectInteraction : MonoBehaviour
{
    [Header("Settings Object Interactable")]
    [Tooltip("Icone/aviso exibido quando a jogadora esta proxima e pode interagir.")]
    public GameObject interactionNotice;

    [Tooltip("Painel do desafio associado a este objeto, aberto ao interagir.")]
    public GameObject challengePanel;

    //Indica se a jogadora esta atualmente dentro da area de trigger deste objeto
    protected bool playerIsHere;

    protected void Start()
    {
        //Garante que o aviso de interacao comece desativado
        if(interactionNotice != null) interactionNotice.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;

            //So mostra o aviso se nenhum painel estiver aberto no momento
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
        //Se algum painel estiver aberto, esconde o aviso e ignora a interacao
        if(IsAnyPanelOpen())
        {
            if(interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);
            return;
        }

        //Reexibe o aviso caso a jogadora ainda esteja na area e nenhum painel esteja aberto
        if(playerIsHere && interactionNotice != null && !interactionNotice.activeSelf)
        {
            interactionNotice.SetActive(true);
        }

        //Dispara a interacao ao pressionar E, apenas se o jogador estiver na area
        if(playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    //Verifica se algum painel do CanvasManager esta atualmente aberto
    private bool IsAnyPanelOpen()
    {
        if(CanvasManager.Instance == null) return false;

        foreach(GameObject panel in CanvasManager.Instance.allPanels)
        {
            if(panel != null && panel.activeSelf) return true;
        }
        return false;
    }

    //Executa a interacao padrao: esconde o minimapa e abre o painel do desafio
    protected virtual void Interact()
    {
        if(CanvasManager.Instance == null)
        {
            Debug.LogError("[ObjectInteraction] CanvasManager.Instance não está disponível.");
            return;
        }

        if(challengePanel == null)
        {
            Debug.LogError("[ObjectInteraction] challengePanel não está atribuído no Inspector.");
            return;
        }

        CanvasManager.Instance.ToggleMiniMap(false);
        CanvasManager.Instance.OpenPanel(challengePanel.name);
    }
}