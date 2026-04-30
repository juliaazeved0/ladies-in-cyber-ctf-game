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
    [Header("Painéis")]
    public GameObject panel1;
    public GameObject panel2;

    public string nextSceneName = "PlayerMap";

    [Header("Fade Visual")]
    [SerializeField] private GameObject panelBlack;
    private Image fadeImage;

    private int count = 0;

    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);

        if (panelBlack != null)
            fadeImage = panelBlack.GetComponent<Image>();
    }

    public void OnNextClicked()
    {
        if (count == 0)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            count = 1;
        }
        else if (count == 1)
        {
            StartCoroutine(FazerFadeECarregar());
        }
    }

    public void OnBackClicked()
    {
        if (count == 1)
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