using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Gerencia a logica de validacao de senha, navegacao na interface do computador
/// e captura de flags dentro dos desafios de hacking.
/// </summary>
public class ValidatePassword : MonoBehaviour
{
    [Header("Configuracoes de UI")]
    public TMP_InputField passwordField;
    public GameObject errorMessage;
    public string correctPassword = "MUDAR123";

    [Header("Paineis do Computador")]
    public GameObject loginPanel;
    public GameObject taskbarPanel;
    public GameObject finalPanel;
    public GameObject panelCaptureFlag;

    [Header("Interacao e Avisos")]
    public GameObject pressEKey;
    public MonoBehaviour interactionScript;

    [Header("Janelas do Sistemas")]
    public GameObject whatsappWindow; 
    public GameObject trashWindow; 

    void Start()
    {
        //Estado inicial da interface
        errorMessage.SetActive(false);
        if(taskbarPanel != null) taskbarPanel.SetActive(false);
        passwordField.ActivateInputField(); //Foca o teclado no campo de senha automaticamente
        panelCaptureFlag.SetActive(false);
        CanvasManager.Instance.ToggleMiniMap(true);
    }

    /// <summary>
    /// Compara a senha digitada com a correta (ignora letras maiusculas/minusculas)
    /// </summary>
    public void CheckPassword()
    {
        bool senhaEstaCorreta = string.Equals(passwordField.text, correctPassword, System.StringComparison.OrdinalIgnoreCase);

        if(senhaEstaCorreta) 
        {
            errorMessage.SetActive(false);
            loginPanel.SetActive(false);
            taskbarPanel.SetActive(true);

            if(whatsappWindow != null) whatsappWindow.SetActive(false);
            if(pressEKey != null) pressEKey.SetActive(false);

            //Impede que a player abra o PC enquanto ja esta dentro dele
            if(interactionScript != null) interactionScript.enabled = false;
        }
        else
        {
            errorMessage.SetActive(true);
            passwordField.text = "";
            passwordField.ActivateInputField();
        }
    }

    /// <summary>
    /// Salva a flag conquistada no FlagManager.
    /// </summary>
    public void CaptureFlag()
    {
        panelCaptureFlag.SetActive(true);
        string newFlag = SafeBase.ViewBase(SafeBase.flag_5);
        FlagManager.Instance.SaveFlag("O Arquivo Vazado", newFlag);
    }

    public void ClosedJustPanelCurrent()
    {
        panelCaptureFlag.SetActive(false);
    }

    public void ClosedPanelTrash(){
        trashWindow.SetActive(false);
        CanvasManager.Instance.ToggleMiniMap(true);
    }

    public void OpenWhatsAppWindow()
    {
        if(whatsappWindow != null) whatsappWindow.SetActive(true);
    }

    /// <summary>
    /// Encerra o desafio e limpa a tela.
    /// </summary>
    public void ExitChallenge()
    {
        //Fecha todas as janelas ativas
        if(loginPanel != null) loginPanel.SetActive(false);
        if(taskbarPanel != null) taskbarPanel.SetActive(false);
        if(finalPanel != null) finalPanel.SetActive(false);
        if(panelCaptureFlag != null) panelCaptureFlag.SetActive(false);
        if(whatsappWindow != null) whatsappWindow.SetActive(false);

        if(CanvasManager.Instance != null)
        {
            //Tenta fechar o painel pelo nome
            CanvasManager.Instance.ClosedPanel(loginPanel.name);
            CanvasManager.Instance.ToggleMiniMap(true);
        }

        if(interactionScript != null) interactionScript.enabled = true;
    }

    public void OpenFinalPanel()
    {
        if(finalPanel != null)
        {
            finalPanel.SetActive(true);
        }
    }
}