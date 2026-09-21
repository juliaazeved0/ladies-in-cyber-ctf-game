using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gerencia o estado de conclusao dos desafios no jogo utilizando um padrao Singleton.
/// </summary>
public class ChallengeManager : MonoBehaviour
{
    //Instancia unica do ChallengeManager no jogo
    public static ChallengeManager Instance;

    [Header("State")]
    [Tooltip("Conjunto com os IDs de todos os desafios ja concluidos.")]
    private HashSet<string> completedChallenges = new HashSet<string>();

    private void Awake()
    {
        //Garante que existe apenas uma instancia do ChallengeManager na cena
        if(Instance != null && Instance != this)
        {
            Debug.LogWarning($"[ChallengeManager] Instância duplicada encontrada em {gameObject.name}. Destruindo objeto.", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //Marca um desafio como conluido utilizando seu ID unico.
    public void CompleteChallenge(string challengeID)
    {
        if(string.IsNullOrEmpty(challengeID))
        {
            Debug.LogWarning($"[ChallengeManager] Tentativa de concluir um desafio com ID nulo ou vazio em {gameObject.name}.", this);
            return;
        }

        completedChallenges.Add(challengeID);
    }

    //Verifica se um desafio especifico ja foi concluido
    public bool IsChallengeCompleted(string challengeID)
    {
        return completedChallenges.Contains(challengeID);
    }
}