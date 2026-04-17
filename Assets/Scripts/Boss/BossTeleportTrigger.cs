using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTeleportTrigger : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "PlayerMap";
    [SerializeField] private Vector3 teleportPosition = new Vector3(49, -18, 0);

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collider detectado: " + other.gameObject.name);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou na área de teleporte!");
            
            // Set the position in PlayerPrefs for the map scene
            string keyX = mapSceneName + "_PlayerX";
            string keyY = mapSceneName + "_PlayerY";
            string keyZ = mapSceneName + "_PlayerZ";
            PlayerPrefs.SetFloat(keyX, teleportPosition.x);
            PlayerPrefs.SetFloat(keyY, teleportPosition.y);
            PlayerPrefs.SetFloat(keyZ, teleportPosition.z);
            PlayerPrefs.Save();


        }
    }
}