using UnityEngine;

/// <summary>
/// Extende a funcionalidade de interacao para objetos que possuem um estado de bloqueio.
/// A interacao e o aviso visual so sao ativados quando o objeto esta destravado.
/// </summary>
public class LockObjectInteraction : ObjectInteraction
{
    [Header("Lock Settings")]
    [Tooltip("Define se o objeto esta liberado para interacao.")]
    public bool isUnlocked = false;

    /// <summary>
    /// Atualiza o estado visual do aviso de interacao com base na proximidade e no bloqueio.
    /// </summary>
    protected override void Update()
    {
       if(interactionNotice != null)
        {
            //O aviso so aparece se a player estiver perto E o objeto estiver destravado
            interactionNotice.SetActive(playerIsHere && isUnlocked);
        }

        base.Update();
    }

    /// <summary>
    /// Executa a interacao principal apenas se o objeto nao estiver mais bloqueado.
    /// </summary>
    protected override void Interact()
    {
        if(isUnlocked)
        {
            base.Interact();
        }
        else
        {
            Debug.Log($"[LockObjectInteraction] {gameObject.name} ainda esta trancado.");
        }
    }
}