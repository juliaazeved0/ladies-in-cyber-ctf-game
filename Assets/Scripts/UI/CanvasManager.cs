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
        }
        else
        {
            // Transfere os novos painéis da cena para a instância persistente
            Instance.UpdatePanels(this.allPanels, this.miniMapContainer);
            Destroy(gameObject);
            return;
        }
        ClosedAllPanels();
    }

    private void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Limpa referências de objetos que foram destruídos na troca de cena
        allPanels.RemoveAll(panel => panel == null);
    }

    public void UpdatePanels(List<GameObject> newPanels, GameObject newMinimap)
    {
        allPanels.Clear();
        foreach (GameObject p in newPanels)
        {
            if (p != null) allPanels.Add(p);
        }
        if (newMinimap != null) miniMapContainer = newMinimap;
    }

    public void ClosedAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }
    }

    // closeOthers = true é o padrão, mantendo compatibilidade com o resto do jogo
    public void OpenPanel(string panelName, bool closeOthers = true)
    {
        if (closeOthers)
        {
            ClosedAllPanels();
        }

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
        if (miniMapContainer != null) miniMapContainer.SetActive(isActive);
    }
}