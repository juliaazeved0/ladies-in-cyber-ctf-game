using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneIntroduction : MonoBehaviour
{   

   public bool playerIsHere = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !playerIsHere)
        {
            SceneManager.LoadSceneAsync("Introduction", LoadSceneMode.Additive);
            playerIsHere = true;
        }
    }

}
