using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    //Singleton
    public static CanvasManager Instance;

    [Header("Panels/backgrounds UI")]
    public List<GameObject> allPanels;

    [Header("Minimap")]
    public GameObject miniMapContainer;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ClosedAllPanels();
    }

    //void OnEnable()
    //{
    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnDisable()
    //{
    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    //{
    //    allPanels.Clear();

    //    foreach (var obj in GameObject.FindGameObjectsWithTag("UIPanel"))
    //    {
    //        allPanels.Add(obj);
    //    }

    //    ClosedAllPanels();
    //}

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

    public void OpenPanel(string panelName)
    {
        ClosedAllPanels();

        foreach(GameObject panel in allPanels)
        {
            if(panel.name == panelName)
            {
                panel.SetActive(true);
                return;
            }
        }
    }

    public void ClosedPanel(string panelName)
    {
        ClosedAllPanels();

        foreach(GameObject panel in allPanels)
        {
            if(panel.name == panelName)
            {
                panel.SetActive(false);
                return;
            }
        }
     }

     public void ToggleMiniMap(bool isActive)
     {
        if(miniMapContainer != null)
        {
            miniMapContainer.SetActive(isActive);
        }
     }
}