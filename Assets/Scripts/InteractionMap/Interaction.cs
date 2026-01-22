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

    public SimpleDialogue simpleDialogue;
    public NPCDialogueNode firstNode;

    void Start()
    {
        interactionNotice.SetActive(false);
        //decidir se preciso mesmo fazer duas verificações de status
        CheckChallengeStatus();
    }

    
    void Update()
    {
        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted && !simpleDialogue.panelDialogue.activeSelf)
        {
            interactionNotice.SetActive(false);
            miniMapCamera.SetActive(false);
            simpleDialogue.StartDialogue(firstNode);
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
            
            if (interactionNotice != null) 
                {
                    interactionNotice.SetActive(false);
                }
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
