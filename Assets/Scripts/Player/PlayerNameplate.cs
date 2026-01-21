using UnityEngine;
using TMPro;

public class PlayerNameplate : MonoBehaviour
{
    public TextMeshProUGUI nameplatePlayer;
    public TextMeshProUGUI nameplateIdPlayer; // É aqui que o "CyberTech" deve aparecer

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    private const string PLAYER_NAMEPLATE_KEY = "nameplatePlayer"; // Renomeei levemente para evitar confusão com a variável lá de cima

    void Start()
    {
        // Lógica original do nome do jogador
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "PLAYER");
        nameplatePlayer.text = namePlayer.ToUpper();

        // --- CORREÇÃO AQUI ---
        // 1. Recuperamos o valor salvo usando a chave correta
        // 2. Jogamos direto no componente de texto (nameplateIdPlayer) para aparecer na tela
        string savedId = PlayerPrefs.GetString(PLAYER_NAMEPLATE_KEY, ""); 
        if (!string.IsNullOrEmpty(savedId))
        {
            nameplateIdPlayer.text = savedId;
        }
    }

    public void SetNameplateIdPlayer()
    {
        string newNameplatePlayer = "CyberTech";
        
        // Atualiza na tela agora
        nameplateIdPlayer.text = newNameplatePlayer;

        // --- CORREÇÃO AQUI ---
        // Usamos a constante "PLAYER_NAMEPLATE_KEY" (que é a string "nameplatePlayer") como o endereço do save
        PlayerPrefs.SetString(PLAYER_NAMEPLATE_KEY, newNameplatePlayer);
        PlayerPrefs.Save();
    }
}