using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controla um dialogo simples com NPC: exibe o painel de dialogo, escreve o
/// texto com efeito de maquina de escrever (WriteMachine), avanca entre nos
/// de dialogo (NPCDialogueNode) e finaliza destacando um objeto (PulseOutline),
/// se configurado.
/// </summary>
public class SimpleDialogue : MonoBehaviour
{
    [Header("Elements UI")]
    [Tooltip("Painel de UI que exibe o dialogo.")]
    public GameObject panelDialogue;

    [Tooltip("Texto onde a fala do NPC eh exibida.")]
    public TextMeshProUGUI textDialogue;

    [Tooltip("Imagem do retrato/sprite do NPC durante o dialogo.")]
    public Image characterNPC;

    [Tooltip("Componente responsavel pelo efeito de escrita (maquina de escrever) do texto.")]
    public WriteMachine writeMachine;

    [Tooltip("Texto que exibe o nome da jogadora (carregado do PlayerPrefs).")]
    public TextMeshProUGUI playerNameplate;

    [Tooltip("Imagem do retrato/sprite da jogadora durante o dialogo.")]
    public Image characterPlayer;

    [Tooltip("Canvas do minimapa, ocultado durante o dialogo.")]
    public GameObject miniMapCanvas;

    [Tooltip("Camera do minimapa, ocultada durante o dialogo.")]
    public GameObject cameraMiniMap;

    [Tooltip("Botao exibido ao final do dialogo para confirmar/prosseguir.")]
    public Button confirmButton;

    [Header("Dynamic Variables")]
    [Tooltip("Objeto que recebera destaque visual apos o termino do dialogo.")]
    public PulseOutline pulsingObject;

    //Impede que a jogadora avance o dialogo nos primeirs instantes
    private bool readyToSpeak = false;

    //Indica se um dialogo esta atualmente em andamento
    protected bool isTalking = false;

    [Header("Buttons")]
    [Tooltip("Botao para sair do dialogo antes de chegar ao fim.")]
    public Button buttonExit;

    [Header("Nodes")]
    [Tooltip("No inicial do dialogo, atribuido ao chamar StartDialogue.")]
    public NPCDialogueNode firstNode;

    //No de dialogo atualmente exibido
    protected NPCDialogueNode dialogueCurrent;

    [Header("Control Inventory")]
    [Tooltip("Indica globalmente se algum SimpleDialogue esta ativo (usado por outros sistemas, como o inventario).")]
    public static bool isSimpleDialogueActive = false;

    //Chave usada para ler o nome da jogadora salvo no PlayerPrefs
    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Update()
    {
        if(!readyToSpeak || !isTalking)
        {
            return;
        }

        //Avanca o dialogo ao pressionar E, apenas se o painel estiver realmente aberto
        if(panelDialogue!= null && panelDialogue.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            NextTalk();
        }
    }

    void Start()
    {
        //Carrega o nome da jogadora salvo (ou usa "Jogadora" como padrao) e exibe em maiusculas
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");

        if(playerNameplate != null)
        {
            playerNameplate.text = namePlayer.ToUpper();
        }
        else
        {
            Debug.LogError("[SimpleDialogue] playerNameplate não está atribuído no Inspector.");
        }
    }

    /// <summary>
    /// Inicia o dialogo a partir do no informado, abrindo o painel e
    /// ocultando o minimapa enquanto o dialogo estiver ativo.
    /// </summary>
    public void StartDialogue(NPCDialogueNode inicialNode)
    {
        StopAllCoroutines();

        isSimpleDialogueActive = true;
        isTalking = true;

        if(textDialogue != null) textDialogue.text = "";
        if(confirmButton != null) confirmButton.gameObject.SetActive(false);
        if(buttonExit != null) buttonExit.gameObject.SetActive(true);

        firstNode = inicialNode;

        if(firstNode != null)
        {
            if(CanvasManager.Instance != null && panelDialogue != null)
            {
                CanvasManager.Instance.OpenPanel(panelDialogue.name);
                CanvasManager.Instance.ToggleMiniMap(false);
            }
            else
            {
                Debug.LogError("[SimpleDialogue] CanvasManager.Instance ou panelDialogue não estão disponíveis.");
            }

            DialogueView(firstNode);

            //Bloqueia o avanco do dialogo por um instante para evitar pular a primeira fala
            readyToSpeak = false;
            StartCoroutine(ReleaseInput());
        }
        else
        {
            Debug.LogError("[SimpleDialogue] node vazio: nenhum nó inicial foi informado.");
        }
    }

    //Libera a entrada da jogadora (tecla E) apos um pequeno atraso
    IEnumerator ReleaseInput()
    {
        yield return new WaitForSeconds(0.2f);
        readyToSpeak = true;
    }

    /// <summary>
    /// Exibe o no de dialogo informado: atualiza o texto (via WriteMachine)
    /// e o retrato do NPC.
    /// </summary>
    public void DialogueView(NPCDialogueNode node)
    {
        dialogueCurrent = node;

        if(writeMachine != null)
        {
            writeMachine.Run(node.talkNPC, textDialogue);
        }
        else
        {
            Debug.LogError("[SimpleDialogue] writeMachine não está atribuído no Inspector.");
        }

        if(characterNPC != null)
            characterNPC.sprite = node.characterNPC;
    }

    /// <summary>
    /// Avanca para a proxima fala/no do dialogo, ou completa instantaneamente
    /// o texto se ainda estiver sendo digitado. Ao chegar ao fim, exibe o
    /// botao de confirmacao e oculta o de saida.
    /// </summary>
    public virtual void NextTalk()
    {
        if(writeMachine == null)
        {
            Debug.LogError("[SimpleDialogue] writeMachine não está atribuído no Inspector.");
            return;
        }

        //Se o texto ainda esta sendo "digitado", completa instantaneamente em vez de avancar
        if(writeMachine.IsTyping)
        {
            writeMachine.Complete();
            return;
        }

        if(dialogueCurrent == null)
        {
            Debug.LogError("[SimpleDialogue] dialogueCurrent está nulo ao tentar avançar o diálogo.");
            return;
        }

        if(dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
        else
        {
            //Fim do dialogo: mostra confirmar, esconde sair
            if(confirmButton != null)
                confirmButton.gameObject.SetActive(true);

            if(buttonExit != null)
                buttonExit.gameObject.SetActive(false);
        }
    }

    //Encerra o dialogo, fechando o painel e reexibindo o minimapa
    public void ExitDialogue()
    {
        isSimpleDialogueActive = false;
        isTalking = false;

        if(CanvasManager.Instance != null && panelDialogue != null)
        {
            CanvasManager.Instance.ClosedPanel(panelDialogue.name);
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        else
        {
            Debug.LogError("[SimpleDialogue] CanvasManager.Instance ou panelDialogue não estão disponíveis.");
        }
    }

    /// <summary>
    /// Confirma o fim da ajuda/dialogo: fecha o dialogo e, se houver um
    /// objeto configurado para destaque, inicia o efeito de pulso nele.
    /// </summary>
    public virtual void ConfirmHelp()
    {
        ExitDialogue();

        if(pulsingObject != null)
        {
            pulsingObject.StartPulsing();
            pulsingObject = null;
        }
    }
}