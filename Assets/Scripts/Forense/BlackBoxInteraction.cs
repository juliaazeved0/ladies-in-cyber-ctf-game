using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoxInteraction : ObjectInteraction
{
    protected override void Interact()
    {
        CanvasManager.Instance.ToggleMiniMap(false);

        CanvasManager.Instance.OpenPanel("PCBlackBoxChallenge");
    }
}
