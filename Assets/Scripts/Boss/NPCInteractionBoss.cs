using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBossInteraction : MonoBehaviour
{
    [Header("Settings NPC")]
    public string uniqueSaveKey; //Chave para salvar se o desafio já foi concluído
    public Image balloonNPC; //Imagem do balão acima do NPC

    [Header("Interaction")]
    public GameObject interactionNotice; //Aviso na tela "Pressionar E"

    [Header("Systems (Assign one or both)")]
    public DialogueManagerBoss dialogueManagerBoss; //Referência ao gerenciador de diálogo

    [Header("Nodes")]
    public DialogueNodeBoss firstNodeBoss; //Primeiro nó do diálogo desse NPC

    private bool playerIsHere = false; //Verifica se a jogadora está perto
    private bool isCompleted = false; //Verifica se o desafio já foi concluído
    private bool isTalking = false; //Controla se o diálogo já começou

    void Start()
    {
        if(interactionNotice != null) interactionNotice.SetActive(false); //Esconde o aviso no início
        if(balloonNPC != null) balloonNPC.gameObject.SetActive(false); //Esconde o balão
        CheckChallengeStatus(); //Verifica se já foi completado antes
    }

    void Update()
    {
        if(playerIsHere && Input.GetKeyDown(KeyCode.E) && !isCompleted) //Verifica se a jogadoraapertou E e não completou o desafio ainda
        {
            if(!isTalking)
            {
                //Se ainda não começou, inicia o diálogo
                StartConversation();
                isTalking = true;
            }
            else
            {
                if(!dialogueManagerBoss.CurrentNodeHasOptions()) //Se já está conversando
                {
                    dialogueManagerBoss.ChooseOption(0); //Se não tem opções, avança automaticamente
                }
                else
                {
                    Debug.Log("Escolha uma opção no mouse para continuar!"); //Se tem opções, força a jogadora a clicar em uma delas
                }
                
            }
        }
    }

    private void StartConversation()
    {
        if(dialogueManagerBoss != null && firstNodeBoss != null)
        {
            dialogueManagerBoss.firstNode = firstNodeBoss; //Define o início
            dialogueManagerBoss.StartDialogue(); //Começa o diálogo
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true; //Jogadora entrou na área de collider para aparecer os popups

            CheckChallengeStatus();

            if(!isCompleted && interactionNotice != null)
            {
                interactionNotice.SetActive(true); //Mostra o aviso "Pressione E"
                if (balloonNPC != null) balloonNPC.gameObject.SetActive(true); //Mostra o balão
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Se a jogadora sair de perto da área de collider, a conversa reseta
            playerIsHere = false;
            isTalking = false;

            if(interactionNotice != null)
            {
                interactionNotice.SetActive(false); //Esconde o aviso

                if(balloonNPC != null) balloonNPC.gameObject.SetActive(false); //Esconde o balão
            }

            //Fecha o painel de diálogo se a jogadora se afastar
            if(dialogueManagerBoss != null) dialogueManagerBoss.panelDialogue.SetActive(false);
        }
    }

    public void CheckChallengeStatus()
    {
        //Se for 1 -> já completou
        //Se for 0 -> ainda não completou
        if(!string.IsNullOrEmpty(uniqueSaveKey))
            isCompleted = PlayerPrefs.GetInt(uniqueSaveKey, 0) == 1;
    }
}