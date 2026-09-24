using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia a persistencia da posicao da jogadora por cena.
/// </summary>
[DefaultExecutionOrder(-10)]
public class DataPlayerPosition : MonoBehaviour
{
    //Referencia estatica global para o Transform da jogadora, permitindo acesso rapido por outros scripts de forma otimizada
    public static Transform PlayerTransform { get; private set; }

    private string sceneName;

    private string KeyX => sceneName + "_PlayerX";
    private string KeyY => sceneName + "_PlayerY";
    private string KeyZ => sceneName + "_PlayerZ";

    /// <summary>
    /// Garante que apenas uma instancia desse objeto exista na sessao do jogo
    /// (Singleton) e configura a referencia estatica.
    /// </summary>
    void Awake()
    {
        foreach(var other in FindObjectsOfType<DataPlayerPosition>())
        {
            if(other != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
        PlayerTransform = transform;
        sceneName = SceneManager.GetActiveScene().name;
    }

    //Carrega a posicao salva ao iniciar a cena e inscreve o evento de mudanca de cena
    void Start()
    {
        LoadGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Remove a inscricao do evento de cena para evitar vazamento de memoria
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Salva automaticamente os dados ao fechar a aplicacao
    void OnApplicationQuit()
    {
        SaveGame();
    }

    //Chamado automaticamente quando uma nova cena eh carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Ignora o carregamento aditivo para nao sobrescrever os dados da cena principal
        if(mode == LoadSceneMode.Additive) return;

        SaveGame();
        sceneName = scene.name;
        LoadGame();
    }

    //Salva as coordenadas X, Y e Z da posicao atual da jogadora no PlayerPrefs
    public void SaveGame()
    {
        PlayerPrefs.SetFloat(KeyX, transform.position.x);
        PlayerPrefs.SetFloat(KeyY, transform.position.y);
        PlayerPrefs.SetFloat(KeyZ, transform.position.z);
        PlayerPrefs.Save();
    }

    //Carrega as coordenadas X, Y e Z salvas para a cena atual e aplica a posicao do objeto
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