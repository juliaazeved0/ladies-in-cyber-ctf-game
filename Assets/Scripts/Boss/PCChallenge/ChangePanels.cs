using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void AbrirPanelSteghideError()
    {
        if(panelSteghideError != null)
        {
            panelSteghideError.SetActive(true);
        }
    }

    public void FecharPanelSteghideError()
    {
        if(panelSteghideError != null)
        {
            panelSteghideError.SetActive(false);
        }
    }

    public void AbrirPanelSteghideBeach()
    {
        if(panelSteghideBeach != null)
        {
            panelSteghideBeach.SetActive(true);

            if (panelSteghideError == null) panelSteghideError.SetActive(false);
        }
    }

    public void FecharPanelSteghideBeach()
    {
        if(panelSteghideBeach != null)
        {
            panelSteghideBeach .SetActive(false);
        }
    }

    public void AbrirPanelMetadadaInfo()
    {
        if(panelMetadataInfo != null)
        {
            panelMetadataInfo.SetActive(true);
        }
    }

    public void FecharPanelMetadadaInfo()
    {
        if (panelMetadataInfo != null)
        {
            panelMetadataInfo.SetActive(false);
        }
    }

    public void AbrirPanelSuccessFlag()
    {
        if(panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(true);
        }
    }

    public void FecharPanelSuccessFlag()
    {
        if(panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(false);
        }
    }

    public void AoClicarNoBotaoSteghide()
    {
        //Se o botão ainda não foi "desbloqueado" pelo script AbrirTerminalBoss
        if(AbrirTerminalBoss.challengeSolved == false)
        {
            AbrirPanelSteghideError(); //Se não resolveu o desafio no terminal, abre o painel de erro
        }
        else
        {
            AbrirPanelSteghideBeach(); //Se já resolveu o desafio no terminal, abre o painel de metadados

            if(panelSteghideError != null) panelSteghideError.SetActive(false); //Garante que o painel de erro feche se ele estiver aberto
        }
    }
}
