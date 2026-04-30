using UnityEngine;
using System.Collections;

/// <summary>
/// Gerenciador global de musica de fundo que persiste entre cenas.
/// Implementa o padrao Singleton para garantir que apenas uma instancia exista.
/// </summary>
public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            audioSource = GetComponent<AudioSource>();

            if(audioSource != null)
            {
                audioSource.loop = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Troca a trilha sonora atual por uma nova, aplicando um efeito de fade.
    /// </summary>
    /// <param name="newClip">O novo arquivo de audio (AudioClip) a ser reproduzido.</param>
    /// <param name="fadeDuration">Duracao do efeito de transicao em segundos.</param>
    public static void ChangeMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if(instance != null && instance.audioSource != null && newClip != null)
        {
            instance.StartCoroutine(instance.FadeMusic(newClip, fadeDuration));
        }
    }

    /// <summary>
    /// Controla a interpolacao do volume para realizar o crossfade entre clipes.
    /// </summary>
    private IEnumerator FadeMusic(AudioClip newClip, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        //Fade out
        for(float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;

        //Troca o arquivo
        audioSource.clip = newClip;
        audioSource.Play();

        //Fade in
        for(float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}