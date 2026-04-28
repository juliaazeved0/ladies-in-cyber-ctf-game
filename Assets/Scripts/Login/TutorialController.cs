using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia a navegacao entre as telas do tutorial inicia.
/// Responsavel por alternar a visibilidade dos paineis e carregar a cena do jogo.
/// </summary>
public class TutorialController : MonoBehaviour
{
    [Header("Configuracoes dos Paineis")]
    [SerializeField] public GameObject panel1;
    [SerializeField] public GameObject panel2;

    [Header("Configuracoes de Fluxo")]
    [SerializeField] public string nextSceneName = "PlayerMap";

    private int count = 0; //Estado atual do tutorial (0 = Tela 1, 1 = Tela 2)

    /// <summary>
    /// Configura o estado inicial do tutorial ao carregar a cena.
    /// </summary>
    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    /// <summary>
    /// Avanca para a proxima tela do tutorial.
    /// </summary>
    public void OnNextClicked()
    {
        if(count == 0)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            count = 1;
        }
        else if(count == 1)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    /// <summary>
    /// Retorna para a tela anterior do tutorial.
    /// </summary>
    public void OnBackClicked()
    {
        if(count == 1)
        {
            panel2.SetActive(false);
            panel1.SetActive(true);
            count = 0;
        }
    }
}