using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Gerencia a entrada de nome da jogadora na tela de login, salvando
/// o valor em PlayerPrefs e avancando para a cena de gameplay.
/// </summary>
public class PlayerNameManager : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("Campo de texto onde a jogadora digita o nome.")]
    [SerializeField] private TMP_InputField nameInput;

    //Chave publica para que outros scripts leiam o mesmo valor salvo sem precisar repetir a string manualmente
    public const string PLAYER_NAME_KEY = "PLAYER_NAME";

    /// <summary>
    /// Chamado pelo botao "Play" da UI. Valida, salva e avanca de cena.
    /// </summary>
    public void OnClickPlay()
    {
        //Evita NullReferenceException caso o campo nao tenha sido arrastado no Inspector
        if(nameInput == null)
        {
            Debug.LogError($"{gameObject.name} esta sem referência ao InputField!");
            return;
        }

        string playerName = nameInput.text.Trim();

        //Evita salvar e avancar de cena com um nome vazio ou so espacos
        if(string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Gameplay");
    }
}