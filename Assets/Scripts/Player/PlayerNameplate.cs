using UnityEngine;
using TMPro;

public class PlayerNameplate : MonoBehaviour
{
    public TextMeshProUGUI nameplatePlayer;
    public TextMeshProUGUI nameplateIdPlayer; 

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    private const string PLAYER_NAMEPLATE_KEY = "nameplatePlayer"; 

    void Start()
    {
     
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "PLAYER");
        nameplatePlayer.text = namePlayer.ToUpper();

        string savedId = PlayerPrefs.GetString(PLAYER_NAMEPLATE_KEY, ""); 
        if (!string.IsNullOrEmpty(savedId))
        {
            nameplateIdPlayer.text = savedId;
        }
    }

    public void SetNameplateIdPlayer()
    {
        string newNameplatePlayer = "CyberTech";
        
      
        nameplateIdPlayer.text = newNameplatePlayer;

        
        PlayerPrefs.SetString(PLAYER_NAMEPLATE_KEY, newNameplatePlayer);
        PlayerPrefs.Save();
    }
}