using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PCPChallenge : MonoBehaviour
{
    [Header("Challenge ID")]
    public string challengeID = "CryptoCapivara";

    [Header("UI")]
    public GameObject boardFolders;
    public TMP_InputField passwordInputText;
    public TMP_InputField passwordInputZip;
    public GameObject popUpSucess;
    public GameObject popUpError;
    public GameObject popUpError2;
    public GameObject popUpZip;
    public TMP_InputField textFlag;
    public GameObject canvasChallenge;

    [Header("Buttons")]
    public GameObject buttonEnter;
    public Button buttonFlag;
    public Button exitChallenge;

    [Header("Pulse")]
    public PulseOutline pcPoly;

    private string rightPasswordZip = "CAPIVARACRIPTO";
    private string rightPassword = "ilovecapibara";

    private bool flagCaptured = false;

    void Start()
    {
        boardFolders.SetActive(false);
        popUpError.SetActive(false);
        popUpError2.SetActive(false);
        popUpSucess.SetActive(false);

        // Se o desafio já foi concluído antes → já nasce resolvido
        if (ChallengeManager.Instance.IsChallengeCompleted(challengeID))
        {
            flagCaptured = true;

            pcPoly.StopPulsing();
            pcPoly.enabled = false;
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
            boardFolders.SetActive(true);
            buttonEnter.SetActive(false);
            passwordInputText.text = "";
        }
        else
        {
            passwordInputText.text = "";
            popUpError.SetActive(true);
        }
    }

    public void CheckArchiveZip()
    {
        bool correct = string.Equals(
            passwordInputZip.text.Trim(),
            rightPasswordZip,
            StringComparison.OrdinalIgnoreCase
        );

        if (correct)
        {
            popUpZip.SetActive(false);
            popUpSucess.SetActive(true);
            passwordInputZip.text = "";
        }
        else
        {
            passwordInputZip.text = "";
            popUpError2.SetActive(true);
        }
    }

    public void CaptureFlag()
    {
        if (flagCaptured) return;

        flagCaptured = true;

        textFlag.text = "Flag Capturada!";

        pcPoly.StopPulsing();
        pcPoly.enabled = false;

        string newFlag = SafeBase.ViewBase(SafeBase.flag_7);

        FlagManager.Instance.SaveFlag("Computador da Polyana", newFlag);

        ChallengeManager.Instance.CompleteChallenge(challengeID);
    }

    public void OnClickEnter()
    {
        CheckPassword();
    }

    public void OnClickExit()
    {
            CanvasManager.Instance.ClosedPanel(canvasChallenge.name);
    }
}