using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    [Header("Efeitos Visuais")]
    public PulseOutline scriptPulse; //Arrastar o objeto que contém o script PulseOutline

    [Header("Interface de Interação")]
    public GameObject interactionNotice; //Modal de interação

    [Header("Sistema do Desafio")]
    public GameObject initialBackground; //Painel do início do desafio

    private bool playerIsNear = false; //Controla se a jogadora está na área

     void Update()
    {
        if(playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            if(initialBackground != null)
            {
                initialBackground.SetActive(true);

                if(interactionNotice != null) interactionNotice.SetActive(false);
            }
        } 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Verifica se o personagem entrou na área de colisão
        {
            playerIsNear = true;

            if (scriptPulse != null) scriptPulse.StartPulsing(); //Liga o brilho ao redor do computador assim que a jogadora se aproxima
            if(interactionNotice != null) interactionNotice.SetActive(true); //Mostra o aviso de interação
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Verifica se a perosnagem saiu da área de colisão
        {
            playerIsNear = false;

            if (scriptPulse != null) scriptPulse.StopPulsing(); //Desativa o brilho do computador assim que a jogadora se afasta
            if(interactionNotice != null) interactionNotice.SetActive(false); //Esconde o aviso de interação
        }
    }
}
