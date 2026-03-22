using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Settings - Panels")]
    public GameObject panelBagBackground;
    public GameObject panelPlayBookBackground;

    [Header("Settings - Buttons & Border")]
    public RectTransform buttonBag;
    public RectTransform buttonPlayBook;
    public RectTransform borderImage; 
    public GameObject inventoryPanel;

    public float velocity = 10f; 

    private RectTransform currentButton; 

    void Update()
    {
        if(DialogueManager.isDialogueActive == false)
        {
            ToggleInventory();
        }
        if(SimpleDialogue.isSimpleDialogueActive == false)
        {
            ToggleInventory();
        }

        if (currentButton != null && borderImage != null)
        {
            borderImage.position = Vector3.Lerp(borderImage.position, currentButton.position, Time.deltaTime * velocity);
        }
    }

    public void OnClickBag()
    {
        CanvasManager.Instance.OpenPanel(panelBagBackground.name);
        MoveToButton(buttonBag);
    }

    public void OnClickPlayBook()
    {
        CanvasManager.Instance.OpenPanel(panelPlayBookBackground.name);
        MoveToButton(buttonPlayBook);
    }
    

    public void ExitInventory()
    {
        CanvasManager.Instance.ClosedPanel(gameObject.name);
        borderImage.gameObject.SetActive(false);
    }

    public void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true); 
        currentButton = newButton;
    }

    public void ToggleInventory()
    {
        bool isOpenPanel = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isOpenPanel);
    }
}