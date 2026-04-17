using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeEventBridge : MonoBehaviour
{
    [SerializeField] private AudioClip bossMusic;

    [Header("Posição de spawn na BossRoom")]
    [SerializeField] private Vector2 bossRoomSpawnPosition = new Vector2(-2.54f, -3.34f);

    public void IniciarCarregamento()
    {
        if (bossMusic != null)
            BackgroundMusic.ChangeMusic(bossMusic);
        else
            Debug.LogWarning("[FadeEventBridge] bossMusic não atribuído no Inspector.");

        // Reseta o spawn antes de carregar para que DataPlayerPosition.LoadGame()
        // use a posição correta ao entrar na BossRoom.
        PlayerPrefs.SetFloat("BossRoom_PlayerX", bossRoomSpawnPosition.x);
        PlayerPrefs.SetFloat("BossRoom_PlayerY", bossRoomSpawnPosition.y);
        PlayerPrefs.SetFloat("BossRoom_PlayerZ", 0f);
        PlayerPrefs.Save();

        StartCoroutine(LoadBossAsync("BossRoom"));
    }

    IEnumerator LoadBossAsync(string cenaNome)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaNome, LoadSceneMode.Single);
        operacao.allowSceneActivation = false;
        while (operacao.progress < 0.9f) { yield return null; }
        yield return new WaitForSeconds(0.1f);
        operacao.allowSceneActivation = true;
        while (!operacao.isDone) { yield return null; }
    }
}
