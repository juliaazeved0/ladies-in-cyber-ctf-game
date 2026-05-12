using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Ponte de eventos para transição de cena (Fade).
/// Gerencia a troca de musica e a persistencia da posicao de spawn para a BossRoom.
/// </summary>
public class FadeEventBridge : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip bossMusic;

    [Header("Boss Room Spawn Configuration")]
    [Tooltip("Coordenadas exatas onde a jogadora aparecera ao carregar a BossRoom.")]
    [SerializeField] private Vector2 bossRoomSpawnPosition = new Vector2(-2.54f, -3.34f);

    /// <summary>
    /// Inicia o processo de transicao, configurando musica e dados de salvamento.
    /// </summary>
    public void IniciarCarregamento()
    {
        //Gerencia a troca de audio global
        if(bossMusic != null)
            BackgroundMusic.ChangeMusic(bossMusic);
        else
            Debug.LogWarning("[FadeEventBridge] bossMusic nao atribuido no Inspector.");

        //Prepara os PlayerPrefs para que o sistma de carregamento de posicao identifique onde colocar a jogadora na nova cena
        PlayerPrefs.SetFloat("BossRoom_PlayerX", bossRoomSpawnPosition.x);
        PlayerPrefs.SetFloat("BossRoom_PlayerY", bossRoomSpawnPosition.y);
        PlayerPrefs.SetFloat("BossRoom_PlayerZ", 0f);
        PlayerPrefs.Save();

        //Inicia o carregamento assincrono para evitar travamentos na UI
        StartCoroutine(LoadBossAsync("BossRoom"));
    }

    /// <summary>
    /// Corrotina para carregar a cena em segundo plano.
    /// </summary>
    IEnumerator LoadBossAsync(string cenaNome)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaNome, LoadSceneMode.Single);

        //Impede a ativacao imediata para garantir que o Fade termine ou outros processos concluam
        operacao.allowSceneActivation = false;

        //Aguarda ate que cena esteja 90% carregada
        while(operacao.progress < 0.9f) 
        { 
            yield return null; 
        }

        //Pequeno dealey de seguranca
        yield return new WaitForSeconds(0.1f);

        //Ativa a cena carregada
        operacao.allowSceneActivation = true;

        while(!operacao.isDone) 
        { 
            yield return null; 
        }
    }
}