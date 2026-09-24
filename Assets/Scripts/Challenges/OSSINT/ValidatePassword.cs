using UnityEngine;
using TMPro;

/// <summary>
/// Controla o desafio de login de um "computador" no jogo: valida a senha
/// digitada, alterna entre os paineis do sistema simulado (login, taskbar,
/// janelas) e gerencia a captura da flag associada a esse desafio.
/// </summary>
public class ValidatePassword : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Campo de input onde a jogadora digita a senha.")]
    public TMP_InputField passwordField;

    [Tooltip("Mensagem exibida quando a senha digitada esta incorreta.")]
    public GameObject errorMessage;

    [Tooltip("Senha correta para liberar o acesso.")]
    public string correctPassword = "MUDAR123";

    [Header("Computer Panels")]
    [Tooltip("Painel de tela de login.")]
    public GameObject loginPanel;

    [Tooltip("Barra de tarefas exibida apos o login bem-sucedido.")]
    public GameObject taskbarPanel;

    [Tooltip("Painel final exibido ao concluir o desafio.")]
    public GameObject finalPanel;

    [Tooltip("Painel exibido ao capturar a flag do desafio.")]
    public GameObject panelCaptureFlag;

    [Header("Interactions and Notices")]
    [Tooltip("Aviso visual para pressionar E e interagir.")]
    public GameObject pressEKey;

    [Tooltip("Script de interacao a ser desabilitado apos o login (evita reabrir o desafio).")]
    public MonoBehaviour interactionScript;

    [Header("Windows")]
    [Tooltip("Janela do WhatsApp simulado dentro do computador.")]
    public GameObject whatsappWindow;

    [Tooltip("Janela da lixeira simulada dentro do computador.")]
    public GameObject trashWindow; 

    void Start()
    {
        if(errorMessage != null) errorMessage.SetActive(false);
        if(taskbarPanel != null) taskbarPanel.SetActive(false);
        if(passwordField != null) passwordField.ActivateInputField();
        if(panelCaptureFlag != null) panelCaptureFlag.SetActive(false);

        if(CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        else
        {
            Debug.LogError("[ValidatePassword] CanvasManager.Instance não está disponível.");
        }
    }

    /// <summary>
    /// Verifica se a senha digitada confere com a senha correta (ignorando
    /// maiusculas/minusculas). Se correta, libera o acesso ao sistema;
    /// caso contrario, exibe a mensagem de erro e limpa o campo.
    /// </summary>
    public void CheckPassword()
    {
        if(passwordField == null)
        {
            Debug.LogError("[ValidatePassword] passwordField não está atribuído no Inspector.");
            return;
        }

        bool passwordCorrect = string.Equals(passwordField.text, correctPassword, System.StringComparison.OrdinalIgnoreCase);

        if(passwordCorrect)
        {
            if(errorMessage != null) errorMessage.SetActive(false);
            if(loginPanel != null) loginPanel.SetActive(false);
            if(taskbarPanel != null) taskbarPanel.SetActive(true);

            if(whatsappWindow != null) whatsappWindow.SetActive(false);
            if(pressEKey != null) pressEKey.SetActive(false);

            if(interactionScript != null) interactionScript.enabled = false;
        }
        else
        {
            if(errorMessage != null) errorMessage.SetActive(true);

            passwordField.text = "";
            passwordField.ActivateInputField();
        }
    }

    //Mostra o painel de captura e registra a flag deste desafio no FlagManager
    public void CaptureFlag()
    {
        if(panelCaptureFlag != null) panelCaptureFlag.SetActive(true);

        string newFlag = SafeBase.ViewBase(SafeBase.flag_5);

        if(FlagManager.Instance != null)
        {
            FlagManager.Instance.SaveFlag("O Arquivo Vazado", newFlag);
        }
        else
        {
            Debug.LogError("[ValidatePassword] FlagManager.Instance não está disponível.");
        }
    }

    //Fecha apenas o painel de captura da flag
    public void ClosedJustPanelCurrent()
    {
        if(panelCaptureFlag != null) panelCaptureFlag.SetActive(false);
    }

    //Fecha a janela da lixeira e reexibe o minimapa
    public void ClosedPanelTrash()
    {
        if(trashWindow != null) trashWindow.SetActive(false);

        if(CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        else
        {
            Debug.LogError("[ValidatePassword] CanvasManager.Instance não está disponível.");
        }
    }

    //Abre a janela do WhatsApp simulado
    public void OpenWhatsAppWindow()
    {
        if(whatsappWindow != null) whatsappWindow.SetActive(true);
    }

    /// <summary>
    /// Encerra o desafio, fechando todos os paineis e janelas relacionados
    /// e reativando o script de interacao original.
    /// </summary>
    public void ExitChallenge()
    {
        if(loginPanel != null) loginPanel.SetActive(false);
        if(taskbarPanel != null) taskbarPanel.SetActive(false);
        if(finalPanel != null) finalPanel.SetActive(false);
        if(panelCaptureFlag != null) panelCaptureFlag.SetActive(false);
        if(whatsappWindow != null) whatsappWindow.SetActive(false);

        if(CanvasManager.Instance != null && loginPanel != null)
        {
            CanvasManager.Instance.ClosedPanel(loginPanel.name);
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        else if(CanvasManager.Instance == null)
        {
            Debug.LogError("[ValidatePassword] CanvasManager.Instance não está disponível.");
        }

        if (interactionScript != null) interactionScript.enabled = true;
    }

    //Exibe o painel final do desafio
    public void OpenFinalPanel()
    {
        if(finalPanel != null)
        {
            finalPanel.SetActive(true);
        }
    }
}