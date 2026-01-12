using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WriteMachine : MonoBehaviour
{
    [Header("Settings Text Effect")]
    public float speedWriter = 0.05f;
    public bool IsTyping { get; private set; } = false;
    private Coroutine typingCoroutine;

    public void Run(string textWrite, TextMeshProUGUI textMeshPro)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(textWrite, textMeshPro));
    }

    private IEnumerator TypeText(string allText, TextMeshProUGUI textMeshPro)
    {
        IsTyping = true;
        textMeshPro.text = "";

        foreach (char key in allText.ToCharArray())
        {
            textMeshPro.text += key;
            yield return new WaitForSeconds(speedWriter);
        }

        IsTyping = false;
        typingCoroutine = null;
    }

}
