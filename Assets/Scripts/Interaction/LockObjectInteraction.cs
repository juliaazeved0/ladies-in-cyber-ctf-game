
using UnityEngine;

public class LockObjectInteraction : ObjectInteraction
{
    [Header("Lock System")]
    public bool isUnlocked = false;

    protected override void Update()
    {
       if (interactionNotice != null)
        {
            interactionNotice.SetActive(playerIsHere && isUnlocked);
        }

        base.Update();
    }
    protected override void Interact()
    {
        if (isUnlocked)
        {
            base.Interact();
        }
        
    }

}