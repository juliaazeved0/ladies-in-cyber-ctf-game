using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey;
    public Image balloonNPC;

    [Header("Dinamic variable")]
    public PulseOutline pulseObjectInitial;


    [Header("Interaction")]
    public GameObject interactionNotice;

    private bool playerIsHere = false;
    private bool isCompleted = false;

    public SimpleDialogue simpleDialogue;
    public NPCDialogueNode firstNode;

    void Start()
    {
        interactionNotice.SetActive(false);
        CheckChallengeStatus();
        balloonNPC.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted && !simpleDialogue.panelDialogue.activeSelf)
        {
            simpleDialogue.pulsingObject = pulseObjectInitial;
            CanvasManager.Instance.ToggleMiniMap(false);
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
                balloonNPC.gameObject.SetActive(true);
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
                    balloonNPC.gameObject.SetActive(false);
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
