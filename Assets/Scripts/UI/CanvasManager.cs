using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    //singleton
    public static CanvasManager Instance;

    [Header("Panels/backgrounds UI")]
    public List<GameObject> allPanels;

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
}
