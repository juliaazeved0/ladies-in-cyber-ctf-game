using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject panelName;
    public PulseOutline pulseOutline;
    public void OnClickHelpCrypto()
    {
        CanvasManager.Instance.ClosedPanel(panelName.name);
        CanvasManager.Instance.ToggleMiniMap(true);
        pulseOutline.StartPulsing();
    }

    

    

}
