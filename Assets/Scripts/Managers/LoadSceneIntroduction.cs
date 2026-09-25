using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Carrega a introducao sem confirmar progresso antes da coleta.</summary>
public class LoadSceneIntroduction : MonoBehaviour
{
    public bool playerIsHere;
    private static bool isLoading;

    public static bool NeedsIntroduction =>
        PlayerPrefs.GetInt(IntroScreenController.INTRO_KEY, 0) != 1 ||
        !FlagManager.HasSavedFlag(SafeBase.ViewBase(SafeBase.flag_0));

    public static void TryLoadIntroduction()
    {
        if(!NeedsIntroduction || isLoading || SceneManager.GetSceneByName("Introduction").isLoaded)
            return;

        isLoading = true;
        var loading = SceneManager.LoadSceneAsync("Introduction", LoadSceneMode.Additive);
        if(loading == null)
        {
            isLoading = false;
            return;
        }
        loading.completed += operation => isLoading = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") || playerIsHere) return;
        playerIsHere = true;
        TryLoadIntroduction();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) playerIsHere = false;
    }
}
