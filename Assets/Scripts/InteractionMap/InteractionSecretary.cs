using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class InteractionSecretary : MonoBehaviour
{

    public GameObject interactionNotice;

    public DialogueManager dialogueManager;

    private bool playerIsHere = false;
    void Start()
    {
        interactionNotice.SetActive(false);
    }

    void Update()
    {
        if (playerIsHere == true)
        {
            interactionNotice.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                interactionNotice.SetActive(false);
                dialogueManager.StartDialogue();
            }
           
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.CompareTag("Player"))
        {
            playerIsHere = true;
            interactionNotice.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collison)
    {
        if(collison.CompareTag("Player"))
        {
            playerIsHere = false;
            interactionNotice.SetActive(false);
        }
    }
}
