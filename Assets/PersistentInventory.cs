using UnityEngine;

public class PersistentInventory : MonoBehaviour
{
    public static PersistentInventory Instance;

    void Awake()
    {
        // Padrão Singleton para garantir que só exista UM inventário
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se mudar de cena e já existir um inventário, destrói o novo para não duplicar
            Destroy(gameObject);
        }
    }
}