using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Anima uma sequencia de sprites em um componente de UI Image.
/// </summary>
public class UIFrameAnimation : MonoBehaviour
{
    [Tooltip("Componente de interface que mostrara os frames.")]
    public Image uiImage;

    [Tooltip("Lista de sprites para a animacao.")]
    public Sprite[] frames;

    [Tooltip("Velocidade da animacao em quadros por segundo.")]
    public float framesPerSecond = 7f;

    private void Update()
    {
        if(frames.Length == 0) return; //Se a lista estiver vazia, para a execucao e nem faz a conta

        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        uiImage.sprite = frames[index];
    }
}