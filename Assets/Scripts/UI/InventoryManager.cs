using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
[Header("Settings")]
    public float velocity = 10f; 

    private RectTransform currentButton; 

    void Update()
    {
        if (currentButton != null)
        {
            transform.position = Vector3.Lerp(transform.position, currentButton.position, Time.deltaTime * velocity);
        }
    }

    public void OnClickBag(){

    }

    public void OnClickPlayBook(){
        
    }

    public void MoveToButton(RectTransform newButton)
    {
        gameObject.SetActive(true); 
        
        currentButton = newButton;
    }
}
