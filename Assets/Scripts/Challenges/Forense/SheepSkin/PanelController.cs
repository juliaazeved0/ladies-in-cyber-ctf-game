using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [Header("Pain�is Principais")] //T�tulo para organizar o Inspector
    //Permite que os pain�is sejam arrastados para o Inspector
    public GameObject panelInitialBackground;
    public GameObject panelNetguard;
    public TMP_InputField tentativaDominio; //Refer�ncia para o campo onde a jogadora digita o texto
    public GameObject panelWrongDomain;
    public GameObject panelRightDomain;
    public GameObject panelFlagSuccess;

    [Header("Dom�nio Correto")]
    [SerializeField] private string dominioCorreto = "http://login-fake-bank.xyz/auth-steal"; //Vari�vel de texto que guarda a resposta correta

    public void AbrirPanelNetguard() //Abre apenas o painel Netguard
    {
        tentativaDominio.text = ""; //Limpa qualquer texto digitado anteriormente
        panelNetguard.SetActive(true); //Mostra o painel
    }

    public void AbrirPanelFlagSuccess() //Abre apenas o painel de sucesso
    {
        tentativaDominio.text = "";
        panelFlagSuccess.SetActive(true);
        //adiciona flag ao inventário
        string newFlag = SafeBase.ViewBase(SafeBase.flag_4);
        FlagManager.Instance.SaveFlag(newFlag);
    }

    public void FecharPainel(GameObject painel) //Arrasta no Inspector qual painel quer fechar
    {
        if (painel != null) //Verifica se esqueceu de arrastar o objeto no Inspector
        {
            tentativaDominio.text = ""; //Limpa o input ao fechar
            painel.SetActive(false); //Desativa o painel passado por par�metro
        }
    }

    //Fun��o de verifica��o sobre o dom�nio correto
    public void EnviarDominio()
    {
        //Verifica se o que foi digitado � igual ao dom�nio correto
        if(tentativaDominio.text.Trim() == dominioCorreto) //Trim() remove espa�os acidentais antes ou depois do texto
        {
            tentativaDominio.text = "";
            panelRightDomain.SetActive(true); //Abre o painel de sucesso
        }
        else
        {
            tentativaDominio.text = "";
            panelWrongDomain.SetActive(true); //Abre o painel de erro
        }
    }

    public void BackMap() //Fun��o para retornar ao mini mapa
    {
        if(panelInitialBackground != null) //Verifica se foi arrastado no Inspector
        {
            panelInitialBackground.SetActive(false); //Fecha o fundo inicial
        }
    }
}