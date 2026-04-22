using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollowPlayer : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        // 1. A MÁGICA: Se eu não tenho um alvo (ou acabei de trocar de cena), eu procuro por ele!
        if (target == null)
        {
            // Procura qualquer objeto na tela que tenha a Tag "Player"
            GameObject playerOnMap = GameObject.FindGameObjectWithTag("Player");
            
            if (playerOnMap != null)
            {
                target = playerOnMap.transform; // Achei! Agora eu sigo ele.
            }
            else
            {
                return; // Se eu não achei ninguém (ex: a tela ainda está preta carregando), eu espero.
            }
        }

        // 2. Se eu tenho um alvo, eu sigo ele!
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }
}