using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class InteractionIntroduction : MonoBehaviour
{

    public bool playerIsHere = false;

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.CompareTag("Player"))
        {
            playerIsHere = true;
            SceneManager.LoadScene("Introduction", LoadSceneMode.Additive);
            Debug.Log("colidiu");
        }
    }
}
