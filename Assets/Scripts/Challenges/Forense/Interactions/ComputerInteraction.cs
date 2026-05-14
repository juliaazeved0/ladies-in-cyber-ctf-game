using UnityEngine;

/// <summary>
/// Gerencia a interacao inicial com o computador dos desafios de Forense.
/// Controla efeitos de brilho e a abertura da interface dos desafios.
/// </summary>
public class ComputerInteraction : MonoBehaviour
{
    [Header("Visual Effects")]
    [Tooltip("Script responsavel pelo efeito de brilho no objeto.")]
    public PulseOutline scriptPulse;

    [Header("UI Interaction")]
    [Tooltip("Aviso visual.")]
    public GameObject interactionNotice;

    [Tooltip("Painel principal que indica o desafio.")]
    public GameObject initialBackground;

    [Header("State")]
    [SerializeField] private bool playerIsNear = false;

    private void Start()
    {
        //Garante que o desafio comece oculto
        if(initialBackground != null) initialBackground.SetActive(false);
        if(interactionNotice != null) interactionNotice.SetActive(false);
    }
    private void Update()
    {
        //Verifica a entrada da jogadora apenas se ela estiver na area
        if(playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            ExecuteInteraction();
        } 
    }

    /// <summary>
    /// Ativa a interface do desafio e oculta avisos temporarios.
    /// </summary>
    private void ExecuteInteraction()
    {
        if(initialBackground != null)
        {
            initialBackground.SetActive(true);

            //Esconde o aviso para nao sobrepor a interface do desafio
            if(interactionNotice != null) interactionNotice.SetActive(false);

            //Desligar o brilho ao abrir o menu para poupar processamento
            if(scriptPulse != null) scriptPulse.StopPulsing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = true;

            //Feedback visual de que o objeto eh interativo
            if(scriptPulse != null) scriptPulse.StartPulsing();
            if(interactionNotice != null) interactionNotice.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = false;

            //Remove feedbacks visuais caso a jogadora se afaste
            if(scriptPulse != null) scriptPulse.StopPulsing();
            if(interactionNotice != null) interactionNotice.SetActive(false);
        }
    }
}