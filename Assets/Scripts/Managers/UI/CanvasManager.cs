using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia os paineis de UI e o minimapa como um Singleton persistente
/// entre cenas. Cada nova cena pode registrar seu proprio conjunto de
/// paineis via UpdatePanels.
/// </summary>
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [Header("Panels/backgrounds UI")]
    [Tooltip("Lista de todos os paineis gerenciaveis da cena atual.")]
    public List<GameObject> allPanels = new List<GameObject>();

    [Header("Minimap")]
    [Tooltip("Container do minimapa, ativado/desativado independentemente dos paineis.")]
    public GameObject miniMapContainer;

    void Awake()
    {
        if(Instance == null)
        {
            //Primeira instancia: torna-se a oficial e sobrevive entre cenas
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            //Inscreve-se no evento de carregamento de cena para limpar referencias antigas a cada nova cena
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            ToggleMiniMap(true); 
        }
        else
        {
            //Repassa os paineis/minimapa configurados na cena para a instancia ja existente
            Instance.UpdatePanels(this.allPanels, this.miniMapContainer);
            Destroy(gameObject);
            return;
        }
        
        ClosedAllPanels();
        ToggleMiniMap(true);
    }

    private void OnDestroy()
    {
        //So remove a inscricao se for de fato a instancia ativa
        if(Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    //Chamado automaticamente pela Unity toda vez que uma cena eh carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Limpa paineis destruidos e garante que o minimapa reapareca
        RemoveDestroyedPanels();
        ToggleMiniMap(true); 
    }

    /// <summary>
    /// Substitui a lista de paineis e o minimapa gerenciados pela instancia persistente. Chamado
    /// pelo CanvasManager duplicado de uma nova cena, repassando suas proprias referencias.
    /// </summary>
    public void UpdatePanels(List<GameObject> newPanels, GameObject newMinimap)
    {
        //Evita NullReferenceException caso a lista recebida esteja nula
        if(newPanels == null)
        {
            Debug.LogWarning("UpdatePanels recebeu uma lista de painéis nula. Nenhuma atualização foi feita.");
            return;
        }

        allPanels.Clear();

        foreach(GameObject p in newPanels)
        {
            if(p != null) allPanels.Add(p);
        }
        
        if(newMinimap != null) miniMapContainer = newMinimap;
        
        ClosedAllPanels();
        ToggleMiniMap(true);
    }

    //Remove da lista quaisquer paineis que tenham sido destruidos
    private void RemoveDestroyedPanels()
    {
        allPanels.RemoveAll(panel => panel == null);
    }

    /// <summary>
    /// Desativa todos os paineis gerenciados, geralmente usado como
    /// reset antes de abrir um painel especifico.
    /// </summary>
    public void ClosedAllPanels()
    {
        foreach(GameObject panel in allPanels)
        {
            if(panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Fecha todos os paineis e abre apenas o painel cujo nome corresponde
    /// ao informado (busca por GameObject.name).
    /// </summary>
    public void OpenPanel(string panelName)
    {
        ClosedAllPanels();

        foreach(GameObject panel in allPanels)
        {
            if(panel != null && panel.name == panelName)
            {
                panel.SetActive(true);
                return;
            }
        }

        //Avisa caso nenhum painel com esse nome tenha sido encontrado, facilitando a identificacao de erros
        Debug.LogWarning($"Nenhum painel encontrado com o nome '{panelName}'.");
    }

    //Fecha apenas o painel cujo nome corresponde ao informado
    public void ClosedPanel(string panelName)
    {
        foreach(GameObject panel in allPanels)
        {
            if(panel != null && panel.name == panelName)
            {
                panel.SetActive(false);
                return;
            }
        }

        Debug.LogWarning($"Nenhum painel encontrado com o nome '{panelName}'.");
    }

    //Ativa ou desativa o minimapa, independentemente do estado dos demais paineis
    public void ToggleMiniMap(bool isActive)
    {
        if(miniMapContainer != null)
        {
            miniMapContainer.SetActive(isActive);
        }
    }
}