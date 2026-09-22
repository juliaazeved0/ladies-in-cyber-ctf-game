using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

/// <summary>
/// Responsavel por extrair o valor da flag em um slot de texto e
/// copiar o conteudo para a area de transferencia do sistema.
/// </summary>
public class ClipboardManager : MonoBehaviour
{
    [Header("Text to Copy")]
    [Tooltip("O elemento de texto que contem a string formatada 'Desafio - Flag'.")]
    public TextMeshProUGUI slotText;

#if UNITY_WEBGL && !UNITY_EDITOR
    //Ponte para a funcao JavaScript definida em MyPlugin.jslib
    //So eh compilada em build WebGL (fora do Editor), ja que depende do navegador
    [DllImport("__Internal")]
    private static extern void JS_CopyToClipboard(string text);
#endif

    /// <summary>
    /// Extrai a flag do texto do slot (parte pos " - " e copia para a area de
    /// transferencia, usando a Clipboard API do navegador em WebGL ou o
    /// clipboard do sistema em outras plataformas/Editor.
    /// </summary>
    public void CopyToClipboard()
    {
        Debug.Log("[ClipboardManager] Botão de copiar clicado.");

        //Evita prosseguir se o texto de referencia nao estiver configurado
        if(slotText == null)
        {
            Debug.LogError("[ClipboardManager] slotText não está atribuído no Inspector.");
            return;
        }

        string textToCopy = slotText.text;
        Debug.Log($"[ClipboardManager] Texto do slot: \"{textToCopy}\"");

        //Por padrao, copia o texto inteiro caso nao tenha o separador " - "
        string flagToCopy = textToCopy;

        //Extrai apenas a flag (parte apos " - "), descartando o nome do desafio
        if(textToCopy.Contains(" - "))
        {
            string[] parts = textToCopy.Split(new string[] { " - " }, System.StringSplitOptions.None);

            if(parts.Length >= 2)
            {
                flagToCopy = parts[1];
            }
        }

        if(!string.IsNullOrEmpty(flagToCopy))
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            //Em builds WebGL, usa a Clipboard API real do navegador via jslib
            JS_CopyToClipboard(flagToCopy);
#else
            //No Editor e em build standalone, o clipboard do sistema funciona normalmente
            GUIUtility.systemCopyBuffer = flagToCopy;
#endif
            Debug.Log($"[ClipboardManager] Flag copiada com sucesso: \"{flagToCopy}\"");
        }
        else
        {
            Debug.LogWarning("[ClipboardManager] Nenhuma flag válida encontrada para copiar.");
        }
    }
}