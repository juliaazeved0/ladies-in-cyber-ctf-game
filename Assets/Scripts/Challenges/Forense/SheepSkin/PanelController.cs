using TMPro;
using UnityEngine;

/// <summary>
/// Gerencia a interface a logica de validacao de dominio para o desafio Sheep´s Skin
/// </summary>
public class PanelController : MonoBehaviour
{
    [Header("Main Panels")]
    [Tooltip("Fundo principal da interface do computador.")]
    public GameObject initialBackground;

    [Tooltip("Painel da ferramenta Netguard para insercao de dominios.")]
    public GameObject netguardPanel;

    [Tooltip("Campo de texto onde a jogadora digita o dominio suspeito.")]
    public TMP_InputField domainInputField;

    [Header("Feedback Panels")]
    public GameObject wrongDomainPanel;
    public GameObject rightDomainPanel;
    public GameObject flagSuccessPanel;

    [Header("Cleanup References")]
    [Tooltip("Paineis internos que devem ser limpos ao sair do PC.")]
    public GameObject mailInputPanel;
    public GameObject ransonwarePanel;

    [Header("Validation Settings")]
    [Tooltip("O dominio exato que a jogadora deve identificador como malicioso.")]
    [SerializeField] private string correctDomain = "http://login-fake-bank.xyz/auth-steal";

    /// <summary>
    /// Abre a ferramenta Netguard e reseta o campo de entrada.
    /// </summary>
    public void OpenNetguardPanel()
    {
        if(initialBackground != null) initialBackground.SetActive(true);

        //Garante que o painel de input esteja limpo antes de mostrar
        if(netguardPanel != null)
        {
            netguardPanel.SetActive(false);
            domainInputField.text = "";
            netguardPanel.SetActive(true);
        } 
    }

    /// <summary>
    /// Ativa o painel de sucesso final e salva a flag no inventario.
    /// </summary>
    public void OpenFlagSuccessPanel()
    {
        domainInputField.text = "";
        if(flagSuccessPanel != null) flagSuccessPanel.SetActive(true);

        //Recupera a flag criptografada e salva no sistema global
        string newFlag = SafeBase.ViewBase(SafeBase.flag_4);

        if(FlagManager.Instance != null)
        {
            FlagManager.Instance.SaveFlag("Sheep’s Skin", newFlag);
        }
    }

    /// <summary>
    /// Fecha um painel especifico passado via Inspector.
    /// </summary>
    public void ClosePanel(GameObject targetPanel)
    {
        if(targetPanel != null)
        {
            domainInputField.text = "";
            targetPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Verifica se o dominio digitado condiz com a resposta correta.
    /// </summary>
    public void SubmitDomain()
    {
        //O uso do Trim() eh essencial para evitar erros por espacos acidentais
        if(domainInputField.text.Trim() == correctDomain)
        {
            domainInputField.text = "";
            if(rightDomainPanel != null) rightDomainPanel.SetActive(true);
        }
        else
        {
            domainInputField.text = "";
            if(wrongDomainPanel != null) wrongDomainPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Desliga a interface do computador e retorna para a exploracao no mapa.
    /// </summary>
    public void BackToMap()
    {
        if(initialBackground != null)
        {
            //Reseta o estado dos paineis internos para uma nova tentativa posterior
            if(mailInputPanel != null) mailInputPanel.SetActive(false);
            if(ransonwarePanel != null) ransonwarePanel.SetActive(false);

            initialBackground.SetActive(false);
        }
    }
}