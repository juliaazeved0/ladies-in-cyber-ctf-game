using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Settings object interactable")]
    private bool playerIsHere;
    public GameObject interactionNotice;
    public GameObject challengePanel;

    void Start()
    {
        interactionNotice.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    void Update()
    {
        if(playerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            CanvasManager.Instance.ToggleMiniMap(false);
            CanvasManager.Instance.OpenPanel(challengePanel.name);
        }
    }

}
