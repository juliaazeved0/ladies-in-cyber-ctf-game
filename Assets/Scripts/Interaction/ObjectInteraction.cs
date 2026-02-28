using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Settings object interactable")]
    protected bool playerIsHere;
    public GameObject interactionNotice;
    public GameObject challengePanel;

    protected void Start()
    {
        interactionNotice.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsHere = true;
            if(interactionNotice != null)
            {
                interactionNotice.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
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

    protected virtual void Update()
    {
        if(challengePanel != null && challengePanel.activeSelf)
        {
            return;
        }

        if(playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false);
        CanvasManager.Instance.OpenPanel(challengePanel.name);
    }

}
