using UnityEngine;

/// <summary>
/// Gerencia a interacao da player com um "computador" na cena: detecta
/// aproximacao via trigger 2D, mostra aviso visual e outline pulsante,
/// e abre o painel de desafio ao pressionar a tecla de interacao.
/// </summary>
public class ComputerInteraction : MonoBehaviour
{
    [Header("Visual Effects")]
    [Tooltip("Script responsavel pelo efeito de brilho no objeto.")]
    [UnityEngine.Serialization.FormerlySerializedAs("scriptPulse")]
    public PulseOutline pulseOutline;

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
        if(initialBackground != null) initialBackground.SetActive(false);
        if(interactionNotice != null) interactionNotice.SetActive(false);
    }
    private void Update()
    {
        if(playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            ExecuteInteraction();
        } 
    }

    private void ExecuteInteraction()
    {
        if(initialBackground == null)
        {
            Debug.LogWarning($"{gameObject.name}: InitialBackground não atribuído. Interação abortada.");
            return;
        }

        initialBackground.SetActive(true);

        if(interactionNotice != null) interactionNotice.SetActive(false);
        if(pulseOutline != null) pulseOutline.StopPulsing();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = true;

            if(pulseOutline != null) pulseOutline.StartPulsing();
            if(interactionNotice != null) interactionNotice.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = false;

            if(pulseOutline != null) pulseOutline.StopPulsing();
            if(interactionNotice != null) interactionNotice.SetActive(false);
        }
    }
}