using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // Se ainda não existir uma instância, esta será a principal
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Impede que o objeto seja destruído na troca de cena
        }
        else
        {
            // Se já existir uma música a tocar, destrói a nova para não duplicar o som
            Destroy(gameObject);
        }
    }
}