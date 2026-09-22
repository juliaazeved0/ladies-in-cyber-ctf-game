using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Desafio de senha da sala de criptografia: valida a senha digitada,
/// gerencia o painel de sucesso, o destaque visual (pulso) dos PCs
/// envolvidos e a captura/persistencia da flag correspondente.
/// </summary>
public class CryptoPassword : MonoBehaviour
{
    [Header("Settings Password Challenge")]
    [Tooltip("Senha correta esperada para este desafio.")]
    public string rightPassword = "t3cl4d01ntu1t1v0";

    [Tooltip("Identificador unico deste desafio, usado pelo ChallengeManager.")]
    public string idChallenge = "CryptoPassword";

    [Tooltip("Campo de input onde a jogadora digita a senha.")]
    public TMP_InputField passwordInputText;

    [Tooltip("Aviso exibido quando a senha digitada esta incorreta.")]
    public GameObject popUpError;

    [Tooltip("Painel exibido ao acertar a senha.")]
    public GameObject panelSucessChallenge;

    [Tooltip("Efeito de pulso do PC da 'Julia' (desativado apos a flag ser capturada).")]
    public PulseOutline pulsePCJ;

    [Tooltip("Efeito de pulso do PC da Polyana.")]
    public PulseOutline pulsePCP;

    [Tooltip("Botao para confirmar a senha digitada.")]
    public GameObject buttonEnter;

    [Tooltip("Painel principal deste desafio.")]
    public GameObject panelChallenge;

    [Tooltip("Texto que exibe a flag capturada.")]
    public TextMeshProUGUI textFlag;

    [Tooltip("Script de interacao do PC da Julia, desabilitado apos a flag ser capturada.")]
    public ObjectInteraction scriptInteractionPcJ;

    [Tooltip("Script de bloqueio do PC da Polyana, desbloqueado apos a flag ser capturada.")]
    public LockObjectInteraction lockpcPolyana;

    [Tooltip("No de dialogo exibido ao sair do desafio ja com a flag capturada.")]
    public NPCDialogueNode sucessNode;

    [Tooltip("Componente de dialogo usado para exibir o no de sucesso.")]
    public SimpleDialogue simpleDialogue;

    //Indica se a flag deste desafio ja foi capturada
    private bool flagCaptured = false;

    void Start()
    {
        if(popUpError != null) popUpError.SetActive(false);
        if(panelSucessChallenge != null) panelSucessChallenge.SetActive(false);
        if(passwordInputText != null) passwordInputText.ActivateInputField();

        if(ChallengeManager.Instance != null)
        {
            if(ChallengeManager.Instance.IsChallengeCompleted(idChallenge))
            {
                flagCaptured = true;

                if(pulsePCJ != null)
                {
                    pulsePCJ.StopPulsing();
                    pulsePCJ.enabled = false;
                }
            }
        }
        else
        {
            Debug.LogError("[CryptoPassword] ChallengeManager.Instance não está disponível.");
        }
    }

    /// <summary>
    /// Verifica se a senha digitada confere com a senha correta (ignorando
    /// maiusculas/minusculas e espacos nas pontas).
    /// </summary>
    public void CheckPassword()
    {
        if(passwordInputText == null)
        {
            Debug.LogError("[CryptoPassword] passwordInputText não está atribuído no Inspector.");
            return;
        }

        bool correct = string.Equals(
            passwordInputText.text.Trim(),
            rightPassword,
            StringComparison.OrdinalIgnoreCase
        );

        if(correct)
        {
            if(panelSucessChallenge != null) panelSucessChallenge.SetActive(true);
            if(buttonEnter != null) buttonEnter.SetActive(false);
        }
        else
        {
            passwordInputText.text = "";

            if(popUpError != null) popUpError.SetActive(true);
        }
    }

    /// <summary>
    /// Encerra o desafio: fecha o painel, reexibe o minimapa e, se a flag ja
    /// tiver sido capturada, desbloqueia o PC da Polyana e dispara o dialogo
    /// de sucesso (caso nenhum outro dialogo esteja ativo).
    /// </summary>
    public void ExitChallenge()
    {
        if(CanvasManager.Instance != null && panelChallenge != null)
        {
            CanvasManager.Instance.ClosedPanel(panelChallenge.name);
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        else
        {
            Debug.LogError("[CryptoPassword] CanvasManager.Instance ou panelChallenge não estão disponíveis.");
        }

        if(flagCaptured)
        {
            if(scriptInteractionPcJ != null)
                scriptInteractionPcJ.enabled = false;

            if(lockpcPolyana != null)
                lockpcPolyana.isUnlocked = true;

            if(!SimpleDialogue.isSimpleDialogueActive)
            {
                if(simpleDialogue != null)
                {
                    simpleDialogue.StartDialogue(sucessNode);
                }
                else
                {
                    Debug.LogError("[CryptoPassword] simpleDialogue não está atribuído no Inspector.");
                }
            }
        }
        else
        {
            Debug.Log("SAINDO SEM CAPTURAR A FLAG!");
        }
    }

    //Fecha o painel de sucesso e reseta o campo de senha para uma nova tentativa
    public void ExitPanelSucess()
    {
        if(panelSucessChallenge != null) panelSucessChallenge.SetActive(false);

        if(passwordInputText != null)
        {
            passwordInputText.text = "";
            passwordInputText.ActivateInputField();
        }

        if(buttonEnter != null) buttonEnter.SetActive(true);

    }

    /// <summary>
    /// Registra a flag deste desafio (se ainda nao capturada), marca o
    /// desafio como concluido e atualiza os efeitos visuais dos PCs.
    /// </summary>
    public void CaptureFlag()
    {
        if(flagCaptured) return;

        flagCaptured = true;

        if(textFlag != null) textFlag.text = "Flag Capturada!";

        string newFlag = SafeBase.ViewBase(SafeBase.flag_6);

        if(FlagManager.Instance != null)
        {
            FlagManager.Instance.SaveFlag("Senha Anotada", newFlag);
        }
        else
        {
            Debug.LogError("[CryptoPassword] FlagManager.Instance não está disponível.");
        }

        if(ChallengeManager.Instance != null)
        {
            ChallengeManager.Instance.CompleteChallenge(idChallenge);
        }
        else
        {
            Debug.LogError("[CryptoPassword] ChallengeManager.Instance não está disponível.");
        }

        if(pulsePCJ != null)
        {
            pulsePCJ.StopPulsing();
            pulsePCJ.enabled = false;
        }

        if(pulsePCP != null)
        {
            pulsePCP.StartPulsing();
        }
    }
}