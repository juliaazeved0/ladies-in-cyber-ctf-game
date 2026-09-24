using UnityEngine;
using Cinemachine;

/// <summary>
/// Configura a camera virtual (Cinemachine) desta cena para seguir
/// o Transform da player, obtido a partir de DataPlayerPosition.
/// </summary>
public class CameraFollowSetup : MonoBehaviour
{
    void Start()
    {
        //Se a player ainda nao existir na cena nesse momento, cai no erro abaixo
        Transform playerTransform = DataPlayerPosition.PlayerTransform;

        if(playerTransform == null)
        {
            Debug.LogError("DataPlayerPosition.PlayerTransform é null. " +
                             "Verifique se a player existe na cena inicial.");
            return;
        }

        var virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if(virtualCamera != null)
        {
            virtualCamera.Follow = playerTransform;
        }
        else
        {
            Debug.LogError($"CinemachineVirtualCamera não encontrado no GameObject '{gameObject.name}'.");
        }
    }
}