using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    [Header("Tempo para clarear")]
    public float duracaoFade = 0.5f;
    
    private Image fadeImage;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        
        if (fadeImage != null)
        {
            // 1. Desliga qualquer Animator para ele não atrapalhar
            Animator anim = GetComponent<Animator>();
            if (anim != null) anim.enabled = false;

            // 2. Força o painel a começar 100% preto logo no primeiro frame
            Color cor = fadeImage.color;
            cor.a = 1f;
            fadeImage.color = cor;

            // 3. Inicia o efeito de clarear
            StartCoroutine(FazerFadeIn());
        }
    }

    private IEnumerator FazerFadeIn()
    {
        float tempo = 0f;
        Color cor = fadeImage.color;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            cor.a = 1f - (tempo / duracaoFade); // Vai de 1 (preto) a 0 (transparente)
            fadeImage.color = cor;
            yield return null; // Espera o próximo frame
        }

        // MUITO IMPORTANTE: Desativa o painel no final para ele não ficar invisível
        // na frente da tela bloqueando os cliques do mouse nos seus botões!
        gameObject.SetActive(false);
    }
}