using UnityEngine;
using System.Collections.Generic;

public class InventoryUIHandler : MonoBehaviour
{
    [Header("Referências dos Ícones")]
    public GameObject[] iconesDasFlags; // Arraste aqui os GameObjects das flags que ficam na bolsa

    // O OnEnable roda toda vez que o painel é ativado (SetEnable(true))
    void OnEnable()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        // 1. Primeiro, escondemos tudo para garantir uma "limpeza"
        foreach (GameObject icone in iconesDasFlags)
        {
            icone.SetActive(false);
        }

        // 2. Agora, perguntamos ao FlagManager quais flags o jogador tem
        // Se o seu FlagManager não for mais Singleton (Instance), 
        // você pode usar FindObjectOfType<FlagManager>()
        FlagManager fm = FindObjectOfType<FlagManager>();

        if (fm != null)
        {
            foreach (string flagName in fm.flagsCapture)
            {
                // Aqui você ativa o ícone que tem o mesmo nome da flag guardada
                foreach (GameObject icone in iconesDasFlags)
                {
                    if (icone.name == flagName) 
                    {
                        icone.SetActive(true);
                    }
                }
            }
        }
    }
}