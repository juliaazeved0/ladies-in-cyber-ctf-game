using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Cole as flags em texto puro aqui")]
    public string[] baseGenerator;

    private int key = 14;

    [ContextMenu("Generate baseSafe")]
    public void EncryptBase()
    {
        // Variável para juntar todas as linhas num texto só
        string todasAsFlagsJuntas = "";

        for (int i = 0; i < baseGenerator.Length; i++)
        {
            string flagAtual = baseGenerator[i];
            string arrayNumbers = $"public static readonly int[] flag_{i} = new int[] {{ ";
            
            foreach (char letter in flagAtual)
            {
                arrayNumbers += (letter + key) + ", ";
            }
            
            // Adiciona a linha finalizada e dá um "Enter" (\n) para a próxima
            arrayNumbers += "};\n"; 
            
            todasAsFlagsJuntas += arrayNumbers;
        }

        // Cospe o blocão inteiro de uma vez só no console!
        Debug.Log(todasAsFlagsJuntas);
    }
}