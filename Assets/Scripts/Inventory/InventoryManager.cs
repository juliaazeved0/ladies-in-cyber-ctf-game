using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Settings - Panels")]
    public GameObject panelBagBackground;
    public GameObject panelPlayBookBackground;
    public GameObject inventoryPanel;

    [Header("Endgame Boss")]
    [SerializeField] private GameObject panelEndgameWarning;
    
    // VARIÁVEL DE CONTROLE: Só permite o "E" se o Boss foi finalizado
    private bool aguardandoConfirmacaoFinal = false;

    [Header("Settings - Buttons & Border")]
    public RectTransform buttonBag;
    public RectTransform buttonPlayBook;
    public RectTransform borderImage;

    [Header("Exibidos no final Boss")]
    public GameObject modalLinkCTF;
    public GameObject buttonLinkCTF;
    public GameObject modalContinueCredits;

    public float velocity = 10f;
    private RectTransform currentButton;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        InitialState();
    }

    void Update()
    {
        // 1. Movimentação visual da borda
        if (currentButton != null && borderImage != null)
        {
            borderImage.position = Vector3.Lerp(
                borderImage.position,
                currentButton.position,
                Time.deltaTime * velocity
            );
        }

        // 2. LOGICA DA TECLA E: Só funciona se aguardandoConfirmacaoFinal for true
        if (aguardandoConfirmacaoFinal && Input.GetKeyDown(KeyCode.E))
        {
            ConfirmarColetaDasFlags();
        }
    }

    private void InitialState()
    {
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
        if (panelEndgameWarning != null) panelEndgameWarning.SetActive(false);
        modalLinkCTF.SetActive(false);
        buttonLinkCTF.SetActive(false);
        modalContinueCredits.SetActive(false);
        aguardandoConfirmacaoFinal = false;
    }

    public void OnClickBag()
    {
        panelPlayBookBackground.SetActive(false);
        panelBagBackground.SetActive(true);
        if (panelEndgameWarning != null) panelEndgameWarning.SetActive(false);
        MoveToButton(buttonBag);
    }

    public void OnClickPlayBook()
    {
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(true);
        MoveToButton(buttonPlayBook);
    }

    /// <summary>
    /// Chamado ao vencer o Boss. Libera o uso da tecla E.
    /// </summary>
    public void AbrirBolsaFinalizacaoBoss()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(true);

        panelPlayBookBackground.SetActive(false);
        panelBagBackground.SetActive(true);
        modalLinkCTF.SetActive(true);
        buttonLinkCTF.SetActive(true);
        modalContinueCredits.SetActive(true);

        if (panelEndgameWarning != null) panelEndgameWarning.SetActive(true);

        // ATIVA A TRAVA: Agora a tecla E pode ser usada
        aguardandoConfirmacaoFinal = true;

        MoveToButton(buttonBag);
    }

    public void ConfirmarColetaDasFlags()
    {
        // Segurança extra: Mesmo chamando via botão, resetamos a trava
        aguardandoConfirmacaoFinal = false;
        SceneManager.LoadScene("Credits");
    }
    
    public void AbrirLinkCTF()
    {
    string url = "https://ctf.itaipuparquetec.org.br/";
    
    // Abre o link no navegador padrão
    Application.OpenURL(url);
    
    Debug.Log("Abrindo link do CTF: " + url);
    }

    public void ExitInventory()
    {
        // Se o jogador sair do inventário, desativamos a trava do E 
        // para ele não ir para os créditos sem querer durante o gameplay normal
        aguardandoConfirmacaoFinal = false;

        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        borderImage.gameObject.SetActive(false);
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
        if (panelEndgameWarning != null) panelEndgameWarning.SetActive(false);
    }

    public void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true);
        currentButton = newButton;
    }
}