using UnityEngine;

/// <summary>
/// Controla o dialogo especifico de um NPC.
/// Alem do comportamento base, este NPC libera um objeto trancado ao confirmar ajuda.
/// </summary>
public class DialogueAleNPC : SimpleDialogue
{
    [Header("Carlos´s Specific Logic")]
    [Tooltip("Referencia ao objeto trancado no mundo que sera desbloqueado.")]
    [SerializeField] private LockObjectInteraction lockObject;

    //Executa o comportamento base de confirmacao de ajuda e desbloqueia o objeto associado
    public override void ConfirmHelp()
    {
        base.ConfirmHelp();

        //Libera a trava do objeto, caso a referencia exista no Inspector
        if(lockObject != null)
        {
            lockObject.isUnlocked = true;
            lockObject = null;
        }
        else
        {
            Debug.LogWarning("Nenhuma referencia a LockObjectInteraction!");
        }
    }
}