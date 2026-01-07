using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WelcomePlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text welcomeText;

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Start()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY);
            welcomeText.text = $"Bem-vinda, {playerName.ToUpper()}!";
        }
        else
        {
            welcomeText.text = "Bem-vinda!";
        }
    }

    public void GoToPlayerMap()
    {
        SceneManager.LoadScene("PlayerMap");
    }
}
