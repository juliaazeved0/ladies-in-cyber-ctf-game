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
    public Button confirmButton; // Opcional

    [Header("Dinamic variable")]
    public PulseOutline pulsingObject; // Opcional

    private bool readyToSpeak = false;
    protected bool isTalking = false; // <--- ADICIONADO AQUI: O crachá que diz se este NPC é o dono da conversa

    [Header("Buttons")]
    public Button buttonExit;

    [Header ("Nodes")]
    public NPCDialogueNode firstNode;
    protected NPCDialogueNode dialogueCurrent;

    [Header("Control inventory")]
    public static bool isSimpleDialogueActive = false;

    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Update()
    {
        // <--- MODIFICADO AQUI: Agora ele exige que readyToSpeak seja true E que isTalking seja true!
        if(!readyToSpeak || !isTalking)
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

        isSimpleDialogueActive = true;
        isTalking = true; // <--- ADICIONADO AQUI: Pega o crachá de fala ao iniciar a conversa

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
        else
        {
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

        if(characterNPC != null)
            characterNPC.sprite = node.characterNPC;
    }

    public virtual void NextTalk()
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
            if(confirmButton != null)
                confirmButton.gameObject.SetActive(true);

            if(buttonExit != null)
                buttonExit.gameObject.SetActive(false);
        }
    }

    public void ExitDialogue()
    {
        isSimpleDialogueActive = false;
        isTalking = false; // <--- ADICIONADO AQUI: Devolve o crachá de fala ao sair da conversa

        CanvasManager.Instance.ClosedPanel(panelDialogue.name);
        CanvasManager.Instance.ToggleMiniMap(true);
    }

    public virtual void ConfirmHelp()
    {
        ExitDialogue();

        if(pulsingObject != null)
        {
            pulsingObject.StartPulsing();
            pulsingObject = null;
        }
    }
}