using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonServer : MonoBehaviour
{
    [Header("Painéis do Servidor")] //Título e painéis para arrastar no Inspector
    public GameObject panelChallenge;
    public GameObject panelConnect;

    public ManagerPanels managerPanels; //Referência ao objeto que tem o script ManagerPanels assainado
    public ServerController serverController; //Referência ao objeto que tem o script ServerController assainado

    public void OnClickButton() //Função para quando a jogadora clicar no botão da interface
    {
        panelChallenge.SetActive(false); //Desativa o painel do desafio
        panelConnect.SetActive(true); //Abre o painel de conexão dos cabos

        if(managerPanels != null) //Quando clicar para conectar o cabo, avisa o sistema do PC para ativar a troca dos painéis
        {
            managerPanels.caboConectado = true; //Libera o desafio do Wireshark
        }

        if(serverController != null) //Avisa o servidor que terminou
        {
            serverController.ConcluirServidor(); //Chama o método que está dentro do ServerController (finalizado = true, ou seja, servidor resolvido)
        }
    }

    public void BackMap() //Método para retornar ao mapa
    {
        if(panelChallenge != null) //Fecha o painel do desafio caso ele esteja aberto
        {
            panelChallenge.SetActive(false);
        }

        if (panelConnect != null) //Fecha o painel de conaxão dos cabos
        {
            panelConnect.SetActive(false);
        }
    }
}
