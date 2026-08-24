using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Inicializa o estado visual do objeto e registra no console
    /// se o dialogo com o Boss ja foi finalizado.
    /// </summary>
    protected virtual void Start()
    {
        //Mensagem de interacao comeca desativada e so sera exibida se a jogadora estiver proxima e o dialogo for concluido
        if(interactionNotice != null) interactionNotice.SetActive(false);

        Debug.Log($"Cena iniciada. Diálogo finalizado: {DialogueManagerBoss.dialogueBossFinished}");
    }

    /// <summary>
    /// Detecta quando a jogadora entra na area de interacao.
    /// A interacao so eh habilitada apos o termino do dialogo com o Boss.
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && DialogueManagerBoss.dialogueBossFinished)
        {
            playerIsHere = true;

            //Atualiza a mensagem de interacao para informar
            //a jogadora que ele pode pressionar a tecla E
            UpdateInteractionNotice();
        }
    }

    /// <summary>
    /// Detecta quando a jogadora deixa a area de interacao e 
    /// remove a mensagem exibida na tela.
    /// </summary>
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;

            if(interactionNotice != null) interactionNotice.SetActive(false);
        }
    }

    /// <summary>
    /// Controla a interacao durante o jogo. Enquanto algum painel
    /// estiver aberto, novas interacoes sao bloqueadas para evitar
    /// conflitos sobre as interfaces.
    /// </summary>
    protected virtual void Update()
    {
        //Impede a interacao com o objeto enquanto outra interface estiver sendo exibida
        if(IsAnyPanelOpen())
        {
            if(interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);

            return;
        }

        //Mantem o aviso de interacao sincronizado com o estado atual
        UpdateInteractionNotice();

        //A interacao eh realizada atraves da tecla E
        if(playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            //A jogadora so pode acessar o desafio depois que o dialogo com o Boss tiver sido finalizado
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

    /// <summary>
    /// Verifica se existe algum painel aberto atraves do CanvasManager.
    /// Isso evita que a jogadora interaja com o objeto enquanto outra UI estiver ativa.
    /// </summary>
    private bool IsAnyPanelOpen()
    {
        if(CanvasManager.Instance == null || CanvasManager.Instance.allPanels == null) return false;

        foreach(GameObject panel in CanvasManager.Instance.allPanels)
        {
            if(panel != null && panel.activeSelf) return true;
        }

        return false;
    }

    /// <summary>
    /// Atualiza a visibilidade do aviso de interacao. O aviso so aparece
    /// quando a jogadora esta dentro da area e o dialogo com o Boss ja
    /// foi concluido.
    /// </summary>
    private void UpdateInteractionNotice()
    {
        if(playerIsHere && DialogueManagerBoss.dialogueBossFinished)
        {
            if(interactionNotice != null && !interactionNotice.activeSelf)
                interactionNotice.SetActive(true);
        }
    }

    /// <summary>
    /// Executa a interacao com o objeto. Esconde o aviso,
    /// fecha o minimapa e abre o painel do desafio.
    /// </summary>
    protected virtual void Interact()
    {
        //Remove o aviso assim que a interacao for iniciada
        if(interactionNotice != null) interactionNotice.SetActive(false);

        //Tratamento de erro
        if(challengePanel == null)
        {
            Debug.LogError("ERRO: Challenge Panel não foi configurado no Inspector!", this);
            return;
        }

        if(CanvasManager.Instance != null)
        {
            //O minimapa eh ocultado para dar destaque ao desafio
            CanvasManager.Instance.ToggleMiniMap(false);

            //Are o painel configurado no Inspector
            CanvasManager.Instance.OpenPanel(challengePanel.name);

            Debug.Log($"Abrindo painel: {challengePanel.name}");
        }
        else
        {
            //Indica se um problema de configuracao caso o CanvasManager nao esteja disponivel na cena
            Debug.LogError("ERRO: CanvasManager.Instance não encontrado!");
        }
    }
}