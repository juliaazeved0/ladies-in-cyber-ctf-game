using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    
    public GameObject panelDialogue;
    
    void Start()
    {
        panelDialogue.SetActive(false);
    }

    void Update()
    
    {
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E foi pressionado");
            panelDialogue.SetActive(true);
        }
    }


}
