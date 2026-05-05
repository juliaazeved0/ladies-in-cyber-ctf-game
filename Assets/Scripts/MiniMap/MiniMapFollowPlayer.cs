using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla a posicao da camera do MiniMapa para seguir a jogadora.
/// Garante que o alvo seja encontrado mesmo apos trocas de cena.
/// </summary>
public class MiniMapFollowPlayer : MonoBehaviour
{
    [Tooltip("O alvo que o MiniMapa deve seguir (Player).")]
    [SerializeField] private Transform target;

    void LateUpdate()
    {
        //Se nao possui um alvo ou troca de cena, procura por ele
        if(target == null)
        {
            GameObject playerOnMap = GameObject.FindGameObjectWithTag("Player");
            
            if(playerOnMap != null)
            {
                target = playerOnMap.transform;
            }
            else
            {
                return;
            }
        }

        //Se possuir um alvo, segue
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }
}