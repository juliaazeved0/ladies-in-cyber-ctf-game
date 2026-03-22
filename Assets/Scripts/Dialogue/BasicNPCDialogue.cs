using UnityEngine;

public class BasicNPCDialogue : SimpleDialogue 
{
    public override void NextTalk()
    {
        if (dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
        else 
        {
            ExitDialogue();
        }
    }

    public override void ConfirmHelp()
    {
        ExitDialogue(); 
    }
}