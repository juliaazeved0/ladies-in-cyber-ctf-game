using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PCPChallenge : MonoBehaviour
{
  public GameObject boardFolders;
  public TMP_InputField passwordInputText;
  public string rightPassword = "ILobeCapibara";
  public GameObject popUpError;
  public GameObject buttonEnter;


void Start()
{
    boardFolders.SetActive(false);
}
   public void CheckPassword()
    {

        if(passwordInputText.text == rightPassword)
        {
                boardFolders.SetActive(true);
                buttonEnter.SetActive(false);
        }else
        {
            passwordInputText.text = "";
            popUpError.SetActive(false);
            popUpError.SetActive(true);
        }
    }

    public void OnClickEnter()
    {
        CheckPassword();
        boardFolders.SetActive(true);
    }
}
