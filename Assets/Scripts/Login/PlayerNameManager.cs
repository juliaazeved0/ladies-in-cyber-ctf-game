using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    public void OnClickPlay()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Gameplay");
    }
}
