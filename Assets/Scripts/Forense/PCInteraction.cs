using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInteraction : MonoBehaviour
{
    private PulseOutline pulse; //Variável para guardar o script "PulseOutline", ele é responsável pelo efeito de brilho ao redor do objeto

    private void Start()
    {
        pulse = GetComponentInParent<PulseOutline>(); //Localiza o script na hierarquia acima desde objeto e guarda em uma variável para permitir a comunicação entre scripts
    }

    private void OnTriggerEnter2D(Collider2D other) //Evento disparado quando algo entra ná área do Collider, ou seja, o gatilho
    {
        if (other.CompareTag("Player")) //Verifica se o objeto que entrou tem a tag "Player"
        {
            pulse.StartPulsing(); //Se for, ativa o script "pulse" para começar o efeito visual
        }
    }

    private void OnTriggerExit2D(Collider2D other) //Ao contrário da função acima, quando algo sai da área do Collider
    {
        if (other.CompareTag("Player")) //Verifica se o objeto que saiu é o "Player"
        {
            pulse.StopPulsing(); //Se afastou, ativa o script "pulse" para parar o efeito
        }
    }
}
