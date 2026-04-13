using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [Header("Panels/backgrounds UI")]
    public List<GameObject> allPanels = new List<GameObject>();

    [Header("Minimap")]
    public GameObject miniMapContainer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // CORREÇÃO: Inicia o minimapa ATIVADO (true)
            ToggleMiniMap(true); 
        }
        else
        {
            // Transfere painéis para a instância persistente
            Instance.UpdatePanels(this.allPanels, this.miniMapContainer);
            Destroy(gameObject);
            return;
        }
        
        ClosedAllPanels();
        
        // CORREÇÃO: Garante que após fechar os painéis, o minimapa continue ATIVADO
        ToggleMiniMap(true);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveDestroyedPanels();
        // Opcional: Garante que o minimapa ligue ao carregar qualquer cena nova
        ToggleMiniMap(true); 
    }

    public void UpdatePanels(List<GameObject> newPanels, GameObject newMinimap)
    {
        allPanels.Clear();
        foreach (GameObject p in newPanels)
        {
            if (p != null) allPanels.Add(p);
        }
        
        if (newMinimap != null) miniMapContainer = newMinimap;
        
        ClosedAllPanels();
        
        // CORREÇÃO: Ativa o minimapa quando as referências são atualizadas (ex: entrada na sala do Boss)
        ToggleMiniMap(true);
    }

    private void RemoveDestroyedPanels()
    {
        allPanels.RemoveAll(panel => panel == null);
    }

    public void ClosedAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    public void OpenPanel(string panelName)
    {
        ClosedAllPanels();

        foreach (GameObject panel in allPanels)
        {
            if (panel != null && panel.name == panelName)
            {
                panel.SetActive(true);
                return;
            }
        }
    }

    public void ClosedPanel(string panelName)
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null && panel.name == panelName)
            {
                panel.SetActive(false);
                return;
            }
        }
    }

    public void ToggleMiniMap(bool isActive)
    {
        if (miniMapContainer != null)
        {
            miniMapContainer.SetActive(isActive);
        }
    }
}