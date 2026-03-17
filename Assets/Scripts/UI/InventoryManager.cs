using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Settings - Panels")]
    public GameObject panelBag;
    public GameObject panelPlayBook;
    public GameObject panelBackgroundInventory;

    [Header("Settings - Buttons & Border")]
    public RectTransform buttonBag;
    public RectTransform buttonPlayBook;
    public RectTransform borderImage; 

    public float velocity = 10f; 

    private RectTransform currentButton; 

    void Start()
    {
        panelBackgroundInventory.SetActive(false);
    }

    void Update()
    {
        if (currentButton != null && borderImage != null)
        {
            borderImage.position = Vector3.Lerp(borderImage.position, currentButton.position, Time.deltaTime * velocity);
        }
    }

    public void OnClickBag()
    {
        panelBackgroundInventory.SetActive(true);
        //CanvasManager.Instance.OpenPanel(panelBackgroundInventory.name);
        CanvasManager.Instance.OpenPanel(panelBag.name);
        MoveToButton(buttonBag);
    }

    public void OnClickPlayBook()
    {
        panelBackgroundInventory.SetActive(true);
        //CanvasManager.Instance.OpenPanel(panelBackgroundInventory.name);
        CanvasManager.Instance.OpenPanel(panelPlayBook.name);
        MoveToButton(buttonPlayBook);
    }
    

    public void ExitInventory()
    {
        panelBackgroundInventory.SetActive(false);
        CanvasManager.Instance.ClosedPanel(gameObject.name);
        borderImage.gameObject.SetActive(false);
        //CanvasManager.Instance.ClosedPanel(panelBackgroundInventory.name);
    }

    public void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true); 
        currentButton = newButton;
    }
}