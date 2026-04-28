using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script utilitario para limpar todos os dados salvos via PlayerPrefs.
/// </summary>
public class ResetPlayerPrefsTest : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}