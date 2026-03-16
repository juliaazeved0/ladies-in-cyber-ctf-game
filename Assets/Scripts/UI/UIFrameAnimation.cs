using UnityEngine;
using UnityEngine.UI;

public class UIFrameAnimation : MonoBehaviour
{
    public Image uiImage;
    public Sprite[] frames;
    public float framesPerSecond = 7f;

    private void Update()
    {
        // Calcula qual frame deve aparecer baseado no tempo do jogo
        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        uiImage.sprite = frames[index];
    }
}