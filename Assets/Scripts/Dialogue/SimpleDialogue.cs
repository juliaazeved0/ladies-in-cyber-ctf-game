using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SimpleDialogue : MonoBehaviour
{
   
    [Header("Elements UI")]
    public GameObject panelDialogue;
    public TextMeshProUGUI textDialogue;
    public Image characterNPC; 
    public WriteMachine writeMachine;
    public TextMeshProUGUI playerNameplate;
    public Image characterPlayer;
    public GameObject miniMapCanvas;
    public GameObject cameraMiniMap;
    public Button confirmButton;
    
    [Header("Dinamic variable")]
    public PulseOutline pulsingObject;

    private bool readyToSpeak = false; 

    [Header("Buttons")]
    public Button buttonExit;


    [Header ("Nodes")]
    public NPCDialogueNode firstNode;
    private NPCDialogueNode dialogueCurrent;

    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Update()
    {
        if(!readyToSpeak)
        {
            return;
        }
        
        if(panelDialogue.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
           NextTalk();
        }
    }

    void Start()
    {
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");
        playerNameplate.text = namePlayer.ToUpper();
    }

    public void StartDialogue(NPCDialogueNode inicialNode)
    {
        StopAllCoroutines();

        if(textDialogue != null) textDialogue.text = "";
        if(confirmButton != null) confirmButton.gameObject.SetActive(false);
        if(buttonExit != null) buttonExit.gameObject.SetActive(true);
        
        firstNode = inicialNode;

        if (firstNode != null)
        {
            CanvasManager.Instance.OpenPanel(panelDialogue.name);
            CanvasManager.Instance.ToggleMiniMap(false);

        
            DialogueView(firstNode);

            readyToSpeak = false;
            StartCoroutine(ReleaseInput());
        }
        else{
            Debug.LogError("node vazio");
        }
    }

    IEnumerator ReleaseInput()
    {
        yield return new WaitForSeconds(0.2f);
        readyToSpeak = true;
    }

    public void DialogueView(NPCDialogueNode node)
    {
        dialogueCurrent = node;

        writeMachine.Run(node.talkNPC, textDialogue);

        characterNPC.sprite = node.characterNPC;

    }

    public void NextTalk()
    {
       //if (WriteMachine.IsTyping)
       // {
       //    WriteMachine.Complete();
       //    return;
       //}

        if(dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
        else 
        {
            confirmButton.gameObject.SetActive(true);
            buttonExit.gameObject.SetActive(false);
        }
    }

    public void ExitDialogue()
    {
        CanvasManager.Instance.ClosedPanel(panelDialogue.name);
        CanvasManager.Instance.ToggleMiniMap(true);
        
    }

    public virtual void ConfirmHelp()
    {
        ExitDialogue();
      if (pulsingObject != null)
        {
            pulsingObject.StartPulsing();
            pulsingObject = null; 
        }
    }
    

}
