using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeEventBridge : MonoBehaviour
{
    public void IniciarCarregamento()
    {
        StartCoroutine(LoadBossAsync("BossRoom"));
    }

    IEnumerator LoadBossAsync(string cenaNome)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaNome, LoadSceneMode.Single);
        
        operacao.allowSceneActivation = false;

        while (operacao.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        operacao.allowSceneActivation = true;

        while (!operacao.isDone)
        {
            yield return null;
        }
    }
}