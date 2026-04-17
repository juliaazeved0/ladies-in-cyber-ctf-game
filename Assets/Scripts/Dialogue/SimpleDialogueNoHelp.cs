using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogueNoHelp : SimpleDialogue
{
    public override void NextTalk()
    {
        if (writeMachine.IsTyping)
        {
            writeMachine.Complete();
            return;
        }

        if (dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
    }
}
