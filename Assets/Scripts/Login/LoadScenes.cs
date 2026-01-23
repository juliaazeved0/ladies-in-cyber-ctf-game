using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public string sceneName;
    void Start() 
    {
        SceneVerify();
        StartCoroutine(LoadAsync(sceneName));
    }

    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);

        loading.allowSceneActivation = false;
 
        while (loading.isDone == false)
        {
            float loadProgress = loading.progress;
            Debug.Log(loadProgress);

            yield return null;
        }
        
    }

    public void SceneVerify()
    {
        if(PlayerPrefs.HasKey("PLAYER_NAME"))
        {
           sceneName = "PlayerMap";
        }
        else
        {
            sceneName = "LoginScene";
        }
    }
}
