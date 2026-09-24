using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BashTerminal;

/// <summary>
/// Controla a navegacao entre os sub-paineis do desafio "Steghide" do Boss
/// (notas, erro, resultado, metadados e flag de sucesso), incluindo a 
/// liberacao da flag final dia FlagManager ao completar o desafio.
/// </summary>
public class ChangePanels : MonoBehaviour
{
    [Header("Panels")]
    [Tooltip("Tela do desktop, desativada ao finalizar o desafio do Boss.")]
    public GameObject desktopBackground;

    [Tooltip("Painel com as notas do desafio.")]
    public GameObject panelNotes;

    [Tooltip("Painel exibido quando o dsafio steghide ainda nao foi resolvido.")]
    public GameObject panelSteghideError;

    [Tooltip("Painel exibido com o resultado do desafio steghide, apos resolvido.")]
    public GameObject panelSteghideBeach;

    [Tooltip("Painel com informacoes de metadados do desafio.")]
    public GameObject panelMetadataInfo;

    [Tooltip("Painel final exibido ao completar o desafio, com a flag liberada.")]
    public GameObject panelSuccessFlag;

    [Header("External References")]
    [Tooltip("Botao que inicia a verificacao do desafio steghide.")]
    public Button steghideButton;

    [Tooltip("Botao de saida geral, ocultado durante a exibicao da flag de sucesso.")]
    public GameObject exitButtonGeral;

    private void CloseAllSubPanels()
    {
        if(panelNotes != null)         panelNotes.SetActive(false);
        if(panelSteghideError != null) panelSteghideError.SetActive(false);
        if(panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
        if(panelMetadataInfo != null)  panelMetadataInfo.SetActive(false);
        if(panelSuccessFlag != null)   panelSuccessFlag.SetActive(false);
    }

    public void OpenNotesPanel()
    {
        CloseAllSubPanels();

        if(panelNotes != null) panelNotes.SetActive(true);
    }

    public void CloseNotesPanel()
    {
        if(panelNotes != null) panelNotes.SetActive(false);
    }

    public void OpenSteghideErrorPanel()
    {
        CloseAllSubPanels();

        if(panelSteghideError != null) panelSteghideError.SetActive(true);
    }

    public void CloseSteghideErrorPanel()
    {
        if(panelSteghideError != null) panelSteghideError.SetActive(false);
    }

    public void OpenSteghideBeachPanel()
    {
        CloseAllSubPanels();

        if(panelSteghideBeach != null)
        {
            TerminalBoss.challengeSolved = true;
            panelSteghideBeach.SetActive(true);
        }
    }

    public void CloseSteghideBeachPanel()
    {
        if(panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
    }

    public void OpenMetadataInfoPanel()
    {
        CloseAllSubPanels();

        if(panelMetadataInfo != null) panelMetadataInfo.SetActive(true);
    }

    public void CloseMetadataInfoPanel()
    {
        if(panelMetadataInfo != null) panelMetadataInfo.SetActive(false);
    }

    /// <summary>
    /// Exibe o painel de flag de sucesso, salva a flag do Boss no FlagManager
    /// e agenda o fechamento automatico apos alguns segundos.
    /// </summary>
    public void OpenSuccessFlagPanel()
    {
        CloseAllSubPanels();

        if(panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(true);

            if(exitButtonGeral != null) 
                exitButtonGeral.SetActive(false);

            if(FlagManager.Instance != null)
            {
                /*A flag nao fica em texto puro no codigo para dificultar que a jogadora a encontre
                 inspecionando os arquivos do jogo. SafeBase decodifica o valor em runtime*/
                string newFlag = SafeBase.ViewBase(SafeBase.flag_8);
                FlagManager.Instance.SaveFlag("BOSS", newFlag);
            }

            StartCoroutine(AdvanceToFinalAutomatically());
        }
    }
    private IEnumerator AdvanceToFinalAutomatically()
    {
        yield return new WaitForSeconds(4f);

        CloseSuccessFlagPanel();
    }

  public void CloseSuccessFlagPanel()
    {
        if(panelSuccessFlag != null) panelSuccessFlag.SetActive(false);

        if(desktopBackground != null) desktopBackground.SetActive(false);

        if(InventoryManager.Instance != null) InventoryManager.Instance.AbrirBolsaFinalizacaoBoss();
    }

    /// <summary>
    /// Abre o painel de resultado do steghide caso o desafio do terminal
    /// ja tenha sido resolvido, caso contrario, mostra o painel de erro.
    /// </summary>
    public void OnSteghideButtonClicked()
    {
        if(TerminalBoss.challengeSolved == false)
        {
            OpenSteghideErrorPanel();
        }
        else
        {
            OpenSteghideBeachPanel();
        }
    }
}