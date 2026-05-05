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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Confirma se quem bateu foi a player e se a cena ja nao esta abrindo
        if(collision.CompareTag("Player") && !playerIsHere)
        {
            //Le a memoria: 0 significa que nao viu, 1 significa que ja viu
            int introDone = PlayerPrefs.GetInt(INTRO_KEY, 0);

            //Se ele ainda NAO viu a tela (0)
            if(introDone == 0)
            {
                Debug.Log("Primeira vez da player na area.");
                
                //Salva IMEDIATAMENTE no PlayerPrefs que ele ja ativou essa area
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

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Destrava a variavel caso a jogadora saia da area
        if(collision.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
}