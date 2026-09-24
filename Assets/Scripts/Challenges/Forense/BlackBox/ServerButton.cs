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
        if(challengePanel != null) challengePanel.SetActive(false);
        if(connectionPanel != null) connectionPanel.SetActive(true);

        if(managerPanels != null)
        {
            managerPanels.isCableConnected = true;
        }
        else
        {
            Debug.LogWarning($"[ServerButton] Referência para 'managerPanels' não atribuída em {gameObject.name}.", this);
        }

        if (serverController != null)
        {
            serverController.CompleteServer();
        }
        else
        {
            Debug.LogWarning($"[ServerButton] Referência para 'serverController' não atribuída em {gameObject.name}.", this);
        }
    }

    //Fecha as interfaces do servidor para retornar a exploracao do mapa
    public void BackToMap()
    {
        if(challengePanel != null) challengePanel.SetActive(false);
        if(connectionPanel != null) connectionPanel.SetActive(false);
    }
}