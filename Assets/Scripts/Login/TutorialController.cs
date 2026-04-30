using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gerencia a navegacao entre as telas do tutorial inicia.
/// Responsavel por alternar a visibilidade dos paineis e carregar a cena do jogo.
/// </summary>
public class TutorialController : MonoBehaviour
{
<<<<<<< HEAD
    [Header("Painéis")]
    public GameObject panel1;
    public GameObject panel2;

    public string nextSceneName = "PlayerMap";

    [Header("Fade Visual")]
    [SerializeField] private GameObject panelBlack;
    private Image fadeImage;

    private int count = 0;

=======
    [Header("Configuracoes dos Paineis")]
    [SerializeField] public GameObject panel1;
    [SerializeField] public GameObject panel2;

    [Header("Configuracoes de Fluxo")]
    [SerializeField] public string nextSceneName = "PlayerMap";

    private int count = 0; //Estado atual do tutorial (0 = Tela 1, 1 = Tela 2)

    /// <summary>
    /// Configura o estado inicial do tutorial ao carregar a cena.
    /// </summary>
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d
    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
<<<<<<< HEAD

        if (panelBlack != null)
            fadeImage = panelBlack.GetComponent<Image>();
    }

    public void OnNextClicked()
    {
        if (count == 0)
=======
    }

    /// <summary>
    /// Avanca para a proxima tela do tutorial.
    /// </summary>
    public void OnNextClicked()
    {
        if(count == 0)
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            count = 1;
        }
<<<<<<< HEAD
        else if (count == 1)
        {
            StartCoroutine(FazerFadeECarregar());
        }
    }

    public void OnBackClicked()
    {
        if (count == 1)
=======
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
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d
        {
            panel2.SetActive(false);
            panel1.SetActive(true);
            count = 0;
        }
    }

    private IEnumerator FazerFadeECarregar()
    {
        if (panelBlack != null && fadeImage != null)
        {
            Animator anim = panelBlack.GetComponent<Animator>();
            if (anim != null) anim.enabled = false;

            panelBlack.SetActive(true);
            Color cor = fadeImage.color;
            cor.a = 0f;
            fadeImage.color = cor;

            float tempo = 0f;
            float duracaoFade = 1f;

            while (tempo < duracaoFade)
            {
                tempo += Time.deltaTime;
                cor.a = Mathf.Clamp01(tempo / duracaoFade);
                fadeImage.color = cor;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}