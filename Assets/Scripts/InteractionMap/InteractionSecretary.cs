using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InteractionSecretary : MonoBehaviour
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
        if (playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
           
            if (interactionNotice != null)
                interactionNotice.gameObject.SetActive(false);

    
            dialogueManager.StartDialogue();
        }
    }
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.CompareTag("Player"))
        {
            playerIsHere = true;
            if (interactionNotice != null)
            {
                interactionNotice.gameObject.SetActive(true);
            }
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
