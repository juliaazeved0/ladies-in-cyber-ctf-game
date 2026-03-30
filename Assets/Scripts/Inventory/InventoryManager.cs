using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Settings - Panels")]
    public GameObject panelBagBackground;
    public GameObject panelPlayBookBackground;

    [Header("Settings - Buttons & Border")]
    public RectTransform buttonBag;
    public RectTransform buttonPlayBook;
    public RectTransform borderImage;
    public GameObject inventoryPanel;

    public float velocity = 10f;

    private RectTransform currentButton;

void Start()
{
    panelBagBackground.SetActive(false);
    panelPlayBookBackground.SetActive(false);
    borderImage.gameObject.SetActive(false);
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
    }

    void OpenPanel(GameObject panelToOpen)
    {
  
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);

        panelToOpen.SetActive(true);
    }

    public void OnClickBag()
    {
        OpenPanel(panelBagBackground);
        MoveToButton(buttonBag);
    }

    public void OnClickPlayBook()
    {
        OpenPanel(panelPlayBookBackground);
        MoveToButton(buttonPlayBook);
    }

    public void ExitInventory()
    {
        // Fecha apenas os subpainéis e o border, mantendo o painel principal aberto
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
        borderImage.gameObject.SetActive(false);
        currentButton = null;

        // Remove a linha que fecha o painel principal para manter os botões visíveis
        // inventoryPanel.SetActive(false);
    }

    void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true);
        currentButton = newButton;
    }
}