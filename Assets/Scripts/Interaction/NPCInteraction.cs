using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gerencia a interacao entre a jogadora e um NPC, controlando a ativacao do balao
/// de fala, avisos de interacao e o inicio do sistema de dialogo.
/// </summary>
public class NPCInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    [Tooltip("Chave unica para verificar no PlayerPrefs se a missao deste NPC ja foi concluida.")]
    public string uniqueSaveKey;

    [Tooltip("Imagem do balao de dialogo exibido acima da cabeca do NPC.")]
    public Image balloonNPC;

    [Header("Dynamic Variables")]
    [Tooltip("Componente de efeito visual que faz o objeto associado pulsar.")]
    public PulseOutline pulseObjectInitial;

    [Tooltip("Aviso de interface (UI) exibido quando o jogador pode interagir.")]
    public GameObject interactionNotice;

    [Header("Systems")]
    [Tooltip("Gerenciador do sistema de dialogo simplificado.")]
    public SimpleDialogue simpleDialogue;

    [Tooltip("No inicial de dialogo deste NPC.")]
    public NPCDialogueNode firstNode;

    [Header("State Tracking")]
    [Tooltip("Indica se a jogadora esta dentro do alcance de interacao.")]
    protected bool playerIsHere = false;

    [Tooltip("Indica se o desafio deste NPC ja foi concluido.")]
    protected bool isCompleted = false;

    void Start()
    {
        interactionNotice.SetActive(false);
        CheckChallengeStatus();
        if(balloonNPC != null) balloonNPC.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        //Se ja existe um dialogo ativo na cena, bloqueia novas interacoes
        if(CanvasManager.Instance != null && CanvasManager.Instance.IsAnyPanelOpen()) return;
        if(!playerIsHere || SimpleDialogue.isSimpleDialogueActive) return;

        //Inicia o dialogo ao pressionar a tecla E, caso a tarefa nao esteja concluida
        if(Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            //Passa o objeto que deve pulsar (ajuda visual) para o sistema de dialogo
            simpleDialogue.pulsingObject = pulseObjectInitial;

            CanvasManager.Instance.ToggleMiniMap(false);
            simpleDialogue.StartDialogue(firstNode);
            
            if(interactionNotice != null) interactionNotice.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;
            CheckChallengeStatus();
     
            //Se o desafio do NPC ainda esta pendente, mostra os avisos visuais
            if(!isCompleted && interactionNotice != null)
            {
                interactionNotice.SetActive(true);
                balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;
            if(interactionNotice != null) 
            {
                interactionNotice.SetActive(false);
                balloonNPC.gameObject.SetActive(false);
            }
         }
    }

    //Consulta o PlayerPrefs para saber se o progresso associado a este NPC foi salvo
    public void CheckChallengeStatus()
    {
        if(!string.IsNullOrEmpty(uniqueSaveKey))
        {
            //Padrao: 0 = incompleto, 1 = completo
            isCompleted = (PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1);
        }
    }
}