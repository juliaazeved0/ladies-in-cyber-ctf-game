using UnityEngine;

/// <summary>
/// Gerencia a troca de paineis da interface do computador no desafio Black Box.
/// </summary>
public class ManagerPanels : MonoBehaviour
{
    [Header("Main Panels")]
    [Tooltip("Fundo principal que engloba toda a interface do PC.")]
    public GameObject initialBackground;
    public GameObject netwatchPanel;
    public GameObject inventoryPanel;
    public GameObject terminalPanel;
    public GameObject errorWiresharkPanel;
    public GameObject successWiresharkPanel;
    public GameObject flagPanel;
    public GameObject successFlagPanel;

    [Header("Sub-Panels")]
    [Tooltip("Painel de detalhes que aparece dentro do Netwatch.")]
    public GameObject detailsPanel;

    [Header("References")]
    [Tooltip("Referencia ao controlador do servidor para desbloqueio via rede.")]
    public ServerController scriptServer;

    [Header("State")]
    [Tooltip("Controle para verificar se o cabo fisico foi conectado ao servidor.")]
    public bool isCableConnected = false;

    
    //Metodos de aberturas de paineis
    public void OpenNetwatch() { CloseAllMainPanels(); netwatchPanel.SetActive(true); }
    public void OpenInventory() { CloseAllMainPanels(); inventoryPanel.SetActive(true); }
    public void OpenTerminal() { CloseAllMainPanels(); terminalPanel.SetActive(true); }
    public void OpenWireshark() { CloseAllMainPanels(); errorWiresharkPanel.SetActive(true); }

    /// <summary>
    /// Logica para abrir o Wireshark verificando se o hardware (cabo) esta pronto.
    /// </summary>
    public void OpenSuccessWireshark()
    {
        CloseAllMainPanels();

        if(!isCableConnected)
        {
            //Mostra o erro se o cabo nao estiver conectado, mas libera o servidor para o proximo passo
            errorWiresharkPanel.SetActive(true);
            if(scriptServer != null) scriptServer.UnlockByHacking();
        }
        else
        {
            //Sucesso se o cabo ja estiver conectado
            successWiresharkPanel.SetActive(true);
        }
    }

    public void OpenFlag()
    {
        if(successWiresharkPanel != null) successWiresharkPanel.SetActive(false);
        if(flagPanel != null) flagPanel.SetActive(true);
    }

    /// <summary>
    /// Finaliza o desafio capturando a flag e salvando no inventario global.
    /// </summary>
    public void OpenSuccessFlag()
    {
        if(flagPanel != null) flagPanel.SetActive(false);
        if(successFlagPanel != null) successFlagPanel.SetActive(true);

        //Salvando a flag no inventario usando o sistema de Base64
        string newFlag = SafeBase.ViewBase(SafeBase.flag_3);

        //Evita erro de NullReference caso o sistema tente salvar algo antes do FlagManager estar carregado
        if(FlagManager.Instance != null)
        {
            //Ajustado para incluir o nome do desafio
            FlagManager.Instance.SaveFlag("Black Box", newFlag);
        }
    }

    public void OpenDetails()
    {
        if(detailsPanel != null) detailsPanel.SetActive(true);
    }

    public void CloseOnlyDetails()
    {
        if(detailsPanel != null) detailsPanel.SetActive(false);
    }

    /// <summary>
    /// Desativa todos os paineis principais para evitar sobreposicao de interfaces.
    /// </summary>
    public void CloseAllMainPanels()
    {
        if(netwatchPanel != null) netwatchPanel.SetActive(false);
        if(inventoryPanel != null) inventoryPanel.SetActive(false);
        if(terminalPanel != null) terminalPanel.SetActive(false);
        if(errorWiresharkPanel != null) errorWiresharkPanel.SetActive(false);
        if(successWiresharkPanel != null) successWiresharkPanel.SetActive(false);
        if(flagPanel != null) flagPanel.SetActive(false);
        if(successFlagPanel != null) successFlagPanel.SetActive(false);
    }

    /// <summary>
    /// Sai da interface do computador e retorna para a exploracao do mapa.
    /// </summary>
    public void ReturnToMap()
    {
        if(initialBackground != null)
        {
            initialBackground.SetActive(false);
        }
    }
}