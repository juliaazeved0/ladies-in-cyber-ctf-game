using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Anima uma UI Image trocando seus sprites em sequencia, simulando uma
/// animacao quadro a quadro em uma taxa de FPS configuravel.
/// </summary>
public class UIFrameAnimation : MonoBehaviour
{
    [Header("Target UI ELements")]
    [Tooltip("Componente de interface que mostrara os frames.")]
    public Image targetImage;

    [Header("Animation Settings")]
    [Tooltip("Lista de sprites para a animacao.")]
    public Sprite[] frames;

    [Tooltip("Velocidade da animacao em quadros por segundo.")]
    public float framesPerSecond = 7f;

    private void Awake()
    {
        //Se nao arrastou a referencia no Inspector, tenta pegar o componente Image no proprio GameObject
        if(targetImage == null)
        {
            targetImage = GetComponent<Image>();

            if(targetImage == null)
            {
                Debug.LogError($"O GameObject {gameObject.name} precisa de um componente Image!");
            }
        }
    }
    private void Update()
    {
        //Sem frames, nao ha o que animar
        if(frames == null || frames.Length == 0) return;

        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        targetImage.sprite = frames[index];
    }
}