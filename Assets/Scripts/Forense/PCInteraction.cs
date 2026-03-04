using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInteraction : MonoBehaviour
{
    private PulseOutline pulse;

    private void Start()
    {
        pulse = GetComponentInParent<PulseOutline>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pulse.StartPulsing();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pulse.StopPulsing();
        }
    }
}
