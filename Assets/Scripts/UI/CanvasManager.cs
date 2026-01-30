using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    //singleton
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
        }
        else
        {
            Destroy(gameObject);
        }
         ClosedAllPanels();
    }
 
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
