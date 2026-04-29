using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// Controlador principal do sistema de dialogo.
/// Gerencia a exibicao de textos, troca de nos e integracao com efeitos de cenario.
/// </summary>
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
    [Tooltip("Objeto que recebera destaque visual apos o termino do dialogo.")]
    public PulseOutline pulsingObject;

    private bool readyToSpeak = false;
    protected bool isTalking = false;

    [Header("Buttons")]
    public Button buttonExit;

    [Header("Nodes")]
    public NPCDialogueNode firstNode;
    protected NPCDialogueNode dialogueCurrent;

    [Header("Control inventory")]
    public static bool isSimpleDialogueActive = false;

    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Update()
    {
        //So processa input se o dialogo estiver ativo e pronto
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
        //Recupera o nome da jogadora para mostrar na tela
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");
        playerNameplate.text = namePlayer.ToUpper();
    }

    /// <summary>
    /// Inicia ua nova conversa a partir de um no especifico.
    /// </summary>
    public void StartDialogue(NPCDialogueNode inicialNode)
    {
        StopAllCoroutines();

        isSimpleDialogueActive = true;
        isTalking = true;

        if(textDialogue != null) textDialogue.text = "";
        if(confirmButton != null) confirmButton.gameObject.SetActive(false);
        if(buttonExit != null) buttonExit.gameObject.SetActive(true);

        firstNode = inicialNode;

        if(firstNode != null)
        {
            CanvasManager.Instance.OpenPanel(panelDialogue.name);
            CanvasManager.Instance.ToggleMiniMap(false);

            DialogueView(firstNode);

            //Evita que o mesmo clique que iniciou o dialogo ja pule a primeira frase
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

    /// <summary>
    /// Avanca o dialogo. Se estiver digitando, completa a frase. Se terminou, passa para o proximo no.
    /// </summary>
    public virtual void NextTalk()
    {
        if(writeMachine.IsTyping)
        {
            writeMachine.Complete();
            return;
        }

        if(dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
        else
        {
            //Ativa o botao de conclusao
            if(confirmButton != null)
                confirmButton.gameObject.SetActive(true);

            if(buttonExit != null)
                buttonExit.gameObject.SetActive(false);
        }
    }

    public void ExitDialogue()
    {
        isSimpleDialogueActive = false;
        isTalking = false;

        CanvasManager.Instance.ClosedPanel(panelDialogue.name);
        CanvasManager.Instance.ToggleMiniMap(true);
    }

    /// <summary>
    /// Chamado pelo botao de confirmacao ao final da conversa
    /// </summary>
    public virtual void ConfirmHelp()
    {
        ExitDialogue();

        //Se houver um objeto para destacar, ativa a pulsacao
        if(pulsingObject != null)
        {
            pulsingObject.StartPulsing();
            pulsingObject = null; //Limpa para a proxima interacao
        }
    }
}