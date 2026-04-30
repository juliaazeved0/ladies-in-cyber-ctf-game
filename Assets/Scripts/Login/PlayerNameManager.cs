using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Gerencia a coleta do nome da jogadora na tela de Login e salva localmente.
/// </summary>
public class PlayerNameManager : MonoBehaviour
{
    [Tooltip("Campo de texto onde a jogadora digita o nome.")]
    [SerializeField] private TMP_InputField nameInput;

    //Chave usada para identificar o nome no PlayerPrefs
    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    /// <summary>
    /// Valida o nome digitado, salva no sistema e carrega a cena de Gameplay.
    /// </summary>
    public void OnClickPlay()
    {
        string playerName = nameInput.text.Trim();

        //Evita avancar sem um nome valido
        if(string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Gameplay");
    }
}