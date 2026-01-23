using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Buttons")]
    public Button buttonExit;


    [Header ("Nodes")]
    public NPCDialogueNode firstNode;
    private NPCDialogueNode dialogueCurrent;

    public const string PLAYER_NAME_KEY = "PLAYER_NAME";


    void Update()
    {
        if(panelDialogue.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
           NextTalk();
        }
    }

    void Start()
    {
        
        panelDialogue.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");
        playerNameplate.text = namePlayer.ToUpper();
    }

    public void StartDialogue(NPCDialogueNode inicialNode)
    {
        firstNode = inicialNode;

        if (firstNode != null)
        {
            panelDialogue.SetActive(true);
            miniMapCanvas.SetActive(false);
            cameraMiniMap.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            

            DialogueView(firstNode);
        }
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
        panelDialogue.SetActive(false);
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);

    }
}
