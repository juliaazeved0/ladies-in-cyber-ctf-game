using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gerencia o inventario de flags capturada pela jogadora.
/// Responsavel por salvar, carregar e verificar o status das flags persistidas.
/// </summary>
public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;
    
    public List<string> flagsCapture = new List<string>();

    /// <summary>
    /// Carrega as flags salvas no disco (PlayerPrefs) ao iniciar o jogo.
    /// </summary>
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            flagsCapture = new List<string>();

            if(PlayerPrefs.HasKey("SavedFlags"))
            {
                string savedData = PlayerPrefs.GetString("SavedFlags");
                
                if(!string.IsNullOrEmpty(savedData))
                {
                    //Divide a string salva pelo caractere separador '|'
                    flagsCapture = new List<string>(System.Array.FindAll(savedData.Split('|'), s => !string.IsNullOrEmpty(s)));
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Salva uma nova flag no inventario e persiste no PlayerPrefs.
    /// </summary>
    /// <param name="challengeName">Nome do desafio associado.</param>
    /// <param name="flag">O conteudo da flag.</param>
    public void SaveFlag(string challengeName, string flag)
    {
        string fullFlag = challengeName + " - " + flag;

        if(!flagsCapture.Contains(fullFlag))
        {
            flagsCapture.Add(fullFlag);
            Debug.Log("Sucesso! Flag guardada no inventário: " + fullFlag);

            //Persiste a lista atualizada
            PlayerPrefs.SetString("SavedFlags", string.Join("|", flagsCapture));
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Verifica se uma flag especifica ja foi capturada.
    /// </summary>
    public bool IsFlagCaptured(string flag)
    {
        return flagsCapture.Any(f => f.EndsWith(flag));
    }
}
