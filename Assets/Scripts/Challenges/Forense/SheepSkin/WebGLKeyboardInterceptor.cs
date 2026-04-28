using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLKeyboardInterceptor : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitKeyboardInterceptor();

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        InitKeyboardInterceptor();
#endif
    }
}