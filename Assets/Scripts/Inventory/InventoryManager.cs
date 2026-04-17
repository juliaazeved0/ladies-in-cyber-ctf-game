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
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
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
        if (aguardandoConfirmacaoFinal) return;

        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(true);
        MoveToButton(buttonPlayBook);
    }

    public void AbrirBolsaFinalizacaoBoss()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(true);

        panelPlayBookBackground.SetActive(false);

        if (modalLinkCTF != null) modalLinkCTF.SetActive(true);
        if (buttonLinkCTF != null) buttonLinkCTF.SetActive(true);
        if (modalContinueCredits != null) modalContinueCredits.SetActive(true);
       

        aguardandoConfirmacaoFinal = true;

        MoveToButton(buttonBag);

        // Abre por último para não ser fechado pelo OnEnable() dos modals acima.
        panelBagBackground.SetActive(true);
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
        borderImage.gameObject.SetActive(false);
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
    }

    public void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true);
        currentButton = newButton;
    }
}
