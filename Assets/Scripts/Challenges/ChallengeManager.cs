using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gerenciador centralizado dos desafios do jogo.
/// Mantem o estado de quais desafios foram concluidos usando um conjunto unico (HashSet).
/// </summary>
public class ChallengeManager : MonoBehaviour
{
    //Instancia estatica para acesso global (Singleton)
    public static ChallengeManager Instance;

    //HashSet garante que cada ID de desafio exista apenas uma vez e permite busca rapida
    private HashSet<string> completedChallenges = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Marca um desafio como concluido.
    /// </summary>
    /// <param name="challengeID">Identificador unico do desafio.</param>
    public void CompleteChallenge(string challengeID)
    {
        completedChallenges.Add(challengeID);
    }

    /// <summary>
    /// Verifica se um desafio especifico ja foi concluido.
    /// </summary>
    /// <param name="challengeID">Identificador do desafio a ser checado.</param>
    /// <returns>Verdadeiro se concluido, falso caso contrario.</returns>
    public bool IsChallengeCompleted(string challengeID)
    {
        return completedChallenges.Contains(challengeID);
    }
}