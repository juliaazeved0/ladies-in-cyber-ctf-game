using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoxManager : MonoBehaviour
{
    [Header("Painéis do Computador")] //Cria um título no Inspector para organizar
    //Variáveis para os painéis que estão dentro do computador
    public GameObject panelInventory;
    public GameObject panelTerminal;
    public GameObject panelNetwatch;
    public GameObject panelDetails;
    public GameObject panelWiresharkError;

    private void SetPanel(GameObject panel, bool state) //Recebe um booleano para ligar ou desligar o objeto
    {
        if(panel != null) //Verifica se o painel foi arrastado para a variável
        {
            panel.SetActive(state); //Ativa ou desativa o objeto
        }
    }

    //Métodos para abrir os painéis. => é uma forma curta de escrever uma função que só tem uma linha
    public void OpenInventory() => SetPanel(panelInventory, true);
    public void OpenTerminal() => SetPanel(panelTerminal, true);
    public void OpenNetwatch() => SetPanel(panelNetwatch, true);
    public void OpenDetails() => SetPanel(panelDetails, true);
    public void OpenWiresharkError() => SetPanel(panelWiresharkError, true);

    //Métodos para fechar os painéis
    public void CloseInventory() => SetPanel(panelInventory, false);
    public void CloseTerminal() => SetPanel(panelTerminal, false);
    public void CloseNetwatch() => SetPanel(panelNetwatch, false);
    public void CloseDetails() => SetPanel(panelDetails, false);
    public void CloseWiresharkError() => SetPanel(panelWiresharkError, false);

    public void BackMap() //Método para desativar o objeto "pai", retornando ao mapa do jogo
    {
        //transform.parent é o objeto que está acima desde na hierarquia e depois é desativado o objeto "pai" completamente
        transform.parent.gameObject.SetActive(false);
    }
}
