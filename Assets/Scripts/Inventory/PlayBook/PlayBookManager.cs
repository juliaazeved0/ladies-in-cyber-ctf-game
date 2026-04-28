using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gerencia a navegacao entre as paginas do livro no inventario.
/// Responsavel por atualizar a imagem exibida e o estado dos botoes de navegacao.
/// </summary>
public class PlaybookManager : MonoBehaviour
{
    public Sprite[] bookPages; 
    public Image basePageLocal; 

    [Header("Buttons")]
    public Button buttonNext;
    public Button buttonBack;

    private int indexCurrent = 0;

    /// <summary>
    /// Reseta o livro para a primeira pagina toda vez que o objeto for ativado.
    /// </summary>
    void OnEnable()
    {
        indexCurrent = 0;
        UpdatePage();
    }

    /// <summary>
    /// Avanca para a proxima pagina, se disponivel.
    /// </summary>
    public void NextPage()
    {
        if(indexCurrent < bookPages.Length - 1)
        {
            indexCurrent++;
            UpdatePage();
        }
    }

    /// <summary>
    /// Retorna para a pagina anterior, se disponivel.
    /// </summary>
    public void BackPage()
    {
        if(indexCurrent > 0)
        {
            indexCurrent--;
            UpdatePage();
        }
    }

    /// <summary>
    /// Atualiza a imagem da pagina e o estado interativo dos botoes.
    /// </summary>
    private void UpdatePage()
    {
        if(bookPages.Length == 0) return;

        basePageLocal.sprite = bookPages[indexCurrent];

        //Atualiza a navegabilidade dos botoes (desativa se chegar no limite)
        if(buttonBack != null) buttonBack.interactable = (indexCurrent > 0);
        if(buttonNext != null) buttonNext.interactable = (indexCurrent < bookPages.Length - 1);
    }
}