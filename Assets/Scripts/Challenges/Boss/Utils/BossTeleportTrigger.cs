using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Detecta a chegada da jogadora na area de retorno do Boss e a teleporta de volta
/// para a cena do mapa, salvando a posicao de destino via PlayerPrefs, aplicando
/// um fade visual suave e trocando a musica de fundo.
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

    private Image fadeImage;

    [Header("Music")]
    [Tooltip("Musica a ser tocada assim que a cena do mapa for carregada.")]
    [SerializeField] private AudioClip mapMusic; 

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

            /*A posicao de destino eh salva via PlayerPrefs porque essa cena sera descarregada
            antes da cena do mapa ler esse valor no Awake dela*/
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerX", teleportPosition.x);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerY", teleportPosition.y);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerZ", teleportPosition.z);

            PlayerPrefs.SetInt("ReturningFromBoss", 1);
            PlayerPrefs.Save();

            StartCoroutine(FadeAndLoad());
        }
    }

    private IEnumerator FadeAndLoad()
    {
        if(!Application.CanStreamedLevelBeLoaded(mapSceneName))
        {
            Debug.LogError($"A cena '{mapSceneName}' não existe ou não está no Build Settings!");
            isTransitioning = false;
            yield break;
        }

        if(panelBlack != null && fadeImage != null)
        {
            /*Desativa qualquer Animator no painel de fade para evitar que uma animacao
            propria dele interfira no fade controlado manualmente por este script*/
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
            yield return new WaitForSeconds(0.5f);
        }

        if(mapMusic != null)
        {
            BackgroundMusic.ChangeMusic(mapMusic);
        }

        SceneManager.LoadSceneAsync(mapSceneName);
    }
}