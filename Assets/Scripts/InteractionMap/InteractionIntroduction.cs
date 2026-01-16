using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class InteractionIntroduction : MonoBehaviour
{

    public bool playerIsHere = false;
    private const string INTRO_KEY = "introductionComplete";


    private void OnTriggerEnter2D(Collider2D collison)
    {
        int playerDone = PlayerPrefs.GetInt(INTRO_KEY, 0);

        if(collison.CompareTag("Player") && playerDone == 0)
        {
            playerIsHere = true;
            PlayerPrefs.SetInt(INTRO_KEY, 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Introduction", LoadSceneMode.Additive);
        }
    }
}
