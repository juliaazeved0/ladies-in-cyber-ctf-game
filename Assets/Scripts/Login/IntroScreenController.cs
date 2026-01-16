using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenController : MonoBehaviour
{
    [Header("Backgrounds")] //Coloca no Inspector os espa�os para arrastar os objetos
    public GameObject backgroundWarning; //Refer�ncia para o fundo de aviso
    public GameObject backgroundNormal; //Fundo nromal

    [Header("Texts")] //Aqui s�o para os textos
    public GameObject textWarning;
    public GameObject textNormal;
    public GameObject textObjective;
    public GameObject textFlag;

    private const string INTRO_KEY = "introductionComplete";
    //private bool IsDone = false;

    private int currentStage = 0; //Cada vez que o jogador apertar e tecla E, o n�mero aumenta e as telas avan�am

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) //Retorna verdadeiro apenas no frame em que a tecla E � pressionada
        {
            Advance();
        }
    }

    void Advance() //Fun��o respons�vel por trocas as telas
    {
        currentStage++; //Incrementa o n�mero da etapa

        //Desliga todos os textos antes de mostrar o correto, tamb�m para evitar que dois textos apare�am ao mesmo tempo
        textWarning.SetActive(false);
        textNormal.SetActive(false);
        textObjective.SetActive(false);

        switch (currentStage) //Analisa o valor da vari�vel e executa um bloco diferente para cada etapa
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
                break;
            
            case 4: // descarrega introdução e volta para o mapa
            
                SceneManager.UnloadSceneAsync("Introduction");
                break;
                
            
            default: //Executa apenas se for maior que 3
                break;
        }
    }
}
