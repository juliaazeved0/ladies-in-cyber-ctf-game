using UnityEngine;
using Cinemachine;

/// <summary>
/// Configura a camera virtual do Cinemachine para 
/// seguir a jogadora assim que a cena carrega.
/// </summary>
public class CameraFollowSetup : MonoBehaviour
{
    void Start()
    {
        //Busca a referencia da jogadora salva de forma persistente
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
            //Define o alvo que a camera virtual deve seguir
            virtualCamera.Follow = playerTransform;
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera não encontrado " + "neste GameObject.");
        }
    }
}