using UnityEngine;
using TMPro;

public class ClipboardManager : MonoBehaviour
{
    [Header("Text to Copy")]
    public TextMeshProUGUI slotText;

    public void CopyToClipboard()
    {
        Debug.Log("copy clicado");

        if (slotText == null)
        {
            Debug.LogError("slot vazio");
            return;
        }

        string textToCopy = slotText.text;
        Debug.Log("texto do slot[" + textToCopy + "]");

        // Extrai apenas a flag dps de -
        string flagToCopy = textToCopy;
        if (textToCopy.Contains(" - "))
        {
            string[] parts = textToCopy.Split(new string[] { " - " }, System.StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                flagToCopy = parts[1];
            }
        }

        if (!string.IsNullOrEmpty(flagToCopy))
        {
            GUIUtility.systemCopyBuffer = flagToCopy;
            Debug.Log("flag copiada [" + flagToCopy + "]");
        }
        else
        {
            Debug.LogWarning("nao copiada");
        }
    }
}