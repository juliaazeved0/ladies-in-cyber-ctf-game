using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPanels : MonoBehaviour
{
    [Header("Pain�is Principais")] //T�tulo no Inspector para organizar e permite que arraste todos os pain�is
    public GameObject initialBackground;
    public GameObject panelNetwatch;
    public GameObject panelInventory;
    public GameObject panelTerminal;
    public GameObject panelWiresharkError;
    public GameObject panelWiresharkSuccess;
    public GameObject panelFlag;
    public GameObject panelFlagSuccess;

    [Header("Sub-Pain�is")] //Sub-pain�l de detalhes dentro do Netwatch
    public GameObject panelDetails;

    [Header("Refer�ncia ServerController")]
    public ServerController scriptServer; //Refer�ncia ao script que controla o servidor, permite que o computador desbloqueie o servidor

    [Header("Estado do Servidor")]
    public bool caboConectado = false; //Controle para certificar se o cabo foi conectado no servidor

    //Fun��es para abrir os pain�is
    public void AbrirNetwatch() { FecharPaineisPrincipais(); panelNetwatch.SetActive(true); } //Abre o Netwatch fechando os outros principais, mas mantendo a hierarquia
    public void AbrirInventory() { FecharPaineisPrincipais(); panelInventory.SetActive(true); }
    public void AbrirTerminal() { FecharPaineisPrincipais(); panelTerminal.SetActive(true); }
    public void AbrirWireshark() { FecharPaineisPrincipais(); panelWiresharkError.SetActive(true); }

    public void AbrirWiresharkSuccess()
    {
        FecharPaineisPrincipais(); //Antes de abrir algo novo, fecha todos os pain�is principais, evitando sobreposi��o

        if (!caboConectado) //Caso o cabo n�o esteja conectado
        {
            panelWiresharkError.SetActive(true); //Mostra o painel de erro
            if(scriptServer != null) scriptServer.UnlockByHacking(); //Computador desbloqueia o servidor
        }
        else //Caso o cabo j� esteja conectado
        {
            panelWiresharkSuccess.SetActive(true); //Mostra o painel de sucesso
        }
    }

    public void AbrirFlag() //Fun��o chamada quando a jogadora quer ver a flag do desafio
    {
        if(panelWiresharkSuccess != null) panelWiresharkSuccess.SetActive(false); //Fecha o painel anterior

        if(panelFlag != null) panelFlag.SetActive(true); //Abre o painel da flag
    }

    public void AbrirFlagSuccess() //Fun��o para mostrar o painel da flag capturada
    {
        if (panelFlag != null) panelFlag.SetActive(false); //Fecha o painel da flag

        if(panelFlagSuccess != null) panelFlagSuccess.SetActive(true); //Abre o painel da flag capturada
    
        // salvando a flag no inventário 
        string newFlag = SafeBase.ViewBase(SafeBase.flag_3);
        FlagManager.Instance.SaveFlag(newFlag);
    }

    public void AbrirDetails() //Abre o painel de detalhes sem fechar o Netwatch que est� atr�s
    {
        if (panelDetails != null) panelDetails.SetActive(true);
    }

    public void FecharApenasDetails() //Fecha apenas o painel de detalhes
    {
        if (panelDetails != null) panelDetails.SetActive(false); //Painel Netwatch continua aberto
    }

    //Fun��o para limpar a interface, ela fecha todos os pain�is principais
    public void FecharPaineisPrincipais()
    {
        if (panelNetwatch != null) panelNetwatch.SetActive(false);
        if (panelInventory != null) panelInventory.SetActive(false);
        if (panelTerminal != null) panelTerminal.SetActive(false);
        if (panelWiresharkError != null) panelWiresharkError.SetActive(false);
        if(panelWiresharkSuccess != null) panelWiresharkSuccess.SetActive(false);
        if(panelFlag != null) panelFlag.SetActive(false);
        if(panelFlagSuccess != null) panelFlagSuccess.SetActive(false);
    }

    public void VoltarMapa() //Fun��o chamada quando a jogadora sai do computador
    {
        if(initialBackground != null)
        {
            initialBackground.SetActive(false); //Fecha toda a interface do PC e a jogadora volta para o mapa
        }
    }
}