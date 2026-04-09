using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenController : MonoBehaviour
{
    [Header("Backgrounds")] //Coloca no Inspector os espaços para arrastar os objetos
    public GameObject backgroundWarning; //Referencia para o fundo de aviso
    public GameObject backgroundNormal;
    public GameObject successFlagPanel;

    [Header("Texts")] //Aqui sao para os textos
    public GameObject textWarning;
    public GameObject textNormal;
    public GameObject textObjective;
    public GameObject textFlag;

    [Header("Interactions")]
    public GameObject flagButton;
    public GameObject promptKey;

    public const string INTRO_KEY = "introductionComplete"; //Transformei em public para o outro script poder ler
    //private bool IsDone = false;

    private int currentStage = 0; //Cada vez que o jogador apertar e tecla E, o numero aumenta e as telas avancam

    private bool canAdvance = true; //Variavel de controle para verificar se o avanco pelo teclado deve estar bloqueado

    void Start()
    {
        //Garante que o botao comece desativado
        if(flagButton != null) flagButton.SetActive(false);
    }

    void Update()
    {
        //So permite avancar com "E" se o painel de sucesso nao estiver ativo
        if(Input.GetKeyDown(KeyCode.E) && !successFlagPanel.activeSelf && canAdvance)
        {
            Advance();
        }
    }

    public void ShowSuccessFlagPanel()
    {
        if(successFlagPanel != null)
        {
            successFlagPanel.SetActive(true); //Mostral o painel da flag capturada
            if(textFlag != null) textFlag.SetActive(false); //Esconde o texto anterior
            if (flagButton != null) flagButton.SetActive(false); //Esconde o botão

            //Salva a flag da introducao
            string newFlag = SafeBase.ViewBase(SafeBase.flag_0);
            FlagManager.Instance.SaveFlag("Introdução", newFlag);
        }
    }

    public void CloseSuccessAndFinish()
    {
        //Salva e descarrega a cena
        PlayerPrefs.SetInt(INTRO_KEY, 1); //Salva que a introducao foi finalizada
        PlayerPrefs.Save(); //Garante a gravacao no disco
        SceneManager.UnloadSceneAsync("Introduction");
    }

    void Advance() //Funçao responsavel por trocas as telas
    {
        currentStage++; //Incrementa o número da etapa

        //Desliga todos os textos antes de mostrar o correto, tambem para evitar que dois textos aparecam ao mesmo tempo
        textWarning.SetActive(false);
        textNormal.SetActive(false);
        textObjective.SetActive(false);
        if(textFlag != null) textFlag.SetActive(false);

        if (flagButton != null) flagButton.SetActive(false); //Desativa o botao por padrao em cada avanco, ele so ligara no case 3

        switch (currentStage) //Analisa o valor da variavel e executa um bloco diferente para cada etapa
        {
            case 1: //Tela inicial
                backgroundWarning.SetActive(false);
                backgroundNormal.SetActive(true);
                textNormal.SetActive(true);
                if(promptKey != null) promptKey.SetActive(true); //Garante que o modal aparece no inicio
                break;

            case 2: //Tela objetivo
                textObjective.SetActive(true);
                break;

            case 3: //Tela final
                textFlag.SetActive(true);
                if(flagButton != null) flagButton.SetActive(true); //Ativa o botao da flag apenas nessa etapa
                if (promptKey != null) promptKey.SetActive(false); //Desativa o modal
                canAdvance = false; //A tecla E nao funciona mais
                break;
            
            case 4: //Se a jogadora apertar "E" na tela da Flag sem clicar no botao, pode ou nao deixar avancar. Apenas seguranca
                CloseSuccessAndFinish();
                break;
            
            default: //Executa apenas se for maior que 3
                break;
        }
    }
}