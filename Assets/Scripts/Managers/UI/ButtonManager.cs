using UnityEngine;

/// <summary>
/// Gerencia comportamentos genericos de botoes na interface.
/// </summary>
public class ButtonManager : MonoBehaviour
{
    public void FinalizarAgora()
    {
        gameObject.SetActive(false); //Desativa o objeto ao qual este script esta anexado
    }
}