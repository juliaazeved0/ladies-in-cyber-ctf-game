using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeEventBridge : MonoBehaviour
{
    [SerializeField] private AudioClip bossMusic;

    public void IniciarCarregamento()
    {
        // Troca a música antes de carregar a cena.
        // O fade de áudio (1s) roda em paralelo com o carregamento assíncrono.
        if (bossMusic != null)
            BackgroundMusic.ChangeMusic(bossMusic);
        else
            Debug.LogWarning("[FadeEventBridge] bossMusic não atribuído no Inspector — música não será trocada.");

        StartCoroutine(LoadBossAsync("BossRoom"));
    }

    IEnumerator LoadBossAsync(string cenaNome)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaNome, LoadSceneMode.Single);

        operacao.allowSceneActivation = false;

        while (operacao.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        operacao.allowSceneActivation = true;

        while (!operacao.isDone)
        {
            yield return null;
        }
    }
}
