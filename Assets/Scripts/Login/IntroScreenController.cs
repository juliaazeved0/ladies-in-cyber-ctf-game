using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla o fluxo da cena de Introducao, gerenciando o progresso do tutorial,
/// a exibicao de paineis e a captura da flag inicial.
/// </summary>
public class IntroScreenController : MonoBehaviour
{
    [Header("Background Panels")]
    [SerializeField] private GameObject warningBackground;
    [SerializeField] private GameObject normalBackground;
    [SerializeField] private GameObject flagSuccessPanel;

    [Header("Text Interface Elements")]
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject normalText;
    [SerializeField] private GameObject objectiveText;
    [SerializeField] private GameObject flagText;

    [Header("Interactions and Inputs")]
    [SerializeField] private GameObject flagButton;
    [SerializeField] private GameObject promptKey;

    //Chave para persistir o status de conclusao da introducao
    public const string INTRO_KEY = "introductionComplete";

    private int currentStage = 0;
    private bool canAdvance = true;

    void Start()
    {
        //Configuracao inicial para garantir que a UI comece no estado correto
        if(flagButton != null) flagButton.SetActive(false);
        if(flagSuccessPanel != null) flagSuccessPanel.SetActive(false);
    }

    void Update()
    {
        //Verifica o avanco via teclado (apenas se permitido e painel de sucesso fechado)
        bool isSuccessPanelActive = flagSuccessPanel != null && flagSuccessPanel.activeSelf;
      
        if(Input.GetKeyDown(KeyCode.E) && !isSuccessPanelActive && canAdvance)
        {
            AdvanceStage();
        }
    }

    /// <summary>
    /// Chamado ao clicar no botao da flag. Mostra o painel de sucesso e salva a flag.
    /// </summary>
    public void OnFlagButtonClicked()
    {
        if(flagSuccessPanel != null)
        {
            flagSuccessPanel.SetActive(true);

            if(flagText != null) flagText.SetActive(false);
            if(flagButton != null) flagButton.SetActive(false);

            //Salva a flag no inventario
            string newFlag = SafeBase.ViewBase(SafeBase.flag_0);

            if(FlagManager.Instance != null)
            {
                FlagManager.Instance.SaveFlag("Introdução", newFlag);
            }
        }
    }

    /// <summary>
    /// Finaliza a introducao e descarrega a cena da memoria.
    /// </summary>
    public void FinishIntroduction()
    {
        PlayerPrefs.SetInt(INTRO_KEY, 1);
        PlayerPrefs.Save();

        Debug.Log("[IntroScreenController] Introducao concluida. Descarregando cena 'Introduction'...");
        SceneManager.UnloadSceneAsync("Introduction");
    }

    /// <summary>
    /// Gerencia a transicao entre os estagios da introducao.
    /// </summary>
    void AdvanceStage()
    {
        currentStage++;

        //Reset visual: Desativa todos os textos antes de ativar o proximo
        ToggleAllTexts(false);

        switch(currentStage)
        {
            case 1:
                if(warningBackground != null) warningBackground.SetActive(false);
                if(normalBackground != null) normalBackground.SetActive(true);
                if(normalText != null) normalText.SetActive(true);
                if(promptKey != null) promptKey.SetActive(true);
                break;

            case 2:
                if(objectiveText != null) objectiveText.SetActive(true);
                break;

            case 3:
                if(flagText != null) flagText.SetActive(true);
                if(flagButton != null) flagButton.SetActive(true);
                if(promptKey != null) promptKey.SetActive(false);

                canAdvance = false; //Bloqueia avanco via tecla E
                break;
            
            case 4:
                FinishIntroduction();
                break;
            
            default:
                break;
        }
    }

    /// <summary>
    /// Metodo auxiliar para evitar repeticao de codigo ao limpar a tela.
    /// </summary>
    private void ToggleAllTexts(bool state)
    {
        if(warningText != null) warningText.SetActive(state);
        if(normalText != null) normalText.SetActive(state);
        if(objectiveText != null) objectiveText.SetActive(state);
        if(flagText!= null) flagText.SetActive(state);
    }
}