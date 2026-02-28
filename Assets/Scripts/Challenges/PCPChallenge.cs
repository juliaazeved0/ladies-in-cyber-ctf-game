using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PCPChallenge : MonoBehaviour
{
  public GameObject boardFolders;
  public TMP_InputField passwordInputText;
  public TMP_InputField passwordInputZip;
  private string rightPasswordZip = "CAPIVARACRIPTO";
  private string rightPassword = "ilobecapibara";
  public GameObject popUpSucess;
  public GameObject popUpError;

  public GameObject popUpError2;
  public GameObject buttonEnter;
  public Button buttonFlag;

  public GameObject popUpZip;

  public TMP_InputField textFlag;

  public Button exitChallenge;

  public GameObject canvasChallenge;

  public PulseOutline pcPoly;


void Start()
{
    boardFolders.SetActive(false);
    popUpError.SetActive(false);
    popUpError2.SetActive(false);
}
   public void CheckPassword()
    {
            if(passwordInputText.text == rightPassword)
            {
                
                boardFolders.SetActive(true);
                buttonEnter.SetActive(false);
                passwordInputText.text = "";
            }
            else
            {
                Debug.Log("else");
                passwordInputText.text = "";
                //popUpError.SetActive(false);
                popUpError.SetActive(true);
            }
            
    }


    public void CheckArchiveZip()
    {
        if(passwordInputZip.text == rightPasswordZip)
        {
                popUpZip.SetActive(false);
                popUpSucess.SetActive(true);
                passwordInputZip.text="";
        }
            else
            {
                passwordInputZip.text = "";
                popUpError2.SetActive(true);
            }
    }

    public void CaptureFlag()
    {
        textFlag.text = "Flag Capturada!";
        pcPoly.StopPulsing();

    }

    public void OnClickEnter()
    {
        CheckPassword();
       // popUpError.SetActive(true);
    }

    public void OnClickExit()
    {
        if(textFlag.text == "Flag Capturada!")
        {
            //chamar dialogo novamente passando novo node
            canvasChallenge.SetActive(false);
        }
    }
    
}
