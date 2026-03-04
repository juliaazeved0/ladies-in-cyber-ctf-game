using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialModal : MonoBehaviour
{
    [Header("Painéis do Computador")]
    public GameObject panelInventory;
    public GameObject panelTerminal;
    public GameObject panelNetwatch;
    public GameObject panelDetails;
    public GameObject panelWiresharkError;

    //Método genérico
    private void SetPanel(GameObject panel, bool state)
    {
        if(panel != null)
        {
            panel.SetActive(state);
        }
    }

    //Abrir painéis
    public void OpenInventory() => SetPanel(panelInventory, true);
    public void OpenTerminal() => SetPanel(panelTerminal, true);
    public void OpenNetwatch() => SetPanel(panelNetwatch, true);
    public void OpenDetails() => SetPanel(panelDetails, true);
    public void OpenWiresharkError() => SetPanel(panelWiresharkError, true);

    //Fechar painéis
    public void CloseInventory() => SetPanel(panelInventory, false);
    public void CloseTerminal() => SetPanel(panelTerminal, false);
    public void CloseNetwatch() => SetPanel(panelNetwatch, false);
    public void CloseDetails() => SetPanel(panelDetails, false);
    public void CloseWiresharkError() => SetPanel(panelWiresharkError, false);

    public void VoltarMapa()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
