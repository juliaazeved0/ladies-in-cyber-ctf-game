using UnityEngine;

/// <summary>
/// Gerencia o comportamento dos botoes dentro da interface fisica do servidor.
/// Faz a ponte entre o hardware do servidor e o software do computador.
/// </summary>
public class ServerButton : MonoBehaviour
{
    [Header("Server Panels")]
    [Tooltip("Painel do desafio tecnico.")]
    public GameObject challengePanel;

    [Tooltip("Painel que mostra a conexao fisica dos cabos.")]
    public GameObject connectionPanel;

    [Header("References")]
    [Tooltip("Referencias ao gerenciador de paineis do computador.")]
    public ManagerPanels managerPanels;

    [Tooltip("Referencia ao controlador de estado do servidor.")]
    public ServerController serverController;

    /// <summary>
    /// Executado ao clicar no botao de concluir etapa no servidor.
    /// Transiciona para a conexao de cabos e atualiza o estado global.
    /// </summary>
    public void OnClickButton()
    {
        //Desativa a etapa de desafio e abre a de conexao
        if(challengePanel != null) challengePanel.SetActive(false);
        if(connectionPanel != null) connectionPanel.SetActive(true);

        //Avisa o sistema do PC que o cabo fisico foi conectado
        if(managerPanels != null)
        {
            managerPanels.isCableConnected = true;
        }

        //Notifica o servidor que o desafio tecnico foi vencido
        if(serverController != null)
        {
            serverController.CompleteServer();
        }
    }

    /// <summary>
    /// Fecha as interfaces do servidor para retornar a exploracao do mapa.
    /// </summary>
    public void BackToMap()
    {
        if(challengePanel != null) challengePanel.SetActive(false);
        if(connectionPanel != null) connectionPanel.SetActive(false);
    }
}