using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BossTeleportTrigger : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "PlayerMap";
    [SerializeField] private Vector3 teleportPosition = new Vector3(49, -18, 0);

    [Header("Fade Visual")]
    [SerializeField] private GameObject panelBlack;
    private Image fadeImage;

    // 1. ADICIONADO: Espaço para colocar a música do mapa principal
    [Header("Música")]
    [SerializeField] private AudioClip mapMusic; 

    private bool isTransitioning = false;

    private void Awake()
    {
        if (panelBlack != null)
            fadeImage = panelBlack.GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            Debug.Log("Player entrou na área de teleporte! Retornando ao mapa.");
            isTransitioning = true;

            PlayerPrefs.SetFloat(mapSceneName + "_PlayerX", teleportPosition.x);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerY", teleportPosition.y);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerZ", teleportPosition.z);
            PlayerPrefs.SetInt("ReturningFromBoss", 1); // sinaliza que voltou do boss
            PlayerPrefs.Save();

            StartCoroutine(FazerFadeECarregar());
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
            yield return new WaitForSeconds(1f);
        }

        // 2. ADICIONADO: Troca a música de volta para a música do mapa!
        if (mapMusic != null)
        {
            // Substitua 'BackgroundMusic' pelo nome correto do seu gerenciador de áudio, 
            // caso seja diferente do que você usou no UnlockBossRoom.
            BackgroundMusic.ChangeMusic(mapMusic);
        }

        SceneManager.LoadSceneAsync(mapSceneName);
    }
}