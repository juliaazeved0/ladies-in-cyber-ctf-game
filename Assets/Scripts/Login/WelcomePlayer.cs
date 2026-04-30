using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Mostra uma mensagem de boas-vindas personalizada na interface recuperando o nome da jogadora salvo.
/// </summary>
public class WelcomePlayer : MonoBehaviour
{
    [Tooltip("Texto da UI que mostrara a mensagem de boas-vindas.")]
    [SerializeField] private TMP_Text welcomeText;

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    /// <summary>
    /// Recupera o nome da jogadora e atualiza o texto da UI.
    /// </summary>
    void Start()
    {
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

    /// <summary>
    /// Navega para a cena do mapa do jogo.
    /// </summary>
    public void GoToPlayerMap()
    {
        SceneManager.LoadScene("PlayerMap");
    }
}