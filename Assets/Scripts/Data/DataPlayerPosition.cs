using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPlayerPosition : MonoBehaviour
{
    // Nome da cena cacheado em Awake — garante a chave correta mesmo
    // durante o descarregamento, quando GetActiveScene() já pode retornar
    // o nome da cena seguinte.
    private string sceneName;

    private string KeyX => sceneName + "_PlayerX";
    private string KeyY => sceneName + "_PlayerY";

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        LoadGame();
    }

    // Disparado quando o objeto é destruído, incluindo ao descarregar a cena.
    // Garante que a posição seja salva na transição entre cenas.
    void OnDestroy()
    {
        SaveGame();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat(KeyX, transform.position.x);
        PlayerPrefs.SetFloat(KeyY, transform.position.y);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(KeyX))
        {
            float x = PlayerPrefs.GetFloat(KeyX);
            float y = PlayerPrefs.GetFloat(KeyY);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
