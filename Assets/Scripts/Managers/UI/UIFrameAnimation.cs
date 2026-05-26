using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Anima uma sequencia de sprites em um componente de UI Image.
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
        //Se targetImage for nula, tenta encontrar no proprio GameObject
        if(targetImage == null)
        {
            targetImage = GetComponent<Image>();

            //Se ainda for nulo, avisa no console para evitar o crash
            if(targetImage == null)
            {
                Debug.LogError($"[UIFrameAnimation] O GameObject {gameObject.name} precisa de um componente Image!");
            }
        }
    }
    private void Update()
    {
        //Se a lista estiver vazia ou nula, interrompe a execucao
        if(frames.Length == 0) return;

        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        targetImage.sprite = frames[index];
    }
}