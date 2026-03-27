using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
  void Awake()
{
    DontDestroyOnLoad(gameObject);
}
}
