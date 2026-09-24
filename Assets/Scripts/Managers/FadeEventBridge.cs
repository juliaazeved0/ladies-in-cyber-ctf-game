using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Ponte de eventos chamada por animacoes para gerenciar o audio,
/// salvar o ponto de spawn e carregar assincronamente a cena do Boss.
/// </summary>
public class FadeEventBridge : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Clipe de audio da musica que tocara na sala do Boss.")]
    [SerializeField] private AudioClip bossMusic;

    [Header("Boss Room Spawn Configuration")]
    [Tooltip("Coordenadas exatas onde a jogadora aparecera ao carregar a BossRoom.")]
    [SerializeField] private Vector2 bossRoomSpawnPosition = new Vector2(-2.54f, -3.34f);

    /// <summary>
    /// Metodo publico chamado ao termino de um Fade/Animacao.
    /// Altera a musica de fundo, salva a posicao do PlayerPrefs e inicia o carregamento.
    /// </summary>
    public void IniciarCarregamento()
    {
        if(bossMusic != null)
            BackgroundMusic.ChangeMusic(bossMusic);
        else
            Debug.LogWarning($"[FadeEventBridge] 'bossMusic' não foi atribuído no Inspector em {gameObject.name}.", this);

        PlayerPrefs.SetFloat("BossRoom_PlayerX", bossRoomSpawnPosition.x);
        PlayerPrefs.SetFloat("BossRoom_PlayerY", bossRoomSpawnPosition.y);
        PlayerPrefs.SetFloat("BossRoom_PlayerZ", 0f);
        PlayerPrefs.Save();

        StartCoroutine(LoadBossAsync("BossRoom"));
    }

    /// <summary>
    /// Corrotina para carregar a cena de forma assincrona em segundo plano
    /// e ativa-la apenas quando o carregamento atingir o estagio final.
    /// </summary>
    IEnumerator LoadBossAsync(string cenaNome)
    {
        if(string.IsNullOrEmpty(cenaNome))
        {
            Debug.LogError("[FadeEventBridge] O nome da cena para carregamento está vazio ou nulo.", this);
            yield break;
        }

        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaNome, LoadSceneMode.Single);

        if(operacao == null)
        {
            Debug.LogError($"[FadeEventBridge] Não foi possível carregar a cena '{cenaNome}'. Verifique se ela foi adicionada em Build Settings.", this);
            yield break;
        }

        //Impede que a cena apareca imediatamente antes de concluir o carregamento
        operacao.allowSceneActivation = false;

        //Aguarda ate o carregamento atingir 90% (estagio de prontidao da Unity)
        while(operacao.progress < 0.9f) 
        { 
            yield return null; 
        }

        yield return new WaitForSeconds(0.1f);

        //Libera a ativacao da cena
        operacao.allowSceneActivation = true;

        while(!operacao.isDone) 
        { 
            yield return null; 
        }
    }
}