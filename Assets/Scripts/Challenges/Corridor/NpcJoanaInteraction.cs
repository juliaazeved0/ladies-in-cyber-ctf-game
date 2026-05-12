using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gerencia a interacao com a NPC Joana.
/// Libera o dialogo final apenas apos a coleta das 8 flags principais.
/// </summary>
public class NPCJoanaInteraction : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("Balao visual que aparece sobre a NPC.")]
    public Image balloonNPC;

    [Tooltip("Aviso visual para a jogadora.")]
    public GameObject interactionNotice;

    [Header("Dialogue Reference")]
    public DialogueManager dialogueManager;

    [Header("Debug & State")]
    [SerializeField] protected bool playerIsHere = false;

    [Tooltip("Se marcado, ignora a contagem de flags para testes.")]
    public bool debugMode = false;

    void Start()
    {
        //Garante que os avisos comecem desativados
        if(interactionNotice != null)
            interactionNotice.SetActive(false);

        if(balloonNPC != null)
            balloonNPC.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if(!playerIsHere) return;

        //Impede nova interacao se um dialogo ja estiver ocorrendo
        if(dialogueManager != null && DialogueManager.isDialogueActive) return;

        //Verifica condicao de vitoria/progresso (8 flags)
        if((AreAllFlagsCollected() || debugMode) && Input.GetKeyDown(KeyCode.E))
        {
            if(dialogueManager != null)
            {
                CanvasManager.Instance.ToggleMiniMap(false);
                dialogueManager.StartDialogue();
            }

            if(interactionNotice != null)
                interactionNotice.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;

            //So mostra feedback visual se a jogadora cumpriu os requisitos ou em debug
            if(AreAllFlagsCollected() || debugMode)
            {
                if(interactionNotice != null)
                    interactionNotice.SetActive(true);

                if(balloonNPC != null)
                    balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;

            if(interactionNotice != null)
                interactionNotice.SetActive(false);

            if(balloonNPC != null)
                balloonNPC.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Verifica se as 8 flags do jogo (excluindo a flag da Sala Boss) foram coletadas.
    /// </summary>
    /// <returns></returns>
    private bool AreAllFlagsCollected()
    {
        return FlagManager.Instance != null && FlagManager.Instance.flagsCapture.Count >= 8;
    }
}