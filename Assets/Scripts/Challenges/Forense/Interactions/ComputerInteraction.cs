using UnityEngine;

/// <summary>
/// Controla a interacao da jogadora com um computador interativo no cenario:
/// mostra o aviso e a pulsacao quando a jogadora se aproxima e abre o painel
/// do desafio ao pressionar a tecla de interacao.
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
    [Tooltip("Debug visual em tempo de Play. O valor real eh controlado pelos triggers de entrada e saida.")]
    [SerializeField] private bool playerIsNear = false;

    private void Start()
    {
        //Garante que os elementos da UI comecem escondidos
        if(initialBackground != null) initialBackground.SetActive(false);
        if(interactionNotice != null) interactionNotice.SetActive(false);
    }
    private void Update()
    {
        //Permite interacao se a jogadora estiver dentro da area de trigger e pressionar a tecla E
        if(playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            ExecuteInteraction();
        } 
    }

    //Abre o painel principal do desafio e desliga o aviso
    private void ExecuteInteraction()
    {
        if(initialBackground != null)
        {
            initialBackground.SetActive(true);

            if(interactionNotice != null) interactionNotice.SetActive(false);
            if(scriptPulse != null) scriptPulse.StopPulsing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = true;

            //Destaca o objeto e avisa que ha uma interacao disponivel
            if(scriptPulse != null) scriptPulse.StartPulsing();
            if(interactionNotice != null) interactionNotice.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = false;

            //Remove o destaque e o aviso ao se afastar
            if(scriptPulse != null) scriptPulse.StopPulsing();
            if(interactionNotice != null) interactionNotice.SetActive(false);
        }
    }
}