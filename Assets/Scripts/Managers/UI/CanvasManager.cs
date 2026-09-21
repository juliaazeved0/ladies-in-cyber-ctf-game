using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton responsavel por gerenciar os paineis de UI e o minimapa 
/// entre cenas. Ao carregar uma nova cena, funde os paineis dessa cena 
/// com a instancia persistente e se autodestroi, evitando duplicidade.
/// </summary>
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [Header("Panels/Backgrounds UI")]
    [Tooltip("Lista de todos os paineis gerenciaveis da cena atual.")]
    public List<GameObject> allPanels = new List<GameObject>();

    [Header("Minimap")]
    [Tooltip("Container do minimapa, ativado/desativado independentemente dos paineis.")]
    public GameObject miniMapContainer;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            /*Ja existe uma instancia persistente: transfere os paineis desta cena
            para ela e se autodestroi. Se os paineis desta lista forem filhos deste
            GameObject, Destroy() vai destrui-los tambem, a limpeza das referencias
            "mortas" acontecem depois.*/
            Instance.UpdatePanels(this.allPanels, this.miniMapContainer);
            Destroy(gameObject);
            return;
        }
        
        ClosedAllPanels();
        ToggleMiniMap(true);
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveDestroyedPanels();
        ToggleMiniMap(true); 
    }

    /// <summary>
    /// Substitui a lista de paineis e o minimapa gerenciados pela instancia persistente
    /// pelos elementos da cena recem-carregada, e fecha tudo em seguida.
    /// </summary>
    public void UpdatePanels(List<GameObject> newPanels, GameObject newMinimap)
    {
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

    private void RemoveDestroyedPanels()
    {
        if(allPanels == null) return;

        allPanels.RemoveAll(panel => panel == null);
    }

    public void ClosedAllPanels()
    {
        if(allPanels == null) return;

        foreach(GameObject panel in allPanels)
        {
            if(panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    public void OpenPanel(string panelName)
    {
        if(allPanels == null) return;

        ClosedAllPanels();

        foreach(GameObject panel in allPanels)
        {
            if(panel != null && panel.name == panelName)
            {
                panel.SetActive(true);
                return;
            }
        }

        Debug.LogWarning($"Nenhum painel encontrado com o nome '{panelName}'.");
    }

    public void ClosedPanel(string panelName)
    {
        if(allPanels == null) return;

        foreach (GameObject panel in allPanels)
        {
            if(panel != null && panel.name == panelName)
            {
                panel.SetActive(false);
                return;
            }
        }

        Debug.LogWarning($"Nenhum painel encontrado com o nome '{panelName}'.");
    }

    public void ToggleMiniMap(bool isActive)
    {
        if(miniMapContainer != null)
        {
            miniMapContainer.SetActive(isActive);
        }
    }
}