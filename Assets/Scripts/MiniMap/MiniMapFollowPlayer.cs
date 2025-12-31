using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class MiniMapFollowPlayer : MonoBehaviour
{
   public Transform target;

   void LateUpdate()
    {
        if(target == null) return;

        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }
}
