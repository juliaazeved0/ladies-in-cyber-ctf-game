using UnityEngine;
using System.Collections;

/// <summary>
/// Gerencia a musica de fundo do jogo com um Singleton persistente entre cenas.
/// Permite trocar a faixa atual com um efeito de fade-out/fade-in suave, em vez
/// de cortar o audio abruptamente.
/// </summary>
public class BackgroundMusic : MonoBehaviour
{
    //Referencia estatica unica, garantindo que so exista uma instancia deste
    //objeto ao longo de todo o jogo (padrao Singleton)
    private static BackgroundMusic instance;

    [Header("Audio Components")]
    [Tooltip("AUdioSource responsavel por tocar a musica de fundo.")]
    private AudioSource audioSource;

    void Awake()
    {
        if(instance == null)
        {
            //Primeira instancia criada: torna-se a instancia oficial e sobrevive
            //a troca de cenas
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
            //Ja existe uma instancia ativa, destroi essa duplicada para nao tocar
            //duas musicas ao mesmo tempo
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Ponto de entrada publico e estatico para trocar a musica atual.
    /// </summary>
    /// <param name="newClip">Novo clipe de audio a ser tocado.</param>
    /// <param name="fadeDuration">Duracao em segundos de cada etapa do fade.</param>
    public static void ChangeMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        if(instance != null && instance.audioSource != null && newClip != null)
        {
            instance.StartCoroutine(instance.FadeMusic(newClip, fadeDuration));
        }
    }

    /// <summary>
    /// Executa a transicao entre musicas em duas etapas.
    /// </summary>
    private IEnumerator FadeMusic(AudioClip newClip, float fadeDuration)
    {
        //Protege o metodo caso ele seja chamado de outro lugar
        if(audioSource == null || newClip == null)
        {
            yield break;
        }

        float startVolume = audioSource.volume;

        //Fade-out: diminui o volume da musica atual ate 0
        for(float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;

        //Troca o clique ja com o volume zerado, evitando um "corte" audivel
        audioSource.clip = newClip;
        audioSource.Play();

        //Fade-in: sobe o volume gradualmente do zero ate o volume original
        for(float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}