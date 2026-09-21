using UnityEngine;
using Cinemachine;

/// <summary>
/// Configura a camera virtual (Cinemachine) desta
/// cena para seguir o Transform da player.
/// </summary>
public class CameraFollowSetup : MonoBehaviour
{
    void Start()
    {
        //Assume que ja foi atribuido por outro script antes do Start() rodar
        Transform playerTransform = DataPlayerPosition.PlayerTransform;

        //Se o player ainda nao existir na cena nesse momento, cai no erro abaixo
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