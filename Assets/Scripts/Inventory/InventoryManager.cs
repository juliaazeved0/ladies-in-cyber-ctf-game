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

    // ⭐ FUNÇÃO CENTRAL (evita sobreposição)
    void OpenPanel(GameObject panelToOpen)
    {
        // Fecha TODOS primeiro
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);

        // Abre só o escolhido
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
        // Fecha tudo
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);

        borderImage.gameObject.SetActive(false);
        currentButton = null;

        inventoryPanel.SetActive(false);
    }

    void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true);
        currentButton = newButton;
    }
}