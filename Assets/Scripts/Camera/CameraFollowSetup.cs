using UnityEngine;
using Cinemachine;

/// <summary>
/// Atribui dinamicamente o alvo de seguimento (Follow) da Cinemachine Virtual Camera
/// para a jogadora persistente (DontDestroyOnLoad).
/// </summary>
public class CameraFollowSetup : MonoBehaviour
{
    void Start()
    {

        //Obtem a referencia estatica do Transform da jogadora
        Transform playerTransform = DataPlayerPosition.PlayerTransform;

        if(playerTransform == null)
        {
            Debug.LogWarning("[CameraFollowSetup] DataPlayerPosition.PlayerTransform é null. " +
                             "Verifique se a player existe na cena inicial.");
            return;
        }

        //Tenta obter o componente da Virtual Camera
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if(virtualCamera != null)
        {
            //Atribui a jogadora como alvo para a camera seguir
            virtualCamera.Follow = playerTransform;
        }
        else
        {
            Debug.LogWarning("[CameraFollowSetup] CinemachineVirtualCamera não encontrado " +
                             "neste GameObject.");
        }
    }
}