using UnityEngine;
using UnityEngine.UI;

public class NPCJoanaInteraction : MonoBehaviour
{
    [Header("Visuals NPC")]
    public Image balloonNPC;

    [Header("Interaction")]
    public GameObject interactionNotice;

    protected bool playerIsHere = false;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    void Start()
    {
        if (interactionNotice != null)
            interactionNotice.SetActive(false);

        if (balloonNPC != null)
            balloonNPC.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!playerIsHere) return;

        if (dialogueManager != null && DialogueManager.isDialogueActive) return;

        if (AreAllFlagsCollected() && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueManager != null)
            {
                CanvasManager.Instance.ToggleMiniMap(false);
                dialogueManager.StartDialogue();
            }

            if (interactionNotice != null)
                interactionNotice.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;

            if (AreAllFlagsCollected())
            {
                if (interactionNotice != null)
                    interactionNotice.SetActive(true);

                if (balloonNPC != null)
                    balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;

            if (interactionNotice != null)
                interactionNotice.SetActive(false);

            if (balloonNPC != null)
                balloonNPC.gameObject.SetActive(false);
        }
    }

    // Verifica se as 8 flags do jogo (excluindo a flag da sala boss) foram coletadas no inventário
    private bool AreAllFlagsCollected()
    {
        return FlagManager.Instance != null && FlagManager.Instance.flagsCapture.Count >= 8;
    }
}
