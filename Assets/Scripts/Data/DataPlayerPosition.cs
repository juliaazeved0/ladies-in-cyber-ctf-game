using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10)]
public class DataPlayerPosition : MonoBehaviour
{
    // Referência estática para o player persistente.
    // Usada por outros scripts (ex.: CameraFollowSetup) sem precisar de Find.
    public static Transform PlayerTransform { get; private set; }

    private string sceneName;

    private string KeyX => sceneName + "_PlayerX";
    private string KeyY => sceneName + "_PlayerY";
    private string KeyZ => sceneName + "_PlayerZ";

    void Awake()
    {
        // Singleton: se já existe uma instância persistente, destroi esta nova.
        foreach (var other in FindObjectsOfType<DataPlayerPosition>())
        {
            if (other != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        // Instância sobrevivente — persiste e registra a referência.
        DontDestroyOnLoad(gameObject);
        PlayerTransform = transform;
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        LoadGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive) return;

        SaveGame();
        sceneName = scene.name;
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat(KeyX, transform.position.x);
        PlayerPrefs.SetFloat(KeyY, transform.position.y);
        PlayerPrefs.SetFloat(KeyZ, transform.position.z);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(KeyX))
        {
            float x = PlayerPrefs.GetFloat(KeyX);
            float y = PlayerPrefs.GetFloat(KeyY);
            float z = PlayerPrefs.HasKey(KeyZ) ? PlayerPrefs.GetFloat(KeyZ) : transform.position.z;
            transform.position = new Vector3(x, y, z);
        }
    }
}
