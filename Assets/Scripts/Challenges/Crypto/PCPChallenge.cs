using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// Desafio do "Computador da Polyana": valida senha de acesso e senha do
/// arquivo zip, gerencia os pop-ups de erro/sucesso e a captura da flag
/// correspondente.
/// </summary>
public class PCPChallenge : MonoBehaviour
{
    [Header("Challenge ID")]
    [Tooltip("Identificador unico deste desafio, usado pelo ChallengeManager.")]
    public string challengeID = "CryptoCapivara";

    [Header("UI")]
    [Tooltip("Painel com as pastas exibido apos a senha de acesso correta.")]
    public GameObject boardFolders;

    [Tooltip("Campo de input da senha de acesso ao computador.")]
    public TMP_InputField passwordInputText;

    [Tooltip("Campo de input da senha do arquivo zip.")]
    public TMP_InputField passwordInputZip;

    [Tooltip("Pop-up exibido ao acertar a senha do zip.")]
    public GameObject popUpSucess;

    [Tooltip("Pop-up de erro da senha de acesso.")]
    public GameObject popUpError;

    [Tooltip("Pop-up de erro da senha do zip.")]
    public GameObject popUpError2;

    [Tooltip("Pop-up de senha do arquivo zip.")]
    public GameObject popUpZip;

    [Tooltip("Campo de texto que exibe a flag capturada.")]
    public TMP_InputField textFlag;

    [Tooltip("Canvas/painel principal deste desafio.")]
    public GameObject canvasChallenge;

    [Header("Buttons")]
    [Tooltip("Botao para confirmar a senha de acesso.")]
    public GameObject buttonEnter;

    [Tooltip("Botao para capturar a flag.")]
    public Button buttonFlag;

    [Tooltip("Botao para sair do desafio.")]
    public Button exitChallenge;

    [Header("Pulse")]
    [Tooltip("Efeito de pulso do PC da Polyana, desativado apos a flag ser capturada.")]
    public PulseOutline pcPoly;

    private string rightPasswordZip = "CAPIVARACRIPTO";
    private string rightPassword = "ilovecapibara";

    //Indica se a flag deste desafio ja foi capturada
    private bool flagCaptured = false;

    void Start()
    {
        if(boardFolders != null) boardFolders.SetActive(false);
        if(popUpError != null) popUpError.SetActive(false);
        if(popUpError2 != null) popUpError2.SetActive(false);
        if(popUpSucess != null) popUpSucess.SetActive(false);

        //Se o desafio ja foi concluido antes → ja nasce resolvido
        if(ChallengeManager.Instance != null)
        {
            if(ChallengeManager.Instance.IsChallengeCompleted(challengeID))
            {
                flagCaptured = true;

                if(pcPoly != null)
                {
                    pcPoly.StopPulsing();
                    pcPoly.enabled = false;
                }
            }
        }
        else
        {
            Debug.LogError("[PCPChallenge] ChallengeManager.Instance não está disponível.");
        }
    }

    //Verifica a senha de acesso ao computador
    public void CheckPassword()
    {
        if(passwordInputText == null)
        {
            Debug.LogError("[PCPChallenge] passwordInputText não está atribuído no Inspector.");
            return;
        }

        bool correct = string.Equals(
            passwordInputText.text.Trim(),
            rightPassword,
            StringComparison.OrdinalIgnoreCase
        );

        if(correct)
        {
            if(boardFolders != null) boardFolders.SetActive(true);
            if(buttonEnter != null) buttonEnter.SetActive(false);

            passwordInputText.text = "";
        }
        else
        {
            passwordInputText.text = "";

            if(popUpError != null) popUpError.SetActive(true);
        }
    }

    //Verifica a senha do arquivo zip
    public void CheckArchiveZip()
    {
        if(passwordInputZip == null)
        {
            Debug.LogError("[PCPChallenge] passwordInputZip não está atribuído no Inspector.");
            return;
        }

        bool correct = string.Equals(
            passwordInputZip.text.Trim(),
            rightPasswordZip,
            StringComparison.OrdinalIgnoreCase
        );

        if(correct)
        {
            if(popUpZip != null) popUpZip.SetActive(false);
            if(popUpSucess != null) popUpSucess.SetActive(true);

            passwordInputZip.text = "";
        }
        else
        {
            passwordInputZip.text = "";

            if(popUpError2 != null) popUpError2.SetActive(true);
        }
    }

    /// <summary>
    /// Registra a flag deste desafio (se ainda nao capturada), atualiza a UI,
    /// desativa o pulso do PC e marca o desafio como concluido.
    /// </summary>
    public void CaptureFlag()
    {
        if(flagCaptured) return;

        flagCaptured = true;

        if(textFlag != null) textFlag.text = "Flag Capturada!";

        if(pcPoly != null)
        {
            pcPoly.StopPulsing();
            pcPoly.enabled = false;
        }

        string newFlag = SafeBase.ViewBase(SafeBase.flag_7);

        if(FlagManager.Instance != null)
            FlagManager.Instance.SaveFlag("Computador da Polyana", newFlag);
        else
            Debug.LogError("[PCPChallenge] FlagManager.Instance não está disponível.");

        if(ChallengeManager.Instance != null)
            ChallengeManager.Instance.CompleteChallenge(challengeID);
        else
            Debug.LogError("[PCPChallenge] ChallengeManager.Instance não está disponível.");
    }

    //Atalho para o botao de confirmar a senha de acesso
    public void OnClickEnter()
    {
        CheckPassword();
    }

    //Fecha o painel do desafio e reexibe o minimapa
    public void OnClickExit()
    {
        if(CanvasManager.Instance != null && canvasChallenge != null)
        {
            CanvasManager.Instance.ClosedPanel(canvasChallenge.name);
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        else
        {
            Debug.LogError("[PCPChallenge] CanvasManager.Instance ou canvasChallenge não estão disponíveis.");
        }
    }
}