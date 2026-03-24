using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePanels : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject desktopBackground;
    public GameObject panelNotes;
    public GameObject panelSteghide;

    public void AbrirPanelNotes()
    {
        if(panelNotes != null)
        {
            panelNotes.SetActive(true);
        }
    }

    public void FecharPanelNotes()
    {
        if(panelNotes != null)
        {
            panelNotes.SetActive(false);
        }
    }

    public void AbrirPanelSteghide()
    {
        if(panelSteghide != null)
        {
            panelSteghide.SetActive(true);
        }
    }

    public void FecharPanelSteghide()
    {
        if(panelSteghide != null)
        {
            panelSteghide.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
