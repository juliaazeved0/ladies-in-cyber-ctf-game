using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CryptoPassword : MonoBehaviour
{
    [Header("Settings password challenge of crypto room")]
    public string rightPassword = "L1C{t3cl4d01ntu1t1v0}";
    
    //public string idChallenge = "Password_PC_CryptoRoom";
    public TMP_InputField passwordInputText;
    public GameObject popUpError;
    public GameObject panelSucessChallenge;
    public PulseOutline pulsePCJ;
    public PulseOutline pulsePCP;
    public GameObject buttonEnter;
    public GameObject buttonExit;
    public GameObject panelChallenge;
    public GameObject buttonExitPanelSucess;
    public NPCDialogueNode sucessNode;
    public SimpleDialogue simpleDialogue;
    public TextMeshProUGUI textFlag;
    public GameObject dialogueNPC;
    public ObjectInteraction scriptInteractionPcJ;

    public void CheckPassword()
    {
        string textInput = passwordInputText.text.Trim();

        if(textInput == rightPassword)
        {
                panelSucessChallenge.SetActive(true);
                buttonEnter.SetActive(false);
        }else
        {
            passwordInputText.text = "";
            popUpError.SetActive(false);
            popUpError.SetActive(true);
        }
    }

    void Start()
    {
        popUpError.SetActive(false);
        panelSucessChallenge.SetActive(false);
        passwordInputText.ActivateInputField();
    }


    public void ExitChallenge()
    {

       if(textFlag.text.Trim().Equals("Flag Capturada!"))
        {   
            if(scriptInteractionPcJ != null)
            {
                scriptInteractionPcJ.enabled = false;
            }
           
            CanvasManager.Instance.ClosedPanel(panelChallenge.name);
            CanvasManager.Instance.OpenPanel(dialogueNPC.name);
            simpleDialogue.StartDialogue(sucessNode);
            pulsePCJ.StopPulsing();
        }
        else{
            Debug.Log("SAINDO SEM CAPTURAR A FLAG");
            CanvasManager.Instance.ClosedPanel(panelChallenge.name);
        }
    }

    public void ExitPanelSucess()
    {
        panelSucessChallenge.SetActive(false);
        passwordInputText.text = "";
        buttonEnter.SetActive(true);

        //criar verificacao aqui para saber se a flag foi salva, se foi salva e exit selecionado, chama o panel de dialogo para continuar para o proximo desafio
        
    }

    public void CaptureFlag()
    {
       string flagCaptured = "Flag Capturada!";
       textFlag.text = flagCaptured;
        //salvar rightPssword dentro de uma lista 
        pulsePCJ.StopPulsing();
        pulsePCP.StartPulsing();
    }

}
