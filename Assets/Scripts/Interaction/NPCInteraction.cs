using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey;
    public Image balloonNPC;

    [Header("Dinamic variable")]
    public PulseOutline pulseObjectInitial;

    [Header("Interaction")]
    public GameObject interactionNotice;

    protected bool playerIsHere = false;
    protected bool isCompleted = false;

    public SimpleDialogue simpleDialogue;
    public NPCDialogueNode firstNode;

    void Start()
    {
        interactionNotice.SetActive(false);
        CheckChallengeStatus();
        if(balloonNPC != null) balloonNPC.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        // Se já tem diálogo ativo no jogo, ninguém mais pode iniciar um novo
        if (!playerIsHere || SimpleDialogue.isSimpleDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            simpleDialogue.pulsingObject = pulseObjectInitial;
            CanvasManager.Instance.ToggleMiniMap(false);
            simpleDialogue.StartDialogue(firstNode);
            
            if (interactionNotice != null) interactionNotice.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;
            CheckChallengeStatus();
     
            if (!isCompleted && interactionNotice != null)
            {
                interactionNotice.SetActive(true);
                balloonNPC.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;
            if (interactionNotice != null) 
            {
                interactionNotice.SetActive(false);
                balloonNPC.gameObject.SetActive(false);
            }
         }
    }

    public void CheckChallengeStatus()
    {
        if (!string.IsNullOrEmpty(uniqueSaveKey))
        {
            isCompleted = (PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1);
        }
    }
}