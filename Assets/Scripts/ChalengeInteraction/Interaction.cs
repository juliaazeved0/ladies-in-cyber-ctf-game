using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey;
    public GameObject challengePanel;

    [Header("Interaction")]
    public GameObject interactionNotice;
    public GameObject miniMapCamera;

    private bool playerIsHere = false;
    private bool isCompleted = false;

    void Start()
    {
        interactionNotice.SetActive(false);
        //decidir se preciso mesmo fazer duas verificações de status
        CheckChallengeStatus();
    }

    
    void Update()
    {
        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted)
        {
            interactionNotice.SetActive(false);
            miniMapCamera.SetActive(false);
            //aqui tenho q invocar o metodo q vai ser responsavel pelo deafio ou so ativar o panel do desafio
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;
            CheckChallengeStatus();
     
      
            if(!isCompleted && interactionNotice != null)
            {
                
                interactionNotice.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;

            interactionNotice.SetActive(false);
        }
    }

    public void CheckChallengeStatus()
    {
        if(!string.IsNullOrEmpty(uniqueSaveKey))
        {
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
        }
    }
}
