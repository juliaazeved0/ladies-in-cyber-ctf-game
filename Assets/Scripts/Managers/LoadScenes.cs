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

<<<<<<< HEAD:Assets/Scripts/Login/LoadScenes.cs
=======
    [Tooltip("Coordenadas de spawn quando o jogador entra no PlayerMap.")]
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d:Assets/Scripts/Managers/LoadScenes.cs
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

<<<<<<< HEAD:Assets/Scripts/Login/LoadScenes.cs
            // Só define o spawn padrão se ainda não há posição salva
            if (!PlayerPrefs.HasKey("PlayerMap_PlayerX"))
            {
                PlayerPrefs.SetFloat("PlayerMap_PlayerX", mapSpawnPosition.x);
                PlayerPrefs.SetFloat("PlayerMap_PlayerY", mapSpawnPosition.y);
                PlayerPrefs.SetFloat("PlayerMap_PlayerZ", 0f);
                PlayerPrefs.Save();
            }
=======
            PlayerPrefs.SetFloat("PlayerMap_PlayerX", mapSpawnPosition.x);
            PlayerPrefs.SetFloat("PlayerMap_PlayerY", mapSpawnPosition.y);
            PlayerPrefs.SetFloat("PlayerMap_PlayerZ", 0f);
            
            PlayerPrefs.Save();
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d:Assets/Scripts/Managers/LoadScenes.cs
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

<<<<<<< HEAD:Assets/Scripts/Login/LoadScenes.cs
            if (loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
=======
            //0.9f eh o limite de progresso quando allowSceneActivation eh falso
            if(loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
            {
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d:Assets/Scripts/Managers/LoadScenes.cs
                loading.allowSceneActivation = true;

            yield return null;
        }
    }
}