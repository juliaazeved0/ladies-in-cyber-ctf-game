using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
     
            audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.loop = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para trocar a música com fade
    public static void ChangeMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if (instance != null && instance.audioSource != null && newClip != null)
        {
            instance.StartCoroutine(instance.FadeMusic(newClip, fadeDuration));
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip, float fadeDuration)
    {
        // Fade out da música atual
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;

        // Troca o clip e inicia
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in da nova música
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}