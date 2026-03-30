using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePanels : MonoBehaviour
{
    [Header("Pain�is")] //Gerenciador de paineis no Inspector
    public GameObject desktopBackground;
    public GameObject panelNotes;
    public GameObject panelSteghideError;
    public GameObject panelSteghideBeach;
    public GameObject panelMetadataInfo;
    public GameObject panelSuccessFlag;

    [Header("Referencias Externas")]
    public Button steghideButton; //Apenas para setar o bot�o principal do desafio

    //Fun��es que ativam ou desativam paineis
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

            // Salva a flag do BOSS
            string newFlag = SafeBase.ViewBase(SafeBase.flag_8);
            FlagManager.Instance.SaveFlag("BOSS", newFlag);
        }
    }

    public void FecharPanelSuccessFlag()
    {
        if(panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(false);
        }
    }

    //Executa para salvar a flag
    //public void OnClickButtonFlag()
    //{
    //    // 1. Verifica o painel (preven��o contra esquecimento no Inspector)
    //    if (panelSuccessFlag != null)
    //    {
    //        panelSuccessFlag.SetActive(true);
    //    }

    //    // 2. Verifica se o FlagManager existe antes de tentar salvar
    //    if (FlagManager.Instance != null)
    //    {
    //        string newFlag = SafeBase.ViewBase(SafeBase.flag_8);
    //        FlagManager.Instance.SaveFlag(newFlag);
    //    }
    //    else
    //    {
    //        // Isso vai te avisar no Console se o FlagManager sumiu da cena
    //        Debug.LogError("ERRO: FlagManager n�o encontrado na cena! Certifique-se de que o objeto do FlagManager existe.");
    //    }
    //}

    //Esse m�todo foi vinculado no bot�o do Steghide da �rea de trabalho
    public void AoClicarNoBotaoSteghide()
    {
        if(AbrirTerminalBoss.challengeSolved == false) //A jogadora ainda n�o terminou o desafio no terminal
        {
            AbrirPanelSteghideError(); //Abre o painel de erro
        }
        else //Se o arquivo "boss_resolvido.txt" foi detectado anteriormente
        {
            AbrirPanelSteghideBeach(); //Se j� resolveu o desafio no terminal, abre o painel de metadados

            if(panelSteghideError != null) panelSteghideError.SetActive(false); //Garante que o painel de erro feche se ele estiver aberto
        }
    }
}