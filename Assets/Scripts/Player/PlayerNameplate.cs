using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameplate : MonoBehaviour
{
   public TextMeshProUGUI nameplatePlayer;
   public TextMeshProUGUI nameplateIdPlayer;

   private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    void Start()
    {
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "PLAYER");
        nameplatePlayer.text = namePlayer.ToUpper();
    }
   
    public void SetNameplateIdPlayer()
    {
        nameplateIdPlayer.text = "Téc. cibersegurança";
    }
}
