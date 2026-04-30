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

    [Tooltip("Coordenadas de spawn quando o jogador entra no PlayerMap.")]
    private Vector2 mapSpawnPosition = new Vector2(-38.12f, -36.71f);

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

        if(!string.IsNullOrEmpty(playerName))
        {
            targetScene = "PlayerMap";

            PlayerPrefs.SetFloat("PlayerMap_PlayerX", mapSpawnPosition.x);
            PlayerPrefs.SetFloat("PlayerMap_PlayerY", mapSpawnPosition.y);
            PlayerPrefs.SetFloat("PlayerMap_PlayerZ", 0f);
            
            PlayerPrefs.Save();
        }
        else
        {
            targetScene = "LoginScene";
        }
    }

    /// <summary>
    /// Executa o carregamento assincrono da cena com um delay minimo visual.
    /// </summary>
    /// <param name="sceneName">Nome da cena</param>
    /// <returns>IEnumerator para controle da Corrotina.</returns>
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

            //0.9f eh o limite de progresso quando allowSceneActivation eh falso
            if(loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
            {
                loading.allowSceneActivation = true;
            }

            yield return null; 
        }
    }
}