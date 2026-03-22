using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenController : MonoBehaviour
{
    [Header("Backgrounds")] //Coloca no Inspector os espaços para arrastar os objetos
    public GameObject backgroundWarning; //Referência para o fundo de aviso
    public GameObject backgroundNormal; //Fundo nromal

    [Header("Texts")] //Aqui são para os textos
    public GameObject textWarning;
    public GameObject textNormal;
    public GameObject textObjective;
    public GameObject textFlag;

    public const string INTRO_KEY = "introductionComplete"; // Transformei em public para o outro script poder ler
    //private bool IsDone = false;

    private int currentStage = 0; //Cada vez que o jogador apertar e tecla E, o número aumenta e as telas avançam

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) //Retorna verdadeiro apenas no frame em que a tecla E é pressionada
        {
            Advance();
        }
    }

    void Advance() //Função responsável por trocas as telas
    {
        currentStage++; //Incrementa o número da etapa

        //Desliga todos os textos antes de mostrar o correto, também para evitar que dois textos apareçam ao mesmo tempo
        textWarning.SetActive(false);
        textNormal.SetActive(false);
        textObjective.SetActive(false);

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
                break;
            
            case 4: // descarrega introdução e volta para o mapa
            
                // === AQUI ESTÁ A CORREÇÃO! ===
                PlayerPrefs.SetInt(INTRO_KEY, 1); // Salva que a introdução foi finalizada
                PlayerPrefs.Save();               // Garante a gravação no disco
                // =============================
            
                SceneManager.UnloadSceneAsync("Introduction");
                break;
                
            
            default: //Executa apenas se for maior que 3
                break;
        }
    }
}