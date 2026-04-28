using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    private string targetScene;

    private Vector2 mapSpawnPosition = new Vector2(-38.12f, -36.71f);

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

            // Só define o spawn padrão se ainda não há posição salva
            if (!PlayerPrefs.HasKey("PlayerMap_PlayerX"))
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

    private IEnumerator LoadAsync(string sceneName)
    {
        float startTime = Time.time;
        float minimumLoadTime = 3f;

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        loading.allowSceneActivation = false;

        while (!loading.isDone)
        {
            float timeElapsed = Time.time - startTime;

            if (loading.progress >= 0.9f && timeElapsed >= minimumLoadTime)
                loading.allowSceneActivation = true;

            yield return null;
        }
    }
}