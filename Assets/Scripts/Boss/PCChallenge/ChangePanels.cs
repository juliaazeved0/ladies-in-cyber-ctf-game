using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BashTerminal; // RESOLVE O ERRO CS0103: Permite acessar a classe TerminalBoss

public class ChangePanels : MonoBehaviour
{
    [Header("Painéis")] 
    public GameObject desktopBackground;
    public GameObject panelNotes;
    public GameObject panelSteghideError;
    public GameObject panelSteghideBeach;
    public GameObject panelMetadataInfo;
    public GameObject panelSuccessFlag;

    [Header("Referencias Externas")]
    public Button steghideButton; 

    // --- PAINEL DE NOTAS ---
    public void AbrirPanelNotes()
    {
        if(panelNotes != null) panelNotes.SetActive(true);
    }

    public void FecharPanelNotes()
    {
        if(panelNotes != null) panelNotes.SetActive(false);
    }

    // --- PAINEL DE ERRO DO STEGHIDE ---
    public void AbrirPanelSteghideError()
    {
        if(panelSteghideError != null) panelSteghideError.SetActive(true);
    }

    public void FecharPanelSteghideError()
    {
        if(panelSteghideError != null) panelSteghideError.SetActive(false);
    }

    // --- PAINEL DA PRAIA (SUCESSO STEGHIDE) ---
    public void AbrirPanelSteghideBeach()
    {
        if(panelSteghideBeach != null)
        {
            // Seta a flag como resolvida caso venha de outra fonte
            TerminalBoss.challengeSolved = true; 
            panelSteghideBeach.SetActive(true);

            // Se o painel de erro estiver aberto, ele fecha ao abrir o de sucesso
            if (panelSteghideError != null) panelSteghideError.SetActive(false);
        }
    }

    public void FecharPanelSteghideBeach()
    {
        if(panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
    }

    // --- PAINEL DE METADADOS ---
    public void AbrirPanelMetadadaInfo()
    {
        if(panelMetadataInfo != null) panelMetadataInfo.SetActive(true);
    }

    public void FecharPanelMetadadaInfo()
    {
        if (panelMetadataInfo != null) panelMetadataInfo.SetActive(false);
    }

    // --- PAINEL DA FLAG FINAL ---
    public void AbrirPanelSuccessFlag()
    {
        if(panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(true);

            // Salva a flag do BOSS usando o sistema de Base64
            if (FlagManager.Instance != null)
            {
                string newFlag = SafeBase.ViewBase(SafeBase.flag_8);
                FlagManager.Instance.SaveFlag("BOSS", newFlag);
            }
        }
    }

    public void FecharPanelSuccessFlag()
    {
        if(panelSuccessFlag != null) panelSuccessFlag.SetActive(false);
    }

    // --- LÓGICA DO BOTÃO STEGHIDE NA DESKTOP ---
    public void AoClicarNoBotaoSteghide()
    {
        // Verifica a variável static do script TerminalBoss
        if(TerminalBoss.challengeSolved == false) 
        {
            // A jogadora ainda não moveu o arquivo no terminal
            AbrirPanelSteghideError(); 
        }
        else 
        {
            // O arquivo foi movido com sucesso no terminal, libera o acesso
            AbrirPanelSteghideBeach(); 
        }
    }
}