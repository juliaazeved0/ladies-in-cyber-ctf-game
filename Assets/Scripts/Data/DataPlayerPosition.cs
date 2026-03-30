using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayerPosition : MonoBehaviour
{
    void Start()
    {
        LoadGame(); // Carrega a posição salva ao iniciar o jogo
    }

    void OnApplicationQuit() // Corrigido: era "OnApllicationQuit"
    {
        SaveGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
