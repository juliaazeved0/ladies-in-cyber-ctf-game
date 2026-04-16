using UnityEngine;
using Cinemachine;

/// <summary>
/// Atribui dinamicamente o Follow do Cinemachine Virtual Camera ao player
/// persistente (DontDestroyOnLoad). Adicione este script ao GameObject
/// "Virtual Camera" em qualquer cena que precise seguir o player.
/// </summary>
public class CameraFollowSetup : MonoBehaviour
{
    void Start()
    {
        Transform player = DataPlayerPosition.PlayerTransform;

        if (player == null)
        {
            Debug.LogWarning("[CameraFollowSetup] DataPlayerPosition.PlayerTransform é null. " +
                             "Verifique se o player existe na cena inicial.");
            return;
        }

        var vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.Follow = player;
        }
        else
        {
            Debug.LogWarning("[CameraFollowSetup] CinemachineVirtualCamera não encontrado " +
                             "neste GameObject.");
        }
    }
}
