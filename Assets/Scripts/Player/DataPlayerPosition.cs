using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia a persistencia da posicao da jogadora entre cenas e sessoes de jogo.
/// Utiliza o padrao Singleton para garantir uma unica instancia global.
/// </summary>
[DefaultExecutionOrder(-10)]
public class DataPlayerPosition : MonoBehaviour
{
    //Referencia estatica para o Transform da jogadora, acessivel globalmente
    public static Transform PlayerTransform { get; private set; }

    private string sceneName;

    //Chaves de acesso para o PlayerPrefs baseadas no nome da cena
    private string KeyX => sceneName + "_PlayerX";
    private string KeyY => sceneName + "_PlayerY";
    private string KeyZ => sceneName + "_PlayerZ";

    void Awake()
    {
        //Implementacao de Singleton manual para garantir unicidade
        foreach(var other in FindObjectsOfType<DataPlayerPosition>())
        {
            if(other != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        //Configuracao da instancia persistente
        DontDestroyOnLoad(gameObject);
        PlayerTransform = transform;
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        LoadGame();
        SceneManager.sceneLoaded += OnSceneLoaded; //Carregamento de cena para atualizar a posicao
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //Evita vazamento de memoria
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    /// <summary>
    /// Evento disparado sempre que uma nova cena eh carregada.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode == LoadSceneMode.Additive) return;

        SaveGame(); //Salva a posicao da cena anterior
        sceneName = scene.name;
        LoadGame(); //Carrega a posicao (se existir) na nova cena
    }

    /// <summary>
    /// Salva as coordenadas atuais da jogadora no PlayerPrefs.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetFloat(KeyX, transform.position.x);
        PlayerPrefs.SetFloat(KeyY, transform.position.y);
        PlayerPrefs.SetFloat(KeyZ, transform.position.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carrega as coordenadas salvas e posiciona a jogadora.
    /// </summary>
    public void LoadGame()
    {
        if(PlayerPrefs.HasKey(KeyX))
        {
            float x = PlayerPrefs.GetFloat(KeyX);
            float y = PlayerPrefs.GetFloat(KeyY);
            float z = PlayerPrefs.HasKey(KeyZ) ? PlayerPrefs.GetFloat(KeyZ) : transform.position.z;

            transform.position = new Vector3(x, y, z);
        }
    }
}