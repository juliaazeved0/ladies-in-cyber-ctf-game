using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WriteMachine : MonoBehaviour
{
    [Header("Settings Text Effect")]
    public float speedWriter = 0.08f;
    public bool IsTyping { get; private set; } = false;

    private Coroutine typingCoroutine;
    private TextMeshProUGUI currentLabel;
    private string currentFullText;

    public void Run(string textWrite, TextMeshProUGUI textMeshPro)
    {
        currentFullText = textWrite;
        currentLabel = textMeshPro;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        IsTyping = true;
        currentLabel.text = "";

        foreach (char key in currentFullText.ToCharArray())
        {
            currentLabel.text += key;
            yield return new WaitForSeconds(speedWriter);
        }

        IsTyping = false;
        typingCoroutine = null;
    }

    public void Complete()
    {
        if (!IsTyping) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentLabel.text = currentFullText;
        IsTyping = false;
        typingCoroutine = null;
    }
}
