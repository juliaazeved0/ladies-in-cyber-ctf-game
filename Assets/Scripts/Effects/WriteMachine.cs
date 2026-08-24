using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Efeito de "maquina de escrever" para textos de dialogo: revela o texto 
/// caractere por caractere em um intervalo configuravel, com suporte a 
/// completar na hora o texto atual antes de terminar a animacao.
/// </summary>
public class WriteMachine : MonoBehaviour
{
    [Header("Settings Text Effect")]
    [Tooltip("Intervalo em segundos entre a exibicao de cada caractere.")]
    public float speedWriter = 0.08f;

    //Exposto como propriedade somente leitura externamente, ja que o
    //controle do estado deve acontecer apenas internamente nessa classe
    public bool IsTyping { get; private set; } = false;

    private Coroutine typingCoroutine;
    private TextMeshProUGUI currentLabel;
    private string currentFullText;

    /// <summary>
    /// Inicia ou reinicia o efeito de digitacao em um TextMeshProUGUI
    /// especifico, exibindo o texto informado gradualmente.
    /// </summary>
    public void Run(string textWrite, TextMeshProUGUI textMeshPro)
    {
        //Evita iniciar a coroutine com referencias invalidas
        if(textMeshPro == null)
        {
            Debug.LogError("Run() foi chamado com um TextMeshProUGUI nulo!");
            return;
        }

        if(textWrite == null)
        {
            Debug.LogError("Run() foi chamado com um texto nulo!");
            return;
        }

        currentFullText = textWrite;
        currentLabel = textMeshPro;

        //Interrompe qualquer digitacao anterior em andamento antes de iniciar a nova
        if(typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    /// <summary>
    /// Revela o texto completo um caractere por vez, respeitando
    /// o intervalo definido na variavel speedWriter.
    /// </summary>
    private IEnumerator TypeText()
    {
        IsTyping = true;
        currentLabel.text = "";

        foreach(char key in currentFullText.ToCharArray())
        {
            currentLabel.text += key;
            yield return new WaitForSeconds(speedWriter);
        }

        IsTyping = false;
        typingCoroutine = null;
    }

    /// <summary>
    /// Pula direto para o final da digitacao em andamento, exibindo o texto
    /// completo instantaneamente. Usado quando a jogadora aperta a tecla
    /// de interacao enquanto o texto esta sendo "digitado".
    /// </summary>
    public void Complete()
    {
        if(!IsTyping) return;

        if(typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentLabel.text = currentFullText;
        IsTyping = false;
        typingCoroutine = null;
    }
}