using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Anima uma Image de UI trocando sprites em sequencia (efeito de flipbook),
/// com a velocidade controlada por quadros por segundo.
/// </summary>
public class UIFrameAnimation : MonoBehaviour
{
    [Header("Target UI Elements")]
    [Tooltip("Componente de interface que mostrara os frames.")]
    public Image targetImage;

    [Header("Animation Settings")]
    [Tooltip("Lista de sprites para a animacao.")]
    public Sprite[] frames;

    [Tooltip("Velocidade da animacao em quadros por segundo.")]
    public float framesPerSecond = 7f;

    private void Awake()
    {
        if(targetImage == null)
        {
            targetImage = GetComponent<Image>();

            if(targetImage == null)
            {
                Debug.LogError($"O GameObject {gameObject.name} precisa de um componente Image!");
                enabled = false;
                return;
            }
        }
    }
    private void Update()
    {
        if(targetImage == null || frames == null || frames.Length == 0) return;

        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        targetImage.sprite = frames[index];
    }
}