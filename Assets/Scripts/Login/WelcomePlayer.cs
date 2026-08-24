using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Mostra uma mensagem de boas-vindas personalizada com o nome da jogadora
/// (se disponivel em PlayerPrefs) e permite avancar para a cena do mapa.
/// </summary>
public class WelcomePlayer : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("Texto da UI que mostrara a mensagem de boas-vindas.")]
    [SerializeField] private TMP_Text welcomeText;

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Start()
    {
        //Evita erro caso a referencia nao tenha sido arrastada no Inspector
        if(welcomeText == null)
        {
            Debug.LogError($"{gameObject.name} está sem referência ao TMP_Text de boas-vindas!");
            return;
        }

        //Se houver um nome salvo, personaliza a manegsame; caso contrario, mostra
        //uma versao generica de boas-vindas
        if(PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY);
            welcomeText.text = $"BEM-VINDA {playerName.ToUpper()} AO CTF LADIES IN CYBER!";
        }
        else
        {
            welcomeText.text = $"BEM-VINDA AO CTF LADIES IN CYBER!";
        }
    }

    //Chamado pelo botao correspondente na UI para avancar ate o mapa
    public void GoToPlayerMap()
    {
        SceneManager.LoadScene("PlayerMap");
    }
}