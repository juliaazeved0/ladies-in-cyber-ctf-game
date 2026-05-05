using UnityEngine;
using TMPro;

/// <summary>
/// Gerencia a exibicao do nome e do ID da jogador5a na placa de identificacao (Nameplate).
/// Recupera os dados salvos no PlayerPrefs ao iniciar a cena.
/// </summary>
public class PlayerNameplate : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameplatePlayer;
    [SerializeField] private TextMeshProUGUI nameplateIdPlayer; 

    //Cgaves constantes para evitar erros de digitacao ao acessar o PlayerPrefs
    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    private const string PLAYER_NAMEPLATE_KEY = "nameplatePlayer"; 

    /// <summary>
    /// Inicializa os textos da placa com os dados salvos localmente.
    /// </summary>
    void Start()
    {
     
        //Recupera o nome da jogadora (Padrao: Player)
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "PLAYER");
        nameplatePlayer.text = namePlayer.ToUpper();

        //Recupera o ID da jogadora
        string savedId = PlayerPrefs.GetString(PLAYER_NAMEPLATE_KEY, ""); 

        if(!string.IsNullOrEmpty(savedId))
        {
            nameplateIdPlayer.text = savedId;
        }
    }

    /// <summary>
    /// Define um novo id para a jogadora e salva a alteracao.
    /// </summary>
    public void SetNameplateIdPlayer()
    {
        string newNameplatePlayer = "CyberTech";
      
        nameplateIdPlayer.text = newNameplatePlayer;
        
        //Salva a alteracao permanentemente
        PlayerPrefs.SetString(PLAYER_NAMEPLATE_KEY, newNameplatePlayer);
        PlayerPrefs.Save();
    }
}