using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsável por gerenciar o fluxo de carregamento entre cenas,
/// garantindo a persistencia de posicao e tempo minimo de loading.
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
        SceneVerify();
        StartCoroutine(LoadAsync(targetScene));
    }

    /// <summary>
    /// Verifica o estado da jogadora e define qual cena carregar.
    /// Caso logado, reseta a posicao no PlayerPrefs para o spawn fixo.
    /// </summary>
    public void SceneVerify()
    {
        string playerName = PlayerPrefs.GetString("PLAYER_NAME", "");

        //Se o nome da jogadora existir, significa que a sessao esta ativa
        if(!string.IsNullOrEmpty(playerName))
        {
            targetScene = "PlayerMap";

            //So define o spawn padrao se ainda nao ha posicao salva
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
            //Se nao houver dados de login, redireciona para a tela de autenticacao
            targetScene = "LoginScene";
        }
    }

    /// <summary>
    /// Executa o carregamento assincrono da cena com um delay minimo visual.
    /// </summary>
    private IEnumerator LoadAsync(string sceneName)
    {
        float startTime = Time.time;
        float minimumLoadTime = 3f;

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);

        //Bloqueia a entrada automatica na cena assim que o carregamento termina
        loading.allowSceneActivation = false;

        while(!loading.isDone)
        {
            float timeElapsed = Time.time - startTime;

            //A cena so eh liberada se o carregamento interno terminou E o tempo minimo de splash passou
            if (loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
                loading.allowSceneActivation = true;

            yield return null;
        }
    }
}