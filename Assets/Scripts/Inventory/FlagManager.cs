using System.Collections.Generic;
using System.Linq;
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

            flagsCapture = new List<string>();

            if (PlayerPrefs.HasKey("SavedFlags"))
            {
                string savedData = PlayerPrefs.GetString("SavedFlags");
                
                if (!string.IsNullOrEmpty(savedData))
                {
                    flagsCapture = new List<string>(System.Array.FindAll(savedData.Split('|'), s => !string.IsNullOrEmpty(s)));
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveFlag(string challengeName, string flag)
    {
        string fullFlag = challengeName + " - " + flag;
        if (!flagsCapture.Contains(fullFlag))
        {
            flagsCapture.Add(fullFlag);
            Debug.Log("Sucesso! Flag guardada no inventário: " + fullFlag);
            PlayerPrefs.SetString("SavedFlags", string.Join("|", flagsCapture));
            PlayerPrefs.Save();
        }
    }

    public bool IsFlagCaptured(string flag)
    {
        return flagsCapture.Any(f => f.EndsWith(flag));
    }
}
