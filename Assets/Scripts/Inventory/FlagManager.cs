using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gerencia a coleta e persistencia das flags capturadas pela jogadora durante o CTF.
/// Implementado como SIngleton para manter o estado entre as cenas. As flags sao
/// salvas localmente via PlayerPrefs como uma string unica separada por '|'.
/// </summary>
public class FlagManager : MonoBehaviour
{
    [Header("Singleton")]
    [Tooltip("Referencia estatica unica para acesso global ao FlagManager (padrao Singleton).")]
    public static FlagManager Instance;

    [Header("Captured Flags")]
    [Tooltip("Lista de flags ja capturadas, no formato 'NomeDoDesafio - Flag'.")]
    public List<string> flagsCapture = new List<string>();

    void Awake()
    {
        //Garante que exista apenas uma instancia do FlagManager na aplicacao
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            flagsCapture = new List<string>();

            //Carrega as flags previamente salvas, se existirem
            if(PlayerPrefs.HasKey("SavedFlags"))
            {
                string savedData = PlayerPrefs.GetString("SavedFlags");
                
                if(!string.IsNullOrEmpty(savedData))
                {
                    //Divide a stirng salva pelo delimitador e remove entradas vazias
                    flagsCapture = new List<string>(System.Array.FindAll(savedData.Split('|'), s => !string.IsNullOrEmpty(s)));
                }
            }
        }
        else
        {
            //Ja existe uma instancia ativa: destroi este objeto duplicado
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Registra uma flag capturada (se ainda nao estiver na lista) e persiste
    /// o progresso no PlayerPrefs.
    /// </summary>
    public void SaveFlag(string challengeName, string flag)
    {
        string fullFlag = challengeName + " - " + flag;

        //Evita duplicar a mesma flag na lista
        if(!flagsCapture.Contains(fullFlag))
        {
            flagsCapture.Add(fullFlag);
            Debug.Log("Sucesso! Flag guardada no inventário: " + fullFlag);

            //Se o salvamento falhar, o jogo nao quebra
            try
            {
                //Persiste a lista inteira como uma unica string separada pelo delimitador
                PlayerPrefs.SetString("SavedFlags", string.Join("|", flagsCapture));
                PlayerPrefs.Save();
            }
            //Registra o erro no console e continua rodando, a flag fica na lista de memoria
            catch(System.Exception e)
            {
                Debug.LogError("Falha ao salvar flags no PlayerPrefs: " + e.Message);
            }
        }
    }

    public static bool HasAllMainFlags()
    {
        for(int index = 0; index < 8; index++)
        {
            string flag = SafeBase.GetFlag(index);
            if(Instance != null ? !Instance.IsFlagCaptured(flag) : !HasSavedFlag(flag)) return false;
        }
        return true;
    }

    public static bool HasSavedFlag(string flag)
    {
        if(string.IsNullOrEmpty(flag)) return false;
        string suffix = " - " + flag;
        return PlayerPrefs.GetString("SavedFlags", "").Split('|')
            .Any(entry => entry.EndsWith(suffix, System.StringComparison.Ordinal));
    }

    //Verifica se uma determinada flag ja foi capturada
    public bool IsFlagCaptured(string flag)
    {
        return !string.IsNullOrEmpty(flag) && flagsCapture.Any(f => f.EndsWith(" - " + flag, System.StringComparison.Ordinal));
    }
}