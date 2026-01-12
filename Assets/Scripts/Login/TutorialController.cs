using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    //Referências aos painéis do tutorial
    public GameObject panel1;
    public GameObject panel2;

    public string nextSceneName = "Introduction"; //Nome da cena que será carregada

    private int count = 0; //Variável para guardar o estado do tutorial
    public void OnNextClicked() //Função é chamada quando o jogador clica na flecha
    {
        if(count == 0) //Se for o primeiro clique
        {
            panel1.SetActive(false); //Esconde o painel 1
            panel2.SetActive(true); //Mostra o painel 2
            count = 1; //Atualiza o contador para ir a segunda etapa, ou seja, segunda cena
        }
        else if(count == 1) //Se for o segundo clique
        {
            SceneManager.LoadScene(nextSceneName); //Carrega a próxima cena
        }
    }
}