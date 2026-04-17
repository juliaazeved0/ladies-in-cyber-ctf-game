using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossTeleportTrigger : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "PlayerMap";
    [SerializeField] private Vector3 teleportPosition = new Vector3(49, -18, 0);

    [Header("Fade (opcional - configure no Inspector)")]
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private GameObject panelBlack;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BossTeleportTrigger - Collider detectado: " + other.gameObject.name);

        if (other.CompareTag("Player") && !isTransitioning)
        {
            Debug.Log("Player entrou na área de teleporte! Retornando ao mapa.");
            isTransitioning = true;

            PlayerPrefs.SetFloat(mapSceneName + "_PlayerX", teleportPosition.x);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerY", teleportPosition.y);
            PlayerPrefs.SetFloat(mapSceneName + "_PlayerZ", teleportPosition.z);
            PlayerPrefs.Save();

            if (fadeAnimator != null && panelBlack != null)
            {
                panelBlack.SetActive(true);
                fadeAnimator.SetTrigger("FadeOut");
                // CarregarCenaMap() deve ser chamado pelo evento da animação de fade
            }
            else
            {
                StartCoroutine(CarregarComDelay());
            }
        }
    }

    // Chamado pelo Animation Event no Inspector (se tiver fade)
    public void CarregarCenaMap()
    {
        SceneManager.LoadSceneAsync(mapSceneName);
    }

    private IEnumerator CarregarComDelay()
    {
        yield return new WaitForSeconds(0.3f);
        CarregarCenaMap();
    }
}
