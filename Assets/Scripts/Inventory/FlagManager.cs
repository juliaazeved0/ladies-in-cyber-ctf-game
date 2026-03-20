using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;
    
    public List<string> flagsCapture = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // carrega dados e garante a permanencia da INSTANCIA 
            if (PlayerPrefs.HasKey("SavedFlags"))
            {
                string savedData = PlayerPrefs.GetString("SavedFlags");
                
                if (!string.IsNullOrEmpty(savedData))
                {
                    flagsCapture = new List<string>(savedData.Split('|'));
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveFlag(string newFlag)
    {
        if (!flagsCapture.Contains(newFlag))
        {
            flagsCapture.Add(newFlag);
            Debug.Log("Sucesso! Flag guardada no inventário: " + newFlag);
            PlayerPrefs.SetString("SavedFlags", string.Join("|", flagsCapture));
            PlayerPrefs.Save();
        }
    }
}