using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    [Header("Efeitos Visuais")]
    public PulseOutline scriptPulse; //Arrastar o objeto que contém o script PulseOutline

    [Header("Interface de Interação")]
    public GameObject interactionNotice; //Modal de interação

    [Header("Sistema do Desafio")]
    public GameObject initialBackground; //Painel do início do desafio

    private bool playerIsNear = false; //Controla se a jogadora está perto do computador

    void Start()
    {
        initialBackground.SetActive(false); //PCBlackBoxChallenge começar desativado
    }
    void Update()
    {
        if(playerIsNear && Input.GetKeyDown(KeyCode.E)) //Verifica se a jogadora está perto do computador e se pressionou a tecla E
        {
            if(initialBackground != null) //Faz o painel aparecer na tela
            {
                initialBackground.SetActive(true);

                if(interactionNotice != null) interactionNotice.SetActive(false); //Esconde o aviso de interação quando o painel do PC abre
            }
        } 
    }
    private void OnTriggerEnter2D(Collider2D collision) //Função chamada automaticamente quando a jogadora entra no collider marcado
    {
        if (collision.CompareTag("Player")) //Verifica se o personagem entrou na área de colisão
        {
            playerIsNear = true; //Marca que a jogadora entrou na área, então, ela pode interagir

            if (scriptPulse != null) scriptPulse.StartPulsing(); //Liga o brilho ao redor do computador assim que a jogadora se aproxima
            if(interactionNotice != null) interactionNotice.SetActive(true); //Mostra o aviso de interação
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Função para quando a jogadora sai da área de interação
    {
        if (collision.CompareTag("Player")) //Verifica se a personagem saiu da área de colisão
        {
            playerIsNear = false; //A jogadora não pode mais interagir com o computador

            if (scriptPulse != null) scriptPulse.StopPulsing(); //Desativa o brilho do computador assim que a jogadora se afasta
            if(interactionNotice != null) interactionNotice.SetActive(false); //Esconde o aviso de interação
        }
    }
}
