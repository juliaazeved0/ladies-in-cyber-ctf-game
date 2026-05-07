using UnityEngine;

/// <summary>
/// Dialogo especifico para um NPC.
/// Alem do comportamento base, este NPC libera um objeto trancado ao confirmar ajuda.
/// </summary>
public class DialogueAleNPC : SimpleDialogue
{
    [Header("Carlos´s Specific Logic")]
    public LockObjectInteraction lockObject;

    /// <summary>
    /// Sobrescreve o metodo de confirmacao para incluir a logica de destravar objetos.
    /// </summary>
    public override void ConfirmHelp()
    {
        base.ConfirmHelp(); //Executa a logica padrao definida no SimpleDialogue

        if(lockObject != null)
        {
            lockObject.isUnlocked = true;
            lockObject = null; 
        }
    }
}