using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia o carregamento aditivo da cena de introducao.
/// Garante que a cena seja carregada apenas na primeira vez que a jogadora entra no gatilho.
/// </summary>
public class LoadSceneIntroduction : MonoBehaviour
{   
    public bool playerIsHere = false;
    private const string INTRO_KEY = "introductionComplete";

    /// <summary>
    /// Detecta a entrada fisica da jogadora na area e dispara o carregamento aditivo se for a primeira vez.
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !playerIsHere)
        {
            int introDone = PlayerPrefs.GetInt(INTRO_KEY, 0);

            if(introDone == 0)
            {
                Debug.Log("Primeira vez da player na area.");
                
                PlayerPrefs.SetInt(INTRO_KEY, 1);
                PlayerPrefs.Save();
                    
                SceneManager.LoadSceneAsync("Introduction", LoadSceneMode.Additive);
                
                playerIsHere = true; 
            }
            else
            {
                Debug.Log("Introducao ja concluida anteriormente.");
            }
        }
    }

    /// <summary>
    /// Detecta a saida da jogadora da area do colisor e redefine os sinalizadores de presenca.
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision)
    {
        //Destrava a variavel caso a jogadora saia da area
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
}