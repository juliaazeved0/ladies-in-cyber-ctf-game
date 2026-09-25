using UnityEngine;

/// <summary>Persiste a conclusao dos desafios e reconhece flags de saves anteriores.</summary>
public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance;
    private const string KeyPrefix = "ChallengeCompleted_";

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if(Instance == this) Instance = null;
    }

    public void CompleteChallenge(string challengeID)
    {
        if(string.IsNullOrEmpty(challengeID)) return;
        PlayerPrefs.SetInt(KeyPrefix + challengeID, 1);
        PlayerPrefs.Save();
    }

    public bool IsChallengeCompleted(string challengeID) => IsCompleted(challengeID);

    public static bool IsCompleted(string challengeID)
    {
        if(string.IsNullOrEmpty(challengeID)) return false;
        if(PlayerPrefs.GetInt(KeyPrefix + challengeID, 0) == 1) return true;

        // Compatibilidade com partidas que salvaram apenas o inventario.
        int[] flag;
        switch(challengeID)
        {
            case "CryptoPassword": flag = SafeBase.flag_6; break;
            case "CryptoCapivara": flag = SafeBase.flag_7; break;
            case "DesafioPressao": flag = SafeBase.flag_1; break;
            default: return false;
        }
        return FlagManager.HasSavedFlag(SafeBase.ViewBase(flag));
    }
}
