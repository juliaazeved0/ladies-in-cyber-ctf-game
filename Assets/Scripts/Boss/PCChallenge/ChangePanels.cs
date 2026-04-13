using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BashTerminal;

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

    // Fecha todos os sub-painéis do desktop sem afetar o painel pai (challengePanel).
    // Nunca use CanvasManager.OpenPanel aqui — ele fecha TODOS os painéis da cena,
    // incluindo o challengePanel, que é pai destes sub-painéis.
    private void FecharTodosOsSubPaineis()
    {
        if (panelNotes != null)         panelNotes.SetActive(false);
        if (panelSteghideError != null) panelSteghideError.SetActive(false);
        if (panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
        if (panelMetadataInfo != null)  panelMetadataInfo.SetActive(false);
        if (panelSuccessFlag != null)   panelSuccessFlag.SetActive(false);
    }

    // --- PAINEL DE NOTAS ---
    public void AbrirPanelNotes()
    {
        FecharTodosOsSubPaineis();
        if (panelNotes != null) panelNotes.SetActive(true);
    }

    public void FecharPanelNotes()
    {
        if (panelNotes != null) panelNotes.SetActive(false);
    }

    // --- PAINEL DE ERRO DO STEGHIDE ---
    public void AbrirPanelSteghideError()
    {
        FecharTodosOsSubPaineis();
        if (panelSteghideError != null) panelSteghideError.SetActive(true);
    }

    public void FecharPanelSteghideError()
    {
        if (panelSteghideError != null) panelSteghideError.SetActive(false);
    }

    // --- PAINEL DA PRAIA (SUCESSO STEGHIDE) ---
    public void AbrirPanelSteghideBeach()
    {
        FecharTodosOsSubPaineis();
        if (panelSteghideBeach != null)
        {
            TerminalBoss.challengeSolved = true;
            panelSteghideBeach.SetActive(true);
        }
    }

    public void FecharPanelSteghideBeach()
    {
        if (panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
    }

    // --- PAINEL DE METADADOS ---
    public void AbrirPanelMetadadaInfo()
    {
        FecharTodosOsSubPaineis();
        if (panelMetadataInfo != null) panelMetadataInfo.SetActive(true);
    }

    public void FecharPanelMetadadaInfo()
    {
        if (panelMetadataInfo != null) panelMetadataInfo.SetActive(false);
    }

    // --- PAINEL DA FLAG FINAL ---
    public void AbrirPanelSuccessFlag()
    {
        FecharTodosOsSubPaineis();
        if (panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(true);

            if (FlagManager.Instance != null)
            {
                string newFlag = SafeBase.ViewBase(SafeBase.flag_8);
                FlagManager.Instance.SaveFlag("BOSS", newFlag);
            }
        }
    }

    public void FecharPanelSuccessFlag()
    {
        // 1. Fecha o painel de sucesso da flag
        if (panelSuccessFlag != null) panelSuccessFlag.SetActive(false);

        // 2. Fecha todo o desafio do boss (challengePanel e seus filhos)
        if (CanvasManager.Instance != null) CanvasManager.Instance.ClosedAllPanels();

        // 3. Abre a bolsa de flags com o aviso de última chance antes dos créditos
        if (InventoryManager.Instance != null) InventoryManager.Instance.AbrirBolsaFinalizacaoBoss();
    }

    // --- LÓGICA DO BOTÃO STEGHIDE NA DESKTOP ---
    public void AoClicarNoBotaoSteghide()
    {
        if (TerminalBoss.challengeSolved == false)
        {
            AbrirPanelSteghideError();
        }
        else
        {
            AbrirPanelSteghideBeach();
        }
    }
}
