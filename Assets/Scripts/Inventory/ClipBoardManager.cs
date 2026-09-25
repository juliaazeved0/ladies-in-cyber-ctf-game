using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

/// <summary>Copia texto pelo navegador em WebGL e pelo sistema no Editor.</summary>
public class ClipboardManager : MonoBehaviour
{
    public TextMeshProUGUI slotText;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void JS_CopyToClipboard(string text);
#endif

    public void CopyToClipboard()
    {
        if(slotText != null) CopyFlag(slotText.text);
    }

    public static string ExtractFlag(string entry)
    {
        if(string.IsNullOrEmpty(entry)) return "";
        int separator = entry.IndexOf(" - ", System.StringComparison.Ordinal);
        return (separator < 0 ? entry : entry.Substring(separator + 3)).Trim();
    }

    public static void CopyFlag(string entry) => CopyText(ExtractFlag(entry));

    public static void CopyText(string text)
    {
        if(string.IsNullOrEmpty(text)) return;
#if UNITY_WEBGL && !UNITY_EDITOR
        JS_CopyToClipboard(text);
#else
        GUIUtility.systemCopyBuffer = text;
#endif
    }
}
