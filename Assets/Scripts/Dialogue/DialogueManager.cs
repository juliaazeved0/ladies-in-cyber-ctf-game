using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia a interface de usuario, fluxo de nos, opcoes de escolha e narracao do sistema de dialogos.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Painel principal da interface de dialogo.")]
    [SerializeField] private GameObject panelDialogue;

    [Tooltip("Texto onde eh exibida a fala do NPC/pergunta atual.")]
    [SerializeField] private TextMeshProUGUI questionText;

    [Tooltip("Imagem do avatar do NPC na interface.")]
    [SerializeField] private Image characterNPC;

    [Tooltip("Array de botoes para as opcoes de resposta do dialogo.")]
    [SerializeField] private Button[] buttonOption;

    [Tooltip("Canvas do minimapa, desativado durante a interacao de dialogo.")]
    [SerializeField] private GameObject miniMapCanvas;

    [Tooltip("Camera do minimapa, desativada durante a interacao de dialogo.")]
    [SerializeField] private GameObject cameraMiniMap;

    [Tooltip("Botao exibido no final do dialogo para reiniciar a conversa.")]
    [SerializeField] private Button buttonPlayAgain;

    [Tooltip("Texto que exibe o nome da jogadora na UI de dialogo.")]
    [SerializeField] private TextMeshProUGUI playerNameText;

    [Tooltip("Botao exibido para finalizar o dialogo e salvar o progresso.")]
    [SerializeField] private Button buttonDone;

    [Tooltip("Botao para fechar/sair do dialogo sem salvar alteracoes.")]
    [SerializeField] private Button buttonExit;

    [Tooltip("Imagem indicadora de bloqueio (cadeado) na UI.")]
    [SerializeField] private GameObject lockImage;

    [Tooltip("Texto do NPC na cena/mundo apos o termino do dialogo.")]
    [SerializeField] private TextMeshProUGUI dialogueNPC;

    [Header("References")]
    [Tooltip("Componente responsavel pelo efeito de escrita gradativa do texto.")]
    [SerializeField] private WriteMachine writeMachine;

    [Tooltip("Componente responsavel pela exibicao da placa de nome da jogadora.")]
    [SerializeField] private PlayerNameplate playerNameplate;

    [Header("Dialogue Nodes")]
    [Tooltip("No inicial de dialogo a ser executado.")]
    [SerializeField] private DialogueNode firstNode;

    [SerializeField] private bool requireAllMainFlags;

    public bool IsSuccessfulConclusion => dialogueCurrent != null &&
        dialogueCurrent.buttonType == ButtonType.Done &&
        (dialogueCurrent.nextDialogue == null || dialogueCurrent.nextDialogue.Length == 0) &&
        panelDialogue != null && panelDialogue.activeInHierarchy;

    private DialogueNode dialogueCurrent;
    private DialogueNode pendingNextNode;

    [Header("Narrator UI")]
    [Tooltip("Painel de exibicao das falas do narrador.")]
    [SerializeField] private GameObject panelNarrator;

    [Tooltip("Texto que exibe a mensagem do narrador.")]
    [SerializeField] private TextMeshProUGUI textNarrator;

    [Tooltip("Botao para avancar na caixa do narrador.")]
    [SerializeField] private Button buttonNextNarrator;

    [Header("Game State")]
    [Tooltip("Indica globalmente se o sistema de dialogo esta ativo no momento.")]
    public static bool isDialogueActive = false;

    [Header("World Objects")]
    [Tooltip("Objeto de bloqueio no mundo (ex: escada) liberado apos o dialogo.")]
    [SerializeField] private GameObject lockLadder;

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    public const string INICIAL_KEY = "dialogueInicial";

    //Configura os estados iniciais da UI, cadastra ouvintes de evento e recupera dados salvos
    void Start()
    {
        panelDialogue.SetActive(false);

        if(panelNarrator != null) panelNarrator.SetActive(false);
        if(buttonNextNarrator != null) buttonNextNarrator.onClick.AddListener(OnClickNextNarrator);

        buttonExit.gameObject.SetActive(false);

        int dialogueInicialDone = PlayerPrefs.GetInt(INICIAL_KEY, 0);

        if(!requireAllMainFlags && dialogueInicialDone == 1)
        {
            lockImage.gameObject.SetActive(false);
            
            if(lockLadder != null) 
            {
                lockLadder.SetActive(false);
            }
        }
        
        string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");

        if(playerNameText != null)
        {
            playerNameText.text = playerName.ToUpper();
        }
    }

    //Inicia a sequencia de dialogo a partir do no inicial configurado
    public void StartDialogue()
    {
        if(requireAllMainFlags && !FlagManager.HasAllMainFlags()) return;
        if(firstNode != null)
        {
            isDialogueActive = true;

            panelDialogue.SetActive(true);
            miniMapCanvas.SetActive(false);
            cameraMiniMap.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            DialogueView(firstNode);
        }
        else
        {
            Debug.LogWarning("O nó inicial (firstNode) não foi atribuído no Inspector!");
        }
    }

    //Atualiza os componentes graficos do painel de dialogo com as informacoes do no atual
    public void DialogueView(DialogueNode node)
    {
        dialogueCurrent = node;
        writeMachine.Run(node.question, questionText);

        bool isLastNode = (node.nextDialogue.Length == 0);

        if(isLastNode)
        { 
            buttonPlayAgain.gameObject.SetActive(false);
            buttonDone.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(false);

            if(node.buttonType == ButtonType.PlayAgain)
            {
                buttonPlayAgain.gameObject.SetActive(true);
            }
            else if(node.buttonType == ButtonType.Done)
            {
                buttonDone.gameObject.SetActive(true);
            }
            else 
            {
                buttonExit.gameObject.SetActive(true);
            }
        }
        else
        {
            buttonDone.gameObject.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(true);
        }

        for(int i = 0; i < buttonOption.Length; i++)
        {
            if(i < node.options.Length)
            {
                buttonOption[i].gameObject.SetActive(true);
                buttonOption[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i];
            }
            else
            { 
                buttonOption[i].gameObject.SetActive(false);
            }
        }
    }

    //Finaliza a interacao de dialogo salvando a conclusao no PlayerPrefs e liberando acessos na cena
    public void OnClickDone()
    {
        // A Joana libera a pista pelo UnlockBossRoom; nunca usa a trava da recepcao.
        if(requireAllMainFlags) return;
        isDialogueActive = false; 

        PlayerPrefs.SetInt(INICIAL_KEY, 1);
        PlayerPrefs.Save();

        panelDialogue.SetActive(false);
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);
    
        if(lockImage != null) lockImage.gameObject.SetActive(false);
        
        dialogueNPC.text = "Bem-vinda ao Centro de Tecnologia do Itaipu Parquetec!";
        
        if(playerNameplate != null) playerNameplate.SetNameplateIdPlayer();
        if(lockLadder != null) lockLadder.SetActive(false);
    }

    //Encerra a interface de dialogo sem marcar o fluxo como concluido
    public void OnClickExit()
    {
        isDialogueActive = false; 

        panelDialogue.SetActive(false);
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);
    }

    //Reinicia o dialogo a partir da primeira fala
    public void DialoguePlayAgain()
    {
        StartDialogue();
    }

    //Processa a escolha de uma opcao pela jogadora e direciona para o proximo no 
    public void ChooseOption(int index)
    {
        pendingNextNode = dialogueCurrent.nextDialogue[index];

        if(dialogueCurrent.narratorFeedbacks != null && index < dialogueCurrent.narratorFeedbacks.Length)
        {
            textNarrator.text = dialogueCurrent.narratorFeedbacks[index];
        }

        for(int i = 0; i < buttonOption.Length; i++)
        {
            buttonOption[i].gameObject.SetActive(false);
        }

        panelNarrator.SetActive(true);
    }

    //Avanca a mensagem exibida pelo narrador e direciona para o proximo no ou fecha o painel
    public void OnClickNextNarrator()
    {
        panelNarrator.SetActive(false);
        if(pendingNextNode != null)
        {
            DialogueView(pendingNextNode);
        }
        else
        {
            isDialogueActive = false; 

            panelDialogue.SetActive(false);
            miniMapCanvas.SetActive(true);
            cameraMiniMap.SetActive(true);
        }
    }
}