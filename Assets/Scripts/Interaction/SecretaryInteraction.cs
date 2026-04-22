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
        if(interactionNotice != null) interactionNotice.gameObject.SetActive(false);
    }

    void Update()
    {
        int playerDone = PlayerPrefs.GetInt(DialogueManager.INICIAL_KEY, 0); //Verifica se o dialogo inicial ja foi feito

        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && playerDone == 0) //Apenas dispara o dialogo se a jogadora estiver perto e clicar na tecla E
        { 
            if (interactionNotice != null) interactionNotice.gameObject.SetActive(false);
    
            if(dialogueManager != null) dialogueManager.StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.CompareTag("Player")) //Verifica se eh a player
        {
            int playerDone = PlayerPrefs.GetInt(DialogueManager.INICIAL_KEY, 0);

            if(playerDone == 0)
            {
                playerIsHere = true; //Apenas vira TRUE se for a player
                if(interactionNotice != null) interactionNotice.gameObject.SetActive(true);
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