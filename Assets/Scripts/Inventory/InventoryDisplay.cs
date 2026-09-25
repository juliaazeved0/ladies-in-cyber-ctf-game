using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Gerencia a exibicao visual das flags capturadas no inventario,
/// controlando a paginacao e a ativacao dos slots de texto.
/// </summary>
public class InventoryDisplay : MonoBehaviour
{
    [Header("UI Text Slots")]
    [Tooltip("Slots de texto para exibir as flags obtidas.")]
    public TextMeshProUGUI[] textSlots;

    [Header("UI Lock Icons (Empty State)")]
    [Tooltip("Icones de cadeado exibidos em slots onde as flags ainda nao foram desbloqueadas.")]
    public GameObject[] lockIcons;

    [Header("Copy Buttons")]
    [Tooltip("Botoes para copiar a flag do slot correspondente para a area de transferencia.")]
    public GameObject[] copyButtons;

    [Header("Navigation Buttons")]
    [Tooltip("Botao para avançar para a proxima pagina.")]
    public Button btnNext;

    [Tooltip("Botao para retornar a pagina anterior.")]
    public Button btnPrevious;

    [Header("Settings")]
    [Tooltip("Quantidade total de flags disponiveis no jogo.")]
    private const int TOTAL_FLAGS = 9;
    private int currentPage = 0;


    void OnEnable()
    {
        currentPage = 0;

        if (FlagManager.Instance != null)
        {
            UpdateSlots();
        }
        else
        {
            Debug.LogWarning($"[InventoryDisplay] Instância de 'FlagManager' não encontrada na cena.", this);
        }
    }

    //Avanca para a proxima pagina do inventario
    public void NextPage()
    {
        int totalPages = Mathf.CeilToInt((float)TOTAL_FLAGS / textSlots.Length);

        if (currentPage < totalPages - 1)
        {
            currentPage++;
            UpdateSlots();
        }
    }

    //Retorna para a pagina anterior
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateSlots();
        }
    }

    //Atualiza os slots de texto, icones de cadeado e botoes de copia baseando-se na pagina atual
    private void UpdateSlots()
    {
        List<string> capturedFlags = FlagManager.Instance.flagsCapture;
        int startIndex = currentPage * textSlots.Length;

        for (int i = 0; i < textSlots.Length; i++)
        {
            int flagIndex = startIndex + i;

            if (flagIndex < TOTAL_FLAGS)
            {
                //Verifica se ha uma flag salva para o indice atual
                if (flagIndex < capturedFlags.Count)
                {
                    if (textSlots[i] != null)
                    {
                        textSlots[i].gameObject.SetActive(true);
                        textSlots[i].text = capturedFlags[flagIndex];
                    }

                    if (i < lockIcons.Length && lockIcons[i] != null) lockIcons[i].SetActive(false);

                    if (i < copyButtons.Length && copyButtons[i] != null)
                    {
                        copyButtons[i].SetActive(true);
                        SetupCopyButton(copyButtons[i], capturedFlags[flagIndex]);
                    }
                }
                else
                {
                    //Slot vazio: mostra o cadeado, esconde o texto e o botao de copiar
                    if (textSlots[i] != null) textSlots[i].gameObject.SetActive(false);
                    if (i < lockIcons.Length && lockIcons[i] != null) lockIcons[i].SetActive(true);
                    if (i < copyButtons.Length && copyButtons[i] != null) copyButtons[i].SetActive(false);
                }
            }
            else
            {
                //Fora dos limites totais de flags
                if (textSlots[i] != null) textSlots[i].gameObject.SetActive(false);
                if (i < lockIcons.Length && lockIcons[i] != null) lockIcons[i].SetActive(false);
                if (i < copyButtons.Length && copyButtons[i] != null) copyButtons[i].SetActive(false);
            }
        }

        UpdateButtons();
    }

    //Configura o botao de copia para jogar a flag correspondente no clipboard do sistema
    private void SetupCopyButton(GameObject copyButtonObject, string flagToCopy)
    {
        Button button = copyButtonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning($"[InventoryDisplay] O objeto '{copyButtonObject.name}' não possui um componente Button.", this);
            return;
        }

        // Estes botoes pertencem ao inventario: substitui tambem callbacks do Inspector.
        // Assim a mesma flag nao e copiada duas vezes por caminhos diferentes.
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => ClipboardManager.CopyFlag(flagToCopy));
    }

    private void UpdateButtons()
    {
        int totalPages = Mathf.CeilToInt((float)TOTAL_FLAGS / textSlots.Length);

        if (btnPrevious != null) btnPrevious.interactable = (currentPage > 0);
        if (btnNext != null) btnNext.interactable = (currentPage < totalPages - 1);
    }
}