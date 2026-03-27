using UnityEngine;
using TMPro;

public class ClipboardManager : MonoBehaviour
{
    [Header("Text to Copy")]
    public TextMeshProUGUI slotText;

    public void CopyToClipboard()
    {
        Debug.Log("1. The copy button was clicked!");

        if (slotText == null)
        {
            Debug.LogError("2. ERROR: The 'Slot Text' is empty in the Inspector. Drag the Text object here!");
            return;
        }

        string textToCopy = slotText.text;
        Debug.Log("3. The text found in the slot is: [" + textToCopy + "]");

        if (!string.IsNullOrEmpty(textToCopy))
        {
            GUIUtility.systemCopyBuffer = textToCopy;
            Debug.Log("4. SUCCESS! Sent to Windows/Linux clipboard!");
        }
        else
        {
            Debug.LogWarning("5. The text was empty, so nothing was copied.");
        }
    }
}