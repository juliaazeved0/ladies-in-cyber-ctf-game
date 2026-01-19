using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefsTest : MonoBehaviour
{

    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }


}
