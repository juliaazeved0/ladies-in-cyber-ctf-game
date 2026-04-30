using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla o fluxo da cena de Introducao, gerenciando o progresso do tutorial,
/// a exibicao de paineis e a captura da flag inicial.
/// </summary>
public class IntroScreenController : MonoBehaviour
{
    [Header("Planos de Fundo")]
    [SerializeField] private GameObject warningBackground;
    [SerializeField] private GameObject normalBackground;
    [SerializeField] private GameObject flagSuccessPanel;

    [Header("Textos")]
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject normalText;
    [SerializeField] private GameObject objectiveText;
    [SerializeField] private GameObject flagText;

    [Header("Interacoes")]
    [SerializeField] private GameObject flagButton;
    [SerializeField] private GameObject promptKey;

    //Chave para persistir o status de conclusao da introducao
    public const string INTRO_KEY = "introductionComplete";

    private int currentStage = 0;
    private bool canAdvance = true;

    void Start()
    {
        if(flagButton != null) flagButton.SetActive(false);
    }

    void Update()
    {
        //Verifica o avanco via teclado (apenas se permitido e painel de sucesso fehcaod)
        if(Input.GetKeyDown(KeyCode.E) && !flagSuccessPanel.activeSelf && canAdvance)
        {
            Advance();
        }
    }

    /// <summary>
    /// Chamado ao clicar no botao da flag. Mostra o painel de sucesso e salva a flag.
    /// </summary>
    public void ShowSuccessFlagPanel()
    {
        if(flagSuccessPanel != null)
        {
            flagSuccessPanel.SetActive(true);
            if(flagText != null) flagText.SetActive(false);
            if(flagButton != null) flagButton.SetActive(false);

            string newFlag = SafeBase.ViewBase(SafeBase.flag_0);
            FlagManager.Instance.SaveFlag("Introdução", newFlag);
        }
    }

    /// <summary>
    /// Finaliza a introducao e descarrega a cena da memoria.
    /// </summary>
    public void CloseSuccessAndFinish()
    {
        PlayerPrefs.SetInt(INTRO_KEY, 1);
        PlayerPrefs.Save();
        SceneManager.UnloadSceneAsync("Introduction");
    }

    /// <summary>
    /// Gerencia a transicao entre os estagios da introducao.
    /// </summary>
    void Advance()
    {
        currentStage++;

        //Reset visual basico
        warningText.SetActive(false);
        normalText.SetActive(false);
        objectiveText.SetActive(false);
        if(flagText != null) flagText.SetActive(false);
        if(flagButton != null) flagButton.SetActive(false);

        switch (currentStage)
        {
            case 1:
                warningBackground.SetActive(false);
                normalBackground.SetActive(true);
                normalText.SetActive(true);
                if(promptKey != null) promptKey.SetActive(true);
                break;

            case 2:
                objectiveText.SetActive(true);
                break;

            case 3:
                flagText.SetActive(true);
                if(flagButton != null) flagButton.SetActive(true);
                if(promptKey != null) promptKey.SetActive(false);
                canAdvance = false; //Bloqueia avanco via tecla E
                break;
            
            case 4:
                CloseSuccessAndFinish();
                break;
            
            default:
                break;
        }
    }
}