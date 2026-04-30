using UnityEngine;
using TMPro;

/// <summary>
/// Responsavel por extrair o valor da flag em um slot de texto e
/// copiar o conteudo para a area de transferencia do sistema.
/// </summary>
public class ClipboardManager : MonoBehaviour
{
    [Header("Text to Copy")]
    [Tooltip("O elemento de texto que contem a string formatada 'Desafio - Flag'.")]
    public TextMeshProUGUI slotText;

    /// <summary>
    /// Copia a parte da Flag (apos o ' - ') para o clipboard.
    /// </summary>
    public void CopyToClipboard()
    {
        Debug.Log("copy clicado");

        if(slotText == null)
        {
            Debug.LogError("slot vazio");
            return;
        }

        string textToCopy = slotText.text;
        Debug.Log("texto do slot[" + textToCopy + "]");

<<<<<<< HEAD
        // Extrai apenas a flag dps de -
=======
>>>>>>> aa32d9583d26a4cf39bbc9ec0c5a3254faa1967d
        string flagToCopy = textToCopy;

        //Extrai apenas a flag (parte apos " - ")
        if (textToCopy.Contains(" - "))
        {
            string[] parts = textToCopy.Split(new string[] { " - " }, System.StringSplitOptions.None);

            if(parts.Length >= 2)
            {
                flagToCopy = parts[1];
            }
        }

        if(!string.IsNullOrEmpty(flagToCopy))
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