
using UnityEngine;

public class DialogueAleNPC : SimpleDialogue
{
    public LockObjectInteraction lockObject;
    public override void ConfirmHelp()
    {
        base.ConfirmHelp();
        if (lockObject != null)
        {
            lockObject.isUnlocked = true;
            lockObject = null; 
        }
    }
}