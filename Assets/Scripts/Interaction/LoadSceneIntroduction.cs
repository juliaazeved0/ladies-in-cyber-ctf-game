using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneIntroduction : MonoBehaviour
{   
    public bool playerIsHere = false;
    private const string INTRO_KEY = "introductionComplete";
   

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Confirma se quem bateu foi o Player e se a cena já não está abrindo
        if (collision.CompareTag("Player") && !playerIsHere)
        {
            // 2. Lê a memória: 0 significa que não viu, 1 significa que já viu
            int introDone = PlayerPrefs.GetInt(INTRO_KEY, 0);

            // 3. Se ele ainda NÃO viu a tela (0)
            if (introDone == 0)
            {
                Debug.Log("primeira vez do player na area");
                
                // Salva IMEDIATAMENTE no PlayerPrefs que ele já ativou essa área
                PlayerPrefs.SetInt(INTRO_KEY, 1);
                PlayerPrefs.Save();
                    
                // Carrega a cena de introdução
                SceneManager.LoadSceneAsync("Introduction", LoadSceneMode.Additive);
                
                // Trava para não carregar duas vezes seguidas no mesmo movimento
                playerIsHere = true; 
            }
            else
            {
                // Se o valor for 1, ele ignora silenciosamente e não carrega nada
                Debug.Log("introducao ok");
            }
        }
    }

    // Apenas destrava a variável caso a jogadora saia da área e volte depois
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
}