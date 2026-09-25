using UnityEngine;

public class InteractionIntroduction : MonoBehaviour
{
    public bool playerIsHere;
    public const string INTRO_KEY = "introductionComplete";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) return;
        playerIsHere = true;
        LoadSceneIntroduction.TryLoadIntroduction();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) playerIsHere = false;
    }
}
