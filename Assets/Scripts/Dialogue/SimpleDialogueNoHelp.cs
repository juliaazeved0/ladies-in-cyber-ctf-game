using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogueNoHelp : SimpleDialogue
{
   public override void NextTalk()
    {
        if (dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
    }
}
