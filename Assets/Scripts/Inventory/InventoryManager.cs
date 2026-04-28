using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

/// <summary>
/// Gerencia a interface do inventario, alterando entre a Bolsa de Flags e o Livro,
/// e controla a transicao para o final do jogo.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Settings - Panels")]
    public GameObject panelBagBackground;
    public GameObject panelPlayBookBackground;
    public GameObject inventoryPanel;

    private bool aguardandoConfirmacaoFinal = false;

    [Header("Settings - Buttons & Border")]
    public RectTransform buttonBag;
    public RectTransform buttonPlayBook;
    public RectTransform borderImage;

    [Header("End Game - Boss State")]
    public GameObject modalLinkCTF;
    public GameObject buttonLinkCTF;
    public GameObject modalContinueCredits;

    public float velocity = 10f;
    private RectTransform currentButton;

    void Awake()
    {
        if(Instance == null) Instance = this;
    }

    void Start()
    {
        //Truque de "Layout": Forca a ativacao/desativacao para que a Unity
        //calcule as posicoes ds elementos de UI corretamente antes da jogadora ver
        bool bagJaEstavaAberta = panelBagBackground != null && panelBagBackground.activeSelf;
        bool bookJaEstavaAberto = panelPlayBookBackground != null && panelPlayBookBackground.activeSelf;
        bool bordaJaEstavaAtiva = borderImage != null && borderImage.gameObject.activeSelf;

        if(panelBagBackground != null) panelBagBackground.SetActive(true);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(true);

        Canvas.ForceUpdateCanvases(); 

        //Restaurando o estado real
        if(panelBagBackground != null) panelBagBackground.SetActive(bagJaEstavaAberta);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(bookJaEstavaAberto);
        if(borderImage != null) borderImage.gameObject.SetActive(bordaJaEstavaAtiva);
        
        //Limpeza inicial de estados do Boss
        if(modalLinkCTF != null) modalLinkCTF.SetActive(false);
        if(buttonLinkCTF != null) buttonLinkCTF.SetActive(false);
        if(modalContinueCredits != null) modalContinueCredits.SetActive(false);
    }

    void Update()
    {
        if (currentButton != null && borderImage != null)
        {
            borderImage.position = Vector3.Lerp(
                borderImage.position,
                currentButton.position,
                Time.deltaTime * velocity
            );
        }

        if (aguardandoConfirmacaoFinal && Input.GetKeyDown(KeyCode.E))
        {
            ConfirmarColetaDasFlags();
        }
    }

    private void InitialState()
    {
        if (panelBagBackground != null) panelBagBackground.SetActive(false);
        if (panelPlayBookBackground != null) panelPlayBookBackground.SetActive(false);
        if (modalLinkCTF != null) modalLinkCTF.SetActive(false);
        if (buttonLinkCTF != null) buttonLinkCTF.SetActive(false);
        if (modalContinueCredits != null) modalContinueCredits.SetActive(false);
        aguardandoConfirmacaoFinal = false;
    }

    public void OnClickBag()
    {
        panelPlayBookBackground.SetActive(false);
        panelBagBackground.SetActive(true);
        MoveToButton(buttonBag);
    }

    public void OnClickPlayBook()
    {
        if(aguardandoConfirmacaoFinal) return;

        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(true);
        MoveToButton(buttonPlayBook);
    }

    public void AbrirBolsaFinalizacaoBoss()
    {
        if(inventoryPanel != null) inventoryPanel.SetActive(true);

        panelPlayBookBackground.SetActive(false);

        if(modalLinkCTF != null) modalLinkCTF.SetActive(true);
        if(buttonLinkCTF != null) buttonLinkCTF.SetActive(true);
        if(modalContinueCredits != null) modalContinueCredits.SetActive(true);
       
        aguardandoConfirmacaoFinal = true;
        MoveToButton(buttonBag);

        if(panelBagBackground != null) panelBagBackground.SetActive(true);
    }

    public void ConfirmarColetaDasFlags()
    {
        aguardandoConfirmacaoFinal = false;
        transform.root.gameObject.SetActive(false);
        SceneManager.LoadScene("Credits");
    }

    public void AbrirLinkCTF()
    {
        Application.OpenURL("https://ctf.itaipuparquetec.org.br/");
        Debug.Log("[InventoryManager] Abrindo link do CTF.");
    }

    public void ExitInventory()
    {
        aguardandoConfirmacaoFinal = false;
        if(borderImage != null) borderImage.gameObject.SetActive(false);
        if(panelBagBackground != null) panelBagBackground.SetActive(false);
        if(panelPlayBookBackground != null) panelPlayBookBackground.SetActive(false);
    }

    public void MoveToButton(RectTransform newButton)
    {
        if(borderImage != null) borderImage.gameObject.SetActive(true);
        currentButton = newButton;
    }
}