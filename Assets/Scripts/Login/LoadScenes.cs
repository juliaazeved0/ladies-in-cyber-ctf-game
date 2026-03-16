using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    private string targetScene;

    void Start()
    {
        SceneVerify();
        StartCoroutine(LoadAsync(targetScene));
    }

    public void SceneVerify()
    {
        string playerName = PlayerPrefs.GetString("PLAYER_NAME", "");

        if (!string.IsNullOrEmpty(playerName))
        {
            targetScene = "PlayerMap";
        }
        else
        {
            targetScene = "LoginScene";
        }
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        // Marca o tempo exato em que a tela de loading começou
        float startTime = Time.time;
        float minimumLoadTime = 3f; // Tempo mínimo em segundos

        // Começa a carregar a próxima cena silenciosamente
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        loading.allowSceneActivation = false;

        while (!loading.isDone)
        {
            // Calcula quanto tempo já passou desde que o Start() rodou
            float timeElapsed = Time.time - startTime;

            // Se o Unity terminou de carregar (0.9f) E já se passaram 3 segundos...
            if (loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
            {
                // Libera a cena nova para aparecer na tela
                loading.allowSceneActivation = true;
            }

            yield return null; 
        }
    }
}