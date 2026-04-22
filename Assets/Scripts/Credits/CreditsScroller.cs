using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsRect;
    public float scrollSpeed = 100f; //Velocidade da subida
    public float stopPositionY = 1500f; //A posição Y onde o crédito termina

    private bool canScroll = true;

    void Start()
    {
        //A imagem para quando o topo dela subir o suficiente para o fundo encostar na tela
        stopPositionY = creditsRect.rect.height - Screen.height;

        //stopPositionY -= 50f;
    }
    void Update()
    {
        if (canScroll)
        {
            //Move a imagem para cima baseada no tempo
            creditsRect.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

            //Verifica se atingiu ou passou da posição de parada
            if(creditsRect.anchoredPosition.y >= stopPositionY)
            {
                //Trava a posição no valor exato do limite e para o movimento
                creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, stopPositionY);
                canScroll = false;
                Debug.Log("Créditos finalizados e mantidos.");
            }
        }
    }
}