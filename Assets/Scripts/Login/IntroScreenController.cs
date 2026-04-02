using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenController : MonoBehaviour
{
    [Header("Backgrounds")] //Coloca no Inspector os espaços para arrastar os objetos
    public GameObject backgroundWarning; //Referência para o fundo de aviso
    public GameObject backgroundNormal;
    public GameObject successFlagPanel;

    [Header("Texts")] //Aqui são para os textos
    public GameObject textWarning;
    public GameObject textNormal;
    public GameObject textObjective;
    public GameObject textFlag;

    [Header("Interactions")]
    public GameObject flagButton;

    public const string INTRO_KEY = "introductionComplete"; //Transformei em public para o outro script poder ler
    //private bool IsDone = false;

    private int currentStage = 0; //Cada vez que o jogador apertar e tecla E, o número aumenta e as telas avançam

    private bool canAdvance = true; //Variável de controle para verificar se o avanço pelo teclado deve estar bloqueado

    void Start()
    {
        //Garante que o botão comece desativado
        if(flagButton != null) flagButton.SetActive(false);
    }

    void Update()
    {
        //Só permite avançar com "E" se o painel de sucesso não estiver ativo
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

            // Salva a flag da introdução
            string newFlag = SafeBase.ViewBase(SafeBase.flag_0);
            FlagManager.Instance.SaveFlag("Introdução", newFlag);
        }
    }

    public void CloseSuccessAndFinish()
    {
        //Salva e descarrega a cena
        PlayerPrefs.SetInt(INTRO_KEY, 1); //Salva que a introdução foi finalizada
        PlayerPrefs.Save(); //Garante a gravação no disco
        SceneManager.UnloadSceneAsync("Introduction");
    }

    void Advance() //Função responsável por trocas as telas
    {
        currentStage++; //Incrementa o número da etapa

        //Desliga todos os textos antes de mostrar o correto, também para evitar que dois textos apareçam ao mesmo tempo
        textWarning.SetActive(false);
        textNormal.SetActive(false);
        textObjective.SetActive(false);
        if(textFlag != null) textFlag.SetActive(false);

        if (flagButton != null) flagButton.SetActive(false); //Desativa o botão por padrão em cada avanço, ele só ligará no case 3

        switch (currentStage) //Analisa o valor da variável e executa um bloco diferente para cada etapa
        {
            case 1: //Tela inicial
                backgroundWarning.SetActive(false);
                backgroundNormal.SetActive(true);
                textNormal.SetActive(true);
                break;

            case 2: //Tela objetivo
                textObjective.SetActive(true);
                break;

            case 3: //Tela final
                textFlag.SetActive(true);
                if(flagButton != null) flagButton.SetActive(true); //Ativa o botão da flag apenas nessa etapa
                canAdvance = false; //A tecla E não funciona mais
                break;
            
            case 4: //Se a jogadora apertar "E" na tela da Flag sem clicar no botão, pode ou não deixar avançar. Apenas segurança
                CloseSuccessAndFinish();
                break;
            
            default: //Executa apenas se for maior que 3
                break;
        }
    }
}