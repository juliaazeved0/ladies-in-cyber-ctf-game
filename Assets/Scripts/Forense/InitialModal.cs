using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitialModal : MonoBehaviour
{
    [Header("Painéis do Computador")]
    public GameObject panelOpenInventory;
    public GameObject panelOpenTerminal;
    public GameObject panelNetwatch;
    public GameObject panelDetails;
    public GameObject panelWiresharkError;

    // =========================
    // ABRIR PAINÉIS
    // =========================

    public void OpenInventory()
    {
        panelOpenInventory.SetActive(true);
    }

    public void OpenTerminal()
    {
        panelOpenTerminal.SetActive(true);
    }

    public void OpenNetwatch()
    {
        panelNetwatch.SetActive(true);
    }

    public void OpenDetails()
    {
        panelDetails.SetActive(true);
    }

    public void OpenWiresharkError()
    {
        panelWiresharkError.SetActive(true);
    }

    // =========================
    // FECHAR PAINÉIS
    // =========================

    public void CloseInventory()
    {
        panelOpenInventory.SetActive(false);
    }

    public void CloseTerminal()
    {
        panelOpenTerminal.SetActive(false);
    }

    public void CloseNetwatch()
    {
        panelNetwatch.SetActive(false);
    }

    public void CloseDetails()
    {
        panelDetails.SetActive(false);
    }

    public void CloseWiresharkError()
    {
        panelWiresharkError.SetActive(false);
    }
}
