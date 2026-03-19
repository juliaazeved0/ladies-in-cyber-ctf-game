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
        }
    }
}