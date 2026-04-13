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

    void Start(){
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
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
        if(panelPlayBookBackground.activeSelf){
            panelPlayBookBackground.SetActive(false);
            panelBagBackground.SetActive(true);
        }
        panelBagBackground.SetActive(true);
        MoveToButton(buttonBag);
    }

    public void OnClickPlayBook()
    {
        if (panelBagBackground.activeSelf){
            panelBagBackground.SetActive(false);
            panelPlayBookBackground.SetActive(true);
        }
        panelPlayBookBackground.SetActive(true);
        MoveToButton(buttonPlayBook);
    }
    

    public void ExitInventory()
    {
        borderImage.gameObject.SetActive(false);
        panelBagBackground.SetActive(false);
        panelPlayBookBackground.SetActive(false);
    }

    public void MoveToButton(RectTransform newButton)
    {
        borderImage.gameObject.SetActive(true); 
        currentButton = newButton;
    }

}