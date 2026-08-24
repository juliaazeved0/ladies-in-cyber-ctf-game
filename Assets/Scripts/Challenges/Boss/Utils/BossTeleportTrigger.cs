using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Detecta quando a jogadora entra na area de teleporte de retorno do boss,
/// salva a posicao de spawn no mapa, realiza um fade visual e troca a 
/// musica antes de carregar a cena do mapa novamente.
/// </summary>
public class BossTeleportTrigger : MonoBehaviour
{
    [Header("Navigation")]
    [Tooltip("Nome da cena do mapa para onde a jogadora sera teleportada.")]
    [SerializeField] private string mapSceneName = "PlayerMap";

    [Tooltip("Posicao em que a jogadora deve aparecer ao retornar ao mapa.")]
    [SerializeField] private Vector3 teleportPosition = new Vector3(49, -18, 0);

    [Header("Fade Visual")]
    [Tooltip("Painel preto usado para o efeito de fade antes de trocar de cena.")]
    [SerializeField] private GameObject panelBlack;
    private Image fadeImage; //Componente Image do panelBlack

    [Header("Music")]
    [Tooltip("Musica a ser tocada assim que a cena do mapa for carregada.")]
    [SerializeField] private AudioClip mapMusic; 

    //Evita que o trigger seja acionado multiplas vezes caso a jogadora permaneca dentro da area
    private bool isTransitioning = false;

    private void Awake()
    {
        if(panelBlack != null)
            fadeImage = panelBlack.GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isTransitioning)
        {
            Debug.Log("Player entrou na área de teleporte! Retornando ao mapa.");
            isTransitioning = true;

            //Salva a posicao de spawn especifica para a cena do mapa
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerX", teleportPosition.x);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerY", teleportPosition.y);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerZ", teleportPosition.z);

            //Flag para que o script do mapa saiba que a jogadora esta retornando do boss
            PlayerPrefs.SetInt("ReturningFromBoss", 1);
            PlayerPrefs.Save();

            StartCoroutine(FadeAndLoad());
        }
    }

    /// <summary>
    /// Executa o fade visual (se configurado), troca a musica de fundo
    /// e carrega a cena do mapa de forma assincrona.
    /// </summary>
    private IEnumerator FadeAndLoad()
    {
        //Verifica se a cena existe e esta registrada no Build Settings antes de iniciar o fade
        if(!Application.CanStreamedLevelBeLoaded(mapSceneName))
        {
            Debug.LogError($"A cena '{mapSceneName}' não existe ou não está no Build Settings!");
            isTransitioning = false;
            yield break;
        }

        if(panelBlack != null && fadeImage != null)
        {
            //Desativa qualquer Animator para nao conflitar com o controle manual do alpha
            Animator anim = panelBlack.GetComponent<Animator>();
            if(anim != null) anim.enabled = false;

            panelBlack.SetActive(true);

            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;

            float time = 0f;
            float fadeDuration = 0.5f;

            while(time < fadeDuration)
            {
                time += Time.deltaTime;
                color.a = Mathf.Clamp01(time / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }
        else
        {
            //Sem painel de fade configurado, apenas espera um tempo fixo antes de trocar de cena
            yield return new WaitForSeconds(0.5f);
        }

        if(mapMusic != null)
        {
            BackgroundMusic.ChangeMusic(mapMusic);
        }

        SceneManager.LoadSceneAsync(mapSceneName);
    }
}