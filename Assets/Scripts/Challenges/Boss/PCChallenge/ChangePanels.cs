using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BashTerminal;
using UnityEngine.SceneManagement;

public class ChangePanels : MonoBehaviour
{
    [Header("Panels")] 
    public GameObject desktopBackground;
    public GameObject panelNotes;
    public GameObject panelSteghideError;
    public GameObject panelSteghideBeach;
    public GameObject panelMetadataInfo;
    public GameObject panelSuccessFlag;

    [Header("External References")]
    public Button steghideButton;
    public GameObject exitButtonGeral;

    /*Fecha todos os sub-paineis do desktop sem afetar o painel pai (challengePanel).
    Nunca use CanvasManager.OpenPanel aqui — ele fecha TODOS os paineis da cena,
    incluindo o challengePanel, que eh pai destes sub-paineis.*/
    private void FecharTodosOsSubPaineis()
    {
        if(panelNotes != null)         panelNotes.SetActive(false);
        if(panelSteghideError != null) panelSteghideError.SetActive(false);
        if(panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
        if(panelMetadataInfo != null)  panelMetadataInfo.SetActive(false);
        if(panelSuccessFlag != null)   panelSuccessFlag.SetActive(false);
    }

    //--- PAINEL DE NOTAS ---
    public void AbrirPanelNotes()
    {
        FecharTodosOsSubPaineis();

        if(panelNotes != null) panelNotes.SetActive(true);
    }

    public void FecharPanelNotes()
    {
        if(panelNotes != null) panelNotes.SetActive(false);
    }

    //--- PAINEL DE ERRO DO STEGHIDE ---
    public void AbrirPanelSteghideError()
    {
        FecharTodosOsSubPaineis();

        if(panelSteghideError != null) panelSteghideError.SetActive(true);
    }

    public void FecharPanelSteghideError()
    {
        if(panelSteghideError != null) panelSteghideError.SetActive(false);
    }

    //--- PAINEL DA PRAIA (SUCESSO STEGHIDE) ---
    public void AbrirPanelSteghideBeach()
    {
        FecharTodosOsSubPaineis();

        if(panelSteghideBeach != null)
        {
            TerminalBoss.challengeSolved = true;
            panelSteghideBeach.SetActive(true);
        }
    }

    public void FecharPanelSteghideBeach()
    {
        if(panelSteghideBeach != null) panelSteghideBeach.SetActive(false);
    }

    //--- PAINEL DE METADADOS ---
    public void AbrirPanelMetadadaInfo()
    {
        FecharTodosOsSubPaineis();

        if(panelMetadataInfo != null) panelMetadataInfo.SetActive(true);
    }

    public void FecharPanelMetadadaInfo()
    {
        if(panelMetadataInfo != null) panelMetadataInfo.SetActive(false);
    }

   //--- PAINEL DA FLAG FINAL ---
    public void AbrirPanelSuccessFlag()
    {
        FecharTodosOsSubPaineis();

        if(panelSuccessFlag != null)
        {
            panelSuccessFlag.SetActive(true);

            //1. Esconde o botao de "Sair" do PC para a jogadora nao fugir do final
            if(exitButtonGeral != null) 
                exitButtonGeral.SetActive(false);

            if(FlagManager.Instance != null)
            {
                string newFlag = SafeBase.ViewBase(SafeBase.flag_8);
                FlagManager.Instance.SaveFlag("BOSS", newFlag);
            }

            //2. Inicia o cronometro para avancar sozinho
            StartCoroutine(AvancarParaFinalAutomaticamente());
        }
    }
    private IEnumerator AvancarParaFinalAutomaticamente()
    {
        //Espera 4 segundos para a jogadora ler a tela de Sucesso e ver a Flag
        yield return new WaitForSeconds(4f);
        
        //Chama a funcao que você ja tem para fechar o PC e abrir a bolsa
        FecharPanelSuccessFlag();
    }

  public void FecharPanelSuccessFlag()
    {
        //1. Fecha o painel de sucesso da flag
        if(panelSuccessFlag != null) panelSuccessFlag.SetActive(false);

        //2. Desliga o fundo do PC a forca para ele nao ficar "escondido" atras da bolsa
        if(desktopBackground != null) desktopBackground.SetActive(false);

        //3. Abre a bolsa de flags com o aviso de ultima chance antes dos creditos
        if (InventoryManager.Instance != null) InventoryManager.Instance.AbrirBolsaFinalizacaoBoss();
    }

    //--- LOGICA DO BOTAO STEGHIDE NA DESKTOP ---
    public void AoClicarNoBotaoSteghide()
    {
        if(TerminalBoss.challengeSolved == false)
        {
            AbrirPanelSteghideError();
        }
        else
        {
            AbrirPanelSteghideBeach();
        }
    }
}