using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Decide qual cena carregar com base no progresso salvo da jogadora
/// (login feito ou nao) e realiza o carregamento assincrono com um
/// tempo minimo de exibicao da tela de loading.
/// </summary>
public class LoadScenes : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Nome da cena que sera carregada.")]
    private string targetScene;

    [Header("Spawn Configuration")]
    [Tooltip("Coordenadas padrao de spawn no mapa caso a jogadora nao tenha dados salvos.")]
    [SerializeField] private Vector2 mapSpawnPosition = new Vector2(-38.12f, -36.71f);

    void Start()
    {
        //Define qual cena carregar (login ou mapa) e prepara o spawn, se necessario
        SceneVerify();

        //So entao inicia o carregamento assincrono da cena ja decidida
        StartCoroutine(LoadAsync(targetScene));
    }

    /// <summary>
    /// Verifica se existe um nome de jogadora salvo em PlayerPrefs.
    /// Se existir, direciona para a cena do mapa e garante uma posicao
    /// de spawn padrao. Caso contrario, direciona para a tela de login.
    /// </summary>
    public void SceneVerify()
    {
        string playerName = PlayerPrefs.GetString("PLAYER_NAME", "");

        if(!string.IsNullOrEmpty(playerName))
        {
            targetScene = "PlayerMap";

            //So grava a posicao padrao se ainda nao houver posicao salva, evitando sobrescrever
            if(!PlayerPrefs.HasKey("PlayerMap_PlayerX"))
            {
                PlayerPrefs.SetFloat("PlayerMap_PlayerX", mapSpawnPosition.x);
                PlayerPrefs.SetFloat("PlayerMap_PlayerY", mapSpawnPosition.y);
                PlayerPrefs.SetFloat("PlayerMap_PlayerZ", 0f);
                PlayerPrefs.Save();
            }
        }
        else
        {
            targetScene = "LoginScene";
        }
    }

    /// <summary>
    /// Carrega a cena de forma assincrona, mas segura a ativacao ate que um
    /// tempo minimo tenha passado, evitando que a tela de loading "pisque"
    /// rapido demais quando a cena carrega quase instantaneamente.
    /// </summary>
    private IEnumerator LoadAsync(string sceneName)
    {
        //Verifica se a cena existe e esta registrada no Build Settings
        if(!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"A cena '{sceneName}' não existe ou não está no Build Settings!");
            yield break;
        }

        float startTime = Time.time;
        float minimumLoadTime = 3f;

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);

        //Trava a troca de cena em 90% ate liberarmos manualmente abaixo
        loading.allowSceneActivation = false;

        while(!loading.isDone)
        {
            float timeElapsed = Time.time - startTime;

            if(loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
                loading.allowSceneActivation = true;

            yield return null;
        }
    }
}