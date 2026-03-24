using UnityEngine;
using System.Collections.Generic;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance;

    private HashSet<string> completedChallenges = new HashSet<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void CompleteChallenge(string challengeID)
    {
        completedChallenges.Add(challengeID);
    }

    public bool IsChallengeCompleted(string challengeID)
    {
        return completedChallenges.Contains(challengeID);
    }
}