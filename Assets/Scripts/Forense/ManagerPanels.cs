using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPanels : MonoBehaviour
{
    [Header("Painéis Principais")]
    public GameObject panelNetwatch;
    public GameObject panelInventory;
    public GameObject panelTerminal;
    public GameObject panelWiresharkError;
    public GameObject initialBackground; // Arraste o InitialBackground aqui

    [Header("Sub-Painéis")]
    public GameObject panelDetails;

    [Header("Referência ServerController")]
    public ServerController scriptServer; 

    public void AbrirWiresharkSuccess()
    {
        panelWiresharkError.SetActive(true);

        if (scriptServer != null)
        {
            scriptServer.UnlockByHacking();
        }
    }

    // --- LÓGICA DE ABRIR ---

    // Abre o Netwatch fechando os outros principais, mas mantendo a hierarquia
    public void AbrirNetwatch() { FecharPaineisPrincipais(); panelNetwatch.SetActive(true); }
    public void AbrirInventory() { FecharPaineisPrincipais(); panelInventory.SetActive(true); }
    public void AbrirTerminal() { FecharPaineisPrincipais(); panelTerminal.SetActive(true); }
    public void AbrirWireshark() { FecharPaineisPrincipais(); panelWiresharkError.SetActive(true); }

    // Abre os detalhes SEM fechar o Netwatch que está atrás
    public void AbrirDetails()
    {
        if (panelDetails != null) panelDetails.SetActive(true);
    }

    // --- LÓGICA DE FECHAR (Um de cada vez) ---

    // Chamado pelo botão "X" do Painel de Detalhes
    public void FecharApenasDetails()
    {
        if (panelDetails != null) panelDetails.SetActive(false);
    }

    // Chamado pelo botão "X" do Netwatch ou botões de saída
    public void FecharPaineisPrincipais()
    {
        if (panelNetwatch != null) panelNetwatch.SetActive(false);
        if (panelInventory != null) panelInventory.SetActive(false);
        if (panelTerminal != null) panelTerminal.SetActive(false);
        if (panelWiresharkError != null) panelWiresharkError.SetActive(false);
    }

    public void VoltarMapa()
    {
        if(initialBackground != null)
        {
            initialBackground.SetActive(false);
        }
    }
}