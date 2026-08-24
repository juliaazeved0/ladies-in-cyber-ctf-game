using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla a interacao da jogadora com um NPC da sala do boss:
/// gerencia balao de fala, aviso de interacao, inicio e avanco 
/// de dialogo e persistencia do estado do desafio.
/// </summary>
public class NPCBossInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    [Tooltip("Chave unica usada para salvar/verificar se esse desafio ja foi completado.")]
    public string uniqueSaveKey;

    [Tooltip("Balao visual exibido acima do NPC enquanto a jogadora esta por perto.")]
    public Image balloonNPC;

    [Header("Interaction")]
    [Tooltip("Aviso visual indicando que eh possivel interagir com o NPC.")]
    public GameObject interactionNotice;

    [Header("Systems")]
    [Tooltip("Gerenciador de dialogo responsavel por exibir e avancar as falas do boss.")]
    public DialogueManagerBoss dialogueManagerBoss;

    [Header("Nodes")]
    [Tooltip("Primeiro no de dialogo a ser exibido ao iniciar a conversa com esse NPC.")]
    public DialogueNodeBoss firstNodeBoss;

    [Header("External Blockers")]
    [Tooltip("Painel externo que, se ativo, bloqueia qualquer interacao com o NPC.")]
    public GameObject terminalPanel;

    private bool playerIsHere = false;
    private bool isCompleted = false;
    private bool isTalking = false;

    void Start()
    {
        if(interactionNotice != null) interactionNotice.SetActive(false);
        if(balloonNPC != null) balloonNPC.gameObject.SetActive(false);

        //Verifica logo no inicio se esse desafio ja foi concluido em uma sessao anterior
        CheckChallengeStatus();
    }

    void Update()
    {
        //Enquanto o painel do terminal estiver ativo, ignora completamente a interacao com o NPC
        if(terminalPanel != null && terminalPanel.activeSelf)
        {
            return;
        }

        //Sem essa referencia, nenhuma logica de dialogo pode ser executada
        if(dialogueManagerBoss == null)
        {
            return;
        }

        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            if(!isTalking && !dialogueManagerBoss.panelDialogue.activeSelf)
            {
                //Nenhuma conversa em andamento: inicia o dialogo
                StartConversation();
                isTalking = true;

                if(interactionNotice != null) interactionNotice.SetActive(false);
            }
            else if(dialogueManagerBoss.panelDialogue.activeSelf)
            {
                //Cada tecla E avanca a conversa de formas diferentes
                if(dialogueManagerBoss.writeMachine.IsTyping)
                {
                    //Completa a linha instantaneamente em vez de esperar
                    dialogueManagerBoss.writeMachine.Complete();
                }
                else if(!dialogueManagerBoss.CurrentNodeHasOptions())
                {
                    //Avanca automaticamente pela unica opcao disponivel (indice 0)
                    dialogueManagerBoss.ChooseOption(0);
                }
                else
                {
                    //A escolha deve ser pelo mouse
                    Debug.Log("Escolha uma opção no mouse para continuar!");
                }
            }
        }

        //Detecta quando o painel de dialogo foi fechado externamente para resetar o estado local e mostrar o aviso
        if(isTalking && !dialogueManagerBoss.panelDialogue.activeSelf)
        {
            isTalking = false;

            if(playerIsHere && interactionNotice != null) interactionNotice.SetActive(true);
        }
    }

    /// <summary>
    /// Configura o no inicial no DialogueManagerBoss e dispara o
    /// inicio da conversa com esse NPC especifico.
    /// </summary>
    private void StartConversation()
    {
        if(dialogueManagerBoss != null && firstNodeBoss != null)
        {
            dialogueManagerBoss.firstNode = firstNodeBoss;
            dialogueManagerBoss.StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;

            //Reavalia o status do desafio ao entrar na area
            CheckChallengeStatus();

            //Evita NullReferenceException caso a referencia nao tenha sido preenchida no Inspector
            if(dialogueManagerBoss == null)
            {
                Debug.LogWarning($"{gameObject.name} está sem referência ao DialogueManagerBoss!");
                return;
            }

            //Mostra o aviso se o desafio ainda nao foi completado e nao houver outro dialogo ja aberto na tela
            if(!isCompleted && !dialogueManagerBoss.panelDialogue.activeSelf)
            {
                if(interactionNotice != null) interactionNotice.SetActive(true);
                if(balloonNPC != null) balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;
            isTalking = false;

            if(interactionNotice != null) interactionNotice.SetActive(false);
            if(balloonNPC != null) balloonNPC.gameObject.SetActive(false);

            //Fecha o dialogo a forca caso a jogadora se afaste no meio de uma conversa
            if(dialogueManagerBoss != null) dialogueManagerBoss.panelDialogue.SetActive(false);
        }
    }

    /// <summary>
    /// Consulta o PlayerPrefs para verificar se o desafio 
    /// associado a esse NPC ja foi completado anteriormente.
    /// </summary>
    public void CheckChallengeStatus()
    {
        if(!string.IsNullOrEmpty(uniqueSaveKey))
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
    }
}