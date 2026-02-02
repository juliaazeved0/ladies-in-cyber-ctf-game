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
    public PulseOutline scriptPulseOutline;

    public void CheckPassword()
    {
        string textInput = passwordInputText.text.Trim();

        if(textInput == rightPassword)
        {
            if(scriptPulseOutline != null)
            {
                scriptPulseOutline.StopPulsing();
                panelSucessChallenge.SetActive(true);
            }
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
        
    }

}
