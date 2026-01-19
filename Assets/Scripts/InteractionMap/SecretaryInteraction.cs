using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SecretaryInteraction : MonoBehaviour
{
    private bool playerIsHere = false;
    public Image interactionNotice;
    public DialogueManager dialogueManager;


    void Start()
    {
        if(interactionNotice != null)
        interactionNotice.gameObject.SetActive(false);
        
    }

    void Update()
    {
        int playerDone = PlayerPrefs.GetInt(DialogueManager.INICIAL_KEY, 0);

        if (playerIsHere && Input.GetKeyDown(KeyCode.E) && playerDone == 0)
        {
           
            if (interactionNotice != null)
                interactionNotice.gameObject.SetActive(false);

    
            dialogueManager.StartDialogue();
        }
    }
    private void OnTriggerEnter2D(Collider2D collison)
    {
        playerIsHere = true;
        int playerDone = PlayerPrefs.GetInt(DialogueManager.INICIAL_KEY, 0);

        if(collison.CompareTag("Player") && playerDone == 0)
        {
            interactionNotice.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collison)
    {
        if(collison.CompareTag("Player"))
        {
            playerIsHere = false;
            if (interactionNotice != null)
            {
                interactionNotice.gameObject.SetActive(false);
            }
        }
    }
}
