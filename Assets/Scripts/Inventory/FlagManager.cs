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
            
            // carrega dados e garante a permanencia da instancia
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

    // Modificado para salvar o nome do desafio concatenado com a flag, separado por " - "
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

    // Método auxiliar para verificar se uma flag específica foi capturada (verifica se algum item termina com a flag)
    public bool IsFlagCaptured(string flag)
    {
        return flagsCapture.Any(f => f.EndsWith(flag));
    }
}