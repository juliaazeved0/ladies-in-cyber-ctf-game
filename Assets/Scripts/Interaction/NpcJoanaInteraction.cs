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

        // Usa a flag real do seu manager
        if (dialogueManager != null && DialogueManager.isDialogueActive) return;

        // Verifica se todas as flags foram coletadas antes de iniciar o diálogo
        if (AreAllFlagsCollected() && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueManager != null)
            {
                CanvasManager.Instance.ToggleMiniMap(false);
                dialogueManager.StartDialogue(); // usa o firstNode interno do manager
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

            if (interactionNotice != null)
                interactionNotice.SetActive(true);

            if (balloonNPC != null)
                balloonNPC.gameObject.SetActive(true);
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

    // Método para verificar se todas as flags foram coletadas
    private bool AreAllFlagsCollected()
    {
        for (int i = 0; i <= 7; i++)
        {
            string flagKey = "flag_" + i;
            if (PlayerPrefs.GetInt(flagKey, 0) != 1)
            {
                return false;
            }
        }
        return true;
    }
}