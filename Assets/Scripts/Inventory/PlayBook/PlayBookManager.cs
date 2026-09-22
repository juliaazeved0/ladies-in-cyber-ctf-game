using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla a navegacao entre paginas do "playbook" (livro de anotacoes/desafios),
/// trocando a sprite exibida e habilitando/desabilitando os botoes de navegacao
/// conforme a pagina atual.
/// </summary>
public class PlaybookManager : MonoBehaviour
{
    [Header("Pages")]
    [Tooltip("Array com as sprites de cada pagina do playbook, na ordem de exibicao.")]
    public Sprite[] bookPages;

    [Tooltip("Componente Image usado como base para exibir a pagina atual.")]
    public Image basePageLocal; 

    [Header("Buttons")]
    [Tooltip("Botao para avancar para a proxima pagina.")]
    public Button buttonNext;

    [Tooltip("Botao para voltar para a pagina anterior.")]
    public Button buttonBack;

    //Indice da pagina atualmente exibida
    private int indexCurrent = 0;

    void OnEnable()
    {
        //Sempre que o playbook eh reaberto, reinicia na primeira pagina
        indexCurrent = 0;
        UpdatePage();
    }

    //AVanca para a proxima pagina, se houver
    public void NextPage()
    {
        if(indexCurrent < bookPages.Length - 1)
        {
            indexCurrent++;
            UpdatePage();
        }
    }

    //Volta para a pagina anterior, se houver
    public void BackPage()
    {
        if(indexCurrent > 0)
        {
            indexCurrent--;
            UpdatePage();
        }
    }

    /// <summary>
    /// Atualiza a sprite exibida para a pagina atual e o estado
    /// (interactable) dos botoes de navegacao.
    /// </summary>
    private void UpdatePage()
    {
        //Evita erro caso o array de paginas esteja vazio
        if(bookPages.Length == 0) return;

        //Evita erro caso a referencia da Image nao esteja atribuida no Inspector
        if(basePageLocal == null)
        {
            Debug.LogError("[PlaybookManager] basePageLocal não está atribuído no Inspector.");
            return;
        }

        basePageLocal.sprite = bookPages[indexCurrent];

        //Desabilita "Voltar" na primeira pagina e "Proximo" na ultima
        if(buttonBack != null) buttonBack.interactable = (indexCurrent > 0);
        if(buttonNext != null) buttonNext.interactable = (indexCurrent < bookPages.Length - 1);
    }
}