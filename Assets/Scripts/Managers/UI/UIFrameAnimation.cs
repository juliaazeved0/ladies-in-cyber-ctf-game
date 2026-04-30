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

    private void Awake()
    {
        //Se uiImage for nula, tenta encontrar no próprio GameObject
        if(uiImage == null)
        {
            uiImage = GetComponent<Image>();

            //Se ainda for nulo, avisa no console para evitar o crash
            if(uiImage == null)
            {
                Debug.LogError($"UIFrameAnimation em {gameObject.name} precisa de um componente Image!");
            }
        }
    }
    private void Update()
    {
        if(frames.Length == 0) return; //Se a lista estiver vazia, para a execucao e nem faz a conta

        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        uiImage.sprite = frames[index];
    }
}