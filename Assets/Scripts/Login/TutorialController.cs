using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    //Referências dos painéis
    public GameObject panel1;
    public GameObject panel2;

    public string nextSceneName = "PlayerMap"; //Nome da próxima cena

    private int count = 0; //Indica em qual tela o usuário está

    void Start() //QUando a cena começa
    {
        panel1.SetActive(true); //Painel 1 aparece
        panel2.SetActive(false); //Painel 2 fica escondido
    }
    public void OnNextClicked() //Botão de avançar
    {
        if(count == 0) //Primeira vez que o botão é clicado
        {
            panel1.SetActive(false); //Esconde a tela 1
            panel2.SetActive(true); //Mostra a tela 2
            count = 1; //Agora vai para a tela 2
        }
        else if(count == 1) //Segunda vez que o botão é clicado
        {
            SceneManager.LoadScene(nextSceneName); //Sai do tutorial e vai para outra cena
        }
    }

    public void OnBackClicked() //Botão de voltar
    {
        if(count == 1) //Se o usuário estiver na tela 2
        {
            panel2.SetActive(false); //Esconde a tela 2
            panel1.SetActive(true); //Mostra a tela 1
            count = 0; //Atualiza o estado da variável, ou seja, volta para o início do tutorial
        }
    }
}