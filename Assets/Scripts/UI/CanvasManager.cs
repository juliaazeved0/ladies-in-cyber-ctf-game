using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Adicionado para detectar mudança de cena

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
            // Inscreve no evento de carregamento de cena para limpar painéis mortos
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Se já existe um, mas este novo tem painéis (é o prefab da cena do Boss)
            // nós passamos os painéis dele para a instância principal antes de destruí-lo
            Instance.UpdatePanels(this.allPanels, this.miniMapContainer);
            Destroy(gameObject);
            return;
        }
        
        ClosedAllPanels();
    }

    private void OnDestroy()
    {
        // Limpeza de evento (boa prática)
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // Chamado sempre que uma nova cena carrega
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveDestroyedPanels();
    }

    // Atualiza as referências quando um novo prefab de CanvasManager aparece em outra cena
    public void UpdatePanels(List<GameObject> newPanels, GameObject newMinimap)
    {
        allPanels.Clear();
        foreach (GameObject p in newPanels)
        {
            if (p != null) allPanels.Add(p);
        }
        
        if (newMinimap != null) miniMapContainer = newMinimap;
        
        ClosedAllPanels();
    }

    private void RemoveDestroyedPanels()
    {
        allPanels.RemoveAll(panel => panel == null);
    }

    public void ClosedAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            // Verificação de nulidade adicionada aqui para evitar o MissingReferenceException
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
            // Verificação de segurança: se o objeto foi destruído, ignora
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