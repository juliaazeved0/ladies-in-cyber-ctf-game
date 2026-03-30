using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System; //Importa as bibliotecas para usar o TextMeshPro

public class ValidatePassword : MonoBehaviour
{
    [Header("Configura��es de UI")]
    public TMP_InputField passwordField; //Campo onde o jogador digita a senha do computador
    public GameObject errorMessage; //Vari�vel para arrastar o objeto de texto da "senha inv�lida"
    public string correctPassword = "MUDAR123"; //A string que define a senha correta

    [Header("Pain�is do Computador")]
    public GameObject loginPanel; //Painel inicial
    public GameObject taskbarPanel; //Painel do �rea de trabalho
    public GameObject finalPanel; //Painel final da flag
    public GameObject panelCaptureFlag;

    [Header("Intera��o e Avisos")]
    public GameObject pressEKey; //objeto de texto para a tecla E
    public MonoBehaviour interactionScript; //Script que abre o PC

    [Header("Janelas")]
    public GameObject whatsappWindow; //Painel da janela do WhatsApp

    void Start()
    {
        errorMessage.SetActive(false); //Garante que o pop-up de erro comece escondido
        if(taskbarPanel != null) taskbarPanel.SetActive(false); //Painel da �rea de trabalho comece desativado
        passwordField.ActivateInputField(); //Faz com que o cursos j� apare�a piscando dentro do Input Field, sem o jogador precisar clicar
        panelCaptureFlag.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) //Verifica se a tecla Enter principal ou do num�rico foi apertada
        {
            CheckPassword(); //Se apertou no Enter, chama a fun��o que valida a senha
        }
    }

    public void CheckPassword()
    {
        bool senhaEstaCorreta = string.Equals(passwordField.text, correctPassword, System.StringComparison.OrdinalIgnoreCase);

        if (senhaEstaCorreta) //Verifica se o texto escrito no campo � igual ao da vari�vel
        {
            errorMessage.SetActive(false); //Se for igual, esconde a mensagem de erro
            loginPanel.SetActive(false); //Desativa a tela inicial
            taskbarPanel.SetActive(true); //Ativa a tela da �rea de trabalho

            if(whatsappWindow != null) whatsappWindow.SetActive(false); //Garante que comece fechado

            if(pressEKey != null) pressEKey.SetActive(false); //Desliga o texto visual da tecla E

            if (interactionScript != null) interactionScript.enabled = false; //Desativa o script que detecta o bot�o E
        }
        else
        {
            errorMessage.SetActive(true); //Mostra a mensagem de erro
            passwordField.text = ""; //Apaga o que o jogador digitou para ele tentar novamente
            passwordField.ActivateInputField(); //Devolve o foco do teclado para o campo automaticamente
        }
    }

    public void CaptureFlag()
    {
        panelCaptureFlag.SetActive(true);
        string newFlag = SafeBase.ViewBase(SafeBase.flag_5);
        // Ajustado para incluir o nome do desafio
        FlagManager.Instance.SaveFlag("O Arquivo Vazado", newFlag);
    }

    public void ClosedJustPanelCurrent()
    {
        panelCaptureFlag.SetActive(false);
    }

    public void OpenWhatsAppWindow()
    {
        if(whatsappWindow != null)
        {
            whatsappWindow.SetActive(true); //Ativa a janela
        }
    }

    public void ExitChallenge(){
        if (finalPanel.activeSelf)
        {
            CanvasManager.Instance.ClosedPanel(finalPanel.name);
            CanvasManager.Instance.ClosedPanel(loginPanel.name);

        }
        CanvasManager.Instance.ClosedPanel(loginPanel.name);
    }

    public void OpenFinalPanel()
    {
        if(finalPanel != null)
        {
            finalPanel.SetActive(true); //Abre o painel final
        }
    }
}