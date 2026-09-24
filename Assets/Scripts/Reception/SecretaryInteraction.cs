using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Gerencia a interacao entre a jogadora e a NPC Secretaria.
/// Mostra um aviso visual e inicia o dialogo quando a tecla de interacao eh pressionada.
/// </summary>
public class SecretaryInteraction : MonoBehaviour
{
    [Header("UI & Feedback")]
    [Tooltip("Imagem da UI exibida como aviso para a jogadora interagir.")]
    [SerializeField] private Image interactionNotice;

    [Header("References")]
    [Tooltip("Referencia ao gerenciador de dialogos responsavel por iniciar a conversa.")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("State Tracking (Debug)")]
    [Tooltip("Indica se a jogadora esta atualmente dentro da area de interacao.")]
    private bool playerIsHere = false;

    void Start()
    {
        //Oculta o aviso visual antes da interacao iniciar
        if(interactionNotice != null) interactionNotice.gameObject.SetActive(false);
    }

    //Monitora a entrada da jogadora a cada frame para disparar o dialogo
    void Update()
    {
        int playerDone = PlayerPrefs.GetInt(DialogueManager.INICIAL_KEY, 0); //Verifica se o dialogo inicial ja foi feito

        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && playerDone == 0) //Apenas dispara o dialogo se a jogadora estiver perto e clicar na tecla E
        { 
            if(interactionNotice != null) interactionNotice.gameObject.SetActive(false);
    
            if(dialogueManager != null) dialogueManager.StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) //Verifica se eh a player
        {
            int playerDone = PlayerPrefs.GetInt(DialogueManager.INICIAL_KEY, 0);

            if(playerDone == 0)
            {
                playerIsHere = true; //Apenas vira TRUE se for a player

                if(interactionNotice != null) interactionNotice.gameObject.SetActive(true);
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
                interactionNotice.gameObject.SetActive(false);
            }
        }
    }
}