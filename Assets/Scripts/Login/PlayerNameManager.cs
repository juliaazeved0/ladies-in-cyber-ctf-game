using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Start()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            SceneManager.LoadScene("IntroductionGame");
        }

    }

    public void OnClickPlay()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("IntroductionGame");
    }
}
