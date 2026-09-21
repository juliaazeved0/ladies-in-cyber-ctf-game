using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gerenciador centralizado dos desafios do jogo.
/// Mantem o estado de quais desafios foram concluidos usando um conjunto unico (HashSet).
/// </summary>
public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance;
    private HashSet<string> completedChallenges = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
    }

    //Marca um desafio como concluido
    public void CompleteChallenge(string challengeID)
    {
        completedChallenges.Add(challengeID);
    }

    //Verifica se um desafio especifico ja foi concluido
    public bool IsChallengeCompleted(string challengeID)
    {
        return completedChallenges.Contains(challengeID);
    }
}