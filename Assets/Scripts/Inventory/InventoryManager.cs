using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla a UI do inventario: alternancia entre paineis (bolsa e playbook),
/// destaque visual do botao selecionado (borda animada) e o fluxo de finalizacao
/// do jogo (modal do link do CTF e transicao para a cena de creditos).
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Tooltip("Referencia estatica unica para acesso global ao Inventorymanager (padrao Singleton).")]
    public static InventoryManager Instance;

    [Header("Settings - Panels")]
    [Tooltip("Painel de fundo da bolsa/inventario.")]
    public GameObject panelBagBackground;

    [Tooltip("Painel de fundo do playbook.")]
    public GameObject panelPlayBookBackground;

    [Tooltip("Painel principal do inventario, ativado na finalizacao do jogo.")]
    public GameObject inventoryPanel;

    //Indica se o jogo esta aguardando a jogadora confirmar (tecla E) a coleta final das flags
    private bool awaitingFinalConfirmation = false;

    [Header("Settings - Buttons & Border")]
    [Tooltip("RectTransform do botao da bolsa.")]
    public RectTransform buttonBag;

    [Tooltip("RectTransform do botao do playbook.")]
    public RectTransform buttonPlayBook;

    [Tooltip("Imagem de borda que se move para destacar o botao selecionado.")]
    public RectTransform borderImage;

    [Header("End Game - Boss State")]
    [Tooltip("Modal exibido com o link do CTF ao finalizar o jogo (derrotar o Boss).")]
    public GameObject modalLinkCTF;

    [Tooltip("Botao que abre o link do CTF no navegador.")]
    public GameObject buttonLinkCTF;

    [Tooltip("Modal que pergunta se a jogadora quer continuar para os creditos.")]
    public GameObject modalContinueCredits;

    [Tooltip("Velocidade de interpolacao (Lerp) do movimento da borda entre os botoes.")]
    public float velocity = 10f;

    //Botao atualmente selecionado, usado como alvo do Lerp da borda
    private RectTransform currentButton;

    void Awake()
    {
        if(Instance == null) Instance = this;
    }

    void Start()
    {
        //Guarda o estado inicial dos paineis antes de forcar a atualizacao do Canvas, para poder restaura-los exatamente como estavam depois
        bool bagWasAlreadyOpen = panelBagBackground != null && panelBagBackground.activeSelf;
        bool bookWasAlreadyOpen = panelPlayBookBackground != null && panelPlayBookBackground.activeSelf;
        bool borderWasAlreadyActive = borderImage != null && borderImage.gameObject.activeSelf;

        //Ativa temporariamente os paineis para forcar o recalculo do layout da UI
        if(panelBagBackground != null) panelBagBackground.SetActive(true);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(true);

        Canvas.ForceUpdateCanvases(); 

        //Restaura os paineis para o estado original (aberto/fechado) antes do forcamento
        if(panelBagBackground != null) panelBagBackground.SetActive(bagWasAlreadyOpen);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(bookWasAlreadyOpen);
        if(borderImage != null) borderImage.gameObject.SetActive(borderWasAlreadyActive);
        
        //Garante que os elementos de fim de jogo comecem desativados
        if(modalLinkCTF != null) modalLinkCTF.SetActive(false);
        if(buttonLinkCTF != null) buttonLinkCTF.SetActive(false);
        if(modalContinueCredits != null) modalContinueCredits.SetActive(false);
    }

    void Update()
    {
        if(currentButton != null && borderImage != null)
        {
            //Move suavemente a borda de destaque ate a posicao do botao selecionado
            borderImage.position = Vector3.Lerp(
                borderImage.position,
                currentButton.position,
                Time.deltaTime * velocity
            );
        }

        //Enquanto aguarda a confirmacao final, escuta a tecla E para prosseguir para os creditos
        if(awaitingFinalConfirmation && Input.GetKeyDown(KeyCode.E))
        {
            ConfirmarColetaDasFlags();
        }
    }

    //Abre o painel da bolsa e fecha o do playbook, destacando o botao correspondente
    public void OnClickBag()
    {
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(false);
        if(panelBagBackground != null) panelBagBackground.SetActive(true);
        MoveToButton(buttonBag);
    }

    /// <summary>
    /// Abre o painel do playbook e fecha o da bolsa, destacando o botao
    /// correspondente. Nao faz nada se o jogo estiver aguardando confirmacao final.
    /// </summary>
    public void OnClickPlayBook()
    {
        if(awaitingFinalConfirmation) return;

        if(panelBagBackground != null) panelBagBackground.SetActive(false);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(true);
        MoveToButton(buttonPlayBook);
    }

    /// <summary>
    /// Abre a bolsa no estado de finalizacao do jogo (apos derrotar o Boss),
    /// exibindo o modal do link do CTF e o modal de continuacao para os creditos.
    /// </summary>
    public void AbrirBolsaFinalizacaoBoss()
    {
        if(inventoryPanel != null) inventoryPanel.SetActive(true);

        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(false);

        if(modalLinkCTF != null) modalLinkCTF.SetActive(true);
        if(buttonLinkCTF != null) buttonLinkCTF.SetActive(true);
        if(modalContinueCredits != null) modalContinueCredits.SetActive(true);
       
        awaitingFinalConfirmation = true;
        MoveToButton(buttonBag);

        if(panelBagBackground != null) panelBagBackground.SetActive(true);
    }

    //Confirma a coleta final das flags e transiciona para a cena de creitos
    public void ConfirmarColetaDasFlags()
    {
        awaitingFinalConfirmation = false;
        transform.root.gameObject.SetActive(false);
        SceneManager.LoadScene("Credits");
    }

    //Abre o link externo da plataforma do CTF no navegador
    public void OpenCTFLink()
    {
        Application.OpenURL("https://ctf.itaipuparquetec.org.br/");
        Debug.Log("[InventoryManager] Abrindo link do CTF.");
    }

    //Fecha o inventario, desativando paineis e a borda de destaque
    public void ExitInventory()
    {
        awaitingFinalConfirmation = false;

        if(borderImage != null) borderImage.gameObject.SetActive(false);
        if(panelBagBackground != null) panelBagBackground.SetActive(false);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(false);
    }

    /// <summary>
    /// Define o botao atualmente selecionado como alvo para o movimento da
    /// borda de destaque.
    /// </summary>
    public void MoveToButton(RectTransform newButton)
    {
        if(borderImage != null) borderImage.gameObject.SetActive(true);
        currentButton = newButton;
    }
}