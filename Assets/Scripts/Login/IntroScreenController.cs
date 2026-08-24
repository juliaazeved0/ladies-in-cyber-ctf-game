using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla a sequencia da fase, alternando entre estagios dos textos conforme a
/// jogadora pressiona uma tecla de avanco, e finaliza descarregando a cena de inicio.
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

    //Chave usada para marcar que a jogadora ja viu a introducao,
    //evitando que ela seja mostrada novamente em sessoes futuras
    public const string INTRO_KEY = "introductionComplete";

    private int currentStage = 0;
    private bool canAdvance = true;

    void Start()
    {
        //Garante que os elementos de "sucesso" comecem escondidos
        if(flagButton != null) flagButton.SetActive(false);
        if(flagSuccessPanel != null) flagSuccessPanel.SetActive(false);
    }

    void Update()
    {
        bool isSuccessPanelActive = flagSuccessPanel != null && flagSuccessPanel.activeSelf;
      
        //AVanca de estagio com a tecla E, desde que o painel de sucesso nao esteja visivel e o
        //avanco nao tenha sido bloqueado
        if(Input.GetKeyDown(KeyCode.E) && !isSuccessPanelActive && canAdvance)
        {
            AdvanceStage();
        }
    }

    /// <summary>
    /// Chamado pelo botao e captura da flag. Revela o painel de sucesso, gera a flag
    /// decodificada via SafeBase e a registra no FlagManager.
    /// </summary>
    public void OnFlagButtonClicked()
    {
        if(flagSuccessPanel != null)
        {
            flagSuccessPanel.SetActive(true);

            if(flagText != null) flagText.SetActive(false);
            if(flagButton != null) flagButton.SetActive(false);

            string newFlag = SafeBase.ViewBase(SafeBase.flag_0);

            if(FlagManager.Instance != null)
            {
                FlagManager.Instance.SaveFlag("Introdução", newFlag);
            }
        }
    }

    //Marca a introducao como concluida em PlayerPrefs e descarrega a cena de introducao
    public void FinishIntroduction()
    {
        PlayerPrefs.SetInt(INTRO_KEY, 1);
        PlayerPrefs.Save();

        Debug.Log("Introducao concluida. Descarregando cena 'Introduction'...");
        SceneManager.UnloadSceneAsync("Introduction");
    }

    /// <summary>
    /// Avanca para o proximo estagio da introducao, ativando/desativando
    /// os elementos da UI correspondentes a cada etapa da narrativa.
    /// </summary>
    void AdvanceStage()
    {
        currentStage++;

        ToggleAllTexts(false); //Desliga todos os textos antes de ativar os do novo estagio

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

                //A partir daqui, o avanco deixa de depender da tecla E e
                //passa a depender do clique no botao da flag
                canAdvance = false;
                break;
            
            case 4:
                FinishIntroduction();
                break;
            
            default:
                break;
        }
    }

    /// <summary>
    /// Liga ou desliga todos os textos de estagio de uma vez,
    /// usado como um reset antes de ativar o texto do estagio atual.
    /// </summary>
    private void ToggleAllTexts(bool state)
    {
        if(warningText != null) warningText.SetActive(state);
        if(normalText != null) normalText.SetActive(state);
        if(objectiveText != null) objectiveText.SetActive(state);
        if(flagText!= null) flagText.SetActive(state);
    }
}