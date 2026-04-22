using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerBossInteraction : ObjectInteractionBoss
{
    [Header("Efeitos Visuais")]
    public PulseOutline scriptPulse;

    protected override void Start()
    {
        //Chama o Start da classe base (ObjectInteractionBoss)
        base.Start();

        //Garante que o desafio comece fechado e sem brilho
        if (challengePanel != null) challengePanel.SetActive(false);
        if (scriptPulse != null) scriptPulse.StopPulsing();
    }

    protected override void Update()
    {
        //Se o diálogo não terminou, garantimos que o brilho e aviso fiquem desligados
        if (!DialogueManagerBoss.dialogueBossFinished)
        {
            if (interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);

            if (scriptPulse != null)
                scriptPulse.StopPulsing();

            return;
        }

        //Se o diálogo terminou, a classe base (ObjectInteractionBoss), cuida de mostrar o "interactionNotice" e detectar a tecla E
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player") && DialogueManagerBoss.dialogueBossFinished)
        {
            if (scriptPulse != null) scriptPulse.StartPulsing();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (collision.CompareTag("Player"))
        {
            if (scriptPulse != null) scriptPulse.StopPulsing();
        }
    }

    //Este método é chamado pela classe base quando o player aperta E
    protected override void Interact()
    {
        //Ativa o brilho (caso não esteja) e chama a lógica de abrir painel da base
        if (scriptPulse != null) scriptPulse.StartPulsing();

        base.Interact();

        Debug.Log("[ComputerBoss] Interação realizada com sucesso.");
    }
}