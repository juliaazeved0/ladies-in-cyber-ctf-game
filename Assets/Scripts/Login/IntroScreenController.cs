using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla a sequencia de telas da introducao do jogo: avanca por estagios
/// via tecla de interacao, exibe o botao de flag ao final e libera a flag
/// via FlagManager. Ao concluir, marca a introducao como vista e descarrega
/// a propria cena.
/// </summary>
public class IntroScreenController : MonoBehaviour
{
    [Header("Background Panels")]
    [Tooltip("Painel de fundo exibido no estagio inicial (aviso).")]
    [SerializeField] private GameObject warningBackground;

    [Tooltip("Painel de fundo exibido a partir do estagio 1 em diante.")]
    [SerializeField] private GameObject normalBackground;

    [Tooltip("Painel exibido ao clicar no botao de flag, confirmando a liberacao.")]
    [SerializeField] private GameObject flagSuccessPanel;

    [Header("Text Interface Elements")]
    [Tooltip("Texto de aviso exibido no estagio inicial.")]
    [SerializeField] private GameObject warningText;

    [Tooltip("Texto exibido no estagio 1.")]
    [SerializeField] private GameObject normalText;

    [Tooltip("Texto com o objetivo, exibido no estagio 2.")]
    [SerializeField] private GameObject objectiveText;

    [Tooltip("Texto exibido no estagio 3, junto ao botao de flag.")]
    [SerializeField] private GameObject flagText;

    [Header("Interactions and Inputs")]
    [Tooltip("Botao que libera a flag da introducao ao ser clicado.")]
    [SerializeField] private GameObject flagButton;

    [Tooltip("Indicador visual da tecla de interacao (ex: Pressione E).")]
    [SerializeField] private GameObject promptKey;

    //Chave usada no PlayerPrefs para marcar que a introducao ja foi concluida
    public const string INTRO_KEY = "introductionComplete";

    private int currentStage = 0;
    private bool canAdvance = true;

    void Start()
    {
        if(flagButton != null) flagButton.SetActive(false);
        if(flagSuccessPanel != null) flagSuccessPanel.SetActive(false);
    }

    void Update()
    {
        bool isSuccessPanelActive = flagSuccessPanel != null && flagSuccessPanel.activeSelf;
      
        if(Input.GetKeyDown(KeyCode.E) && !isSuccessPanelActive && canAdvance)
        {
            AdvanceStage();
        }
    }

    //Exibe o painel de sucesso e salva a flag da introducao via FlagManager
    public void OnFlagButtonClicked()
    {
        if(FlagManager.Instance == null) return;
        FlagManager.Instance.SaveFlag("Introdução", SafeBase.ViewBase(SafeBase.flag_0));
        if(flagSuccessPanel != null) flagSuccessPanel.SetActive(true);
        if(flagText != null) flagText.SetActive(false);
        if(flagButton != null) flagButton.SetActive(false);
    }

    //Marca a introducao como concluida e descarrega esta cena
    public void FinishIntroduction()
    {
        if(!FlagManager.HasSavedFlag(SafeBase.ViewBase(SafeBase.flag_0))) return;

        PlayerPrefs.SetInt(INTRO_KEY, 1);
        PlayerPrefs.Save();

        Debug.Log("Introducao concluida. Descarregando cena 'Introduction'...");
        SceneManager.UnloadSceneAsync("Introduction");
    }

    void AdvanceStage()
    {
        currentStage++;

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

                canAdvance = false;
                break;
            
            case 4:
                FinishIntroduction();
                break;
            
            default:
                break;
        }
    }

    private void ToggleAllTexts(bool state)
    {
        if(warningText != null) warningText.SetActive(state);
        if(normalText != null) normalText.SetActive(state);
        if(objectiveText != null) objectiveText.SetActive(state);
        if(flagText!= null) flagText.SetActive(state);
    }
}