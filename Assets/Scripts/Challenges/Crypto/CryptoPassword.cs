using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CryptoPassword : MonoBehaviour
{
    [Header("Settings password challenge of crypto room")]
    public string rightPassword = "t3cl4d01ntu1t1v0";

    public string idChallenge = "CryptoPassword";

    public TMP_InputField passwordInputText;
    public GameObject popUpError;
    public GameObject panelSucessChallenge;

    public PulseOutline pulsePCJ;
    public PulseOutline pulsePCP;

    public GameObject buttonEnter;
    public GameObject panelChallenge;

    public TextMeshProUGUI textFlag;

    public ObjectInteraction scriptInteractionPcJ;
    public LockObjectInteraction lockpcPolyana;

    private bool flagCaptured = false;

    void Start()
    {
        popUpError.SetActive(false);
        panelSucessChallenge.SetActive(false);
        passwordInputText.ActivateInputField();

        // se já concluiu antes → já trava o pulse
        if (ChallengeManager.Instance.IsChallengeCompleted(idChallenge))
        {
            flagCaptured = true;

            pulsePCJ.StopPulsing();
            pulsePCJ.enabled = false;
        }
    }

    public void CheckPassword()
    {
        bool correct = string.Equals(
            passwordInputText.text.Trim(),
            rightPassword,
            StringComparison.OrdinalIgnoreCase
        );

        if (correct)
        {
            panelSucessChallenge.SetActive(true);
            buttonEnter.SetActive(false);
        }
        else
        {
            passwordInputText.text = "";
            popUpError.SetActive(true);
        }
    }

    public void ExitChallenge()
    {
        if (flagCaptured)
        {
            if (scriptInteractionPcJ != null)
                scriptInteractionPcJ.enabled = false;

            if (lockpcPolyana != null)
                lockpcPolyana.isUnlocked = true;

            CanvasManager.Instance.ClosedPanel(panelChallenge.name);
        }
        else
        {
            Debug.Log("SAINDO SEM CAPTURAR A FLAG");
            CanvasManager.Instance.ClosedPanel(panelChallenge.name);
        }
    }

    public void ExitPanelSucess()
    {
        panelSucessChallenge.SetActive(false);
        passwordInputText.text = "";
        buttonEnter.SetActive(true);
        passwordInputText.ActivateInputField();
    }

    public void CaptureFlag()
    {
        if (flagCaptured) return;

        flagCaptured = true;

        textFlag.text = "Flag Capturada!";

        string newFlag = SafeBase.ViewBase(SafeBase.flag_6);
        FlagManager.Instance.SaveFlag(newFlag);

        ChallengeManager.Instance.CompleteChallenge(idChallenge);

        pulsePCJ.StopPulsing();
        pulsePCJ.enabled = false;

        pulsePCP.StartPulsing();
    }
}