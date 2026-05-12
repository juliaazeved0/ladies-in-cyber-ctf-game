using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Versao simplificada do sistema de dialogo sem funcionalidades de ajuda.
/// Foca apenas na progressao linear entre os nos de dialogo.
/// </summary>
public class SimpleDialogueNoHelp : SimpleDialogue
{
    /// <summary>
    /// Avanca para a proxima fala ou completa o texto atual se ainda estiver sendo digitado.
    /// </summary>
    public override void NextTalk()
    {
        //Se a maquina de escrever ainda estiver digitando, completa o texto instantaneamente
        if(writeMachine.IsTyping)
        {
            writeMachine.Complete();
            return;
        }

        //Se houver um proximo no configurado, avanca a visualizacao
        if(dialogueCurrent.nextNode != null)
        {
            DialogueView(dialogueCurrent.nextNode);
        }
    }
}