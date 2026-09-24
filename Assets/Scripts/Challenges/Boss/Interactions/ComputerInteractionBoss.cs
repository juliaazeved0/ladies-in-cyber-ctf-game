using UnityEngine;

/// <summary>
/// Especializacao de ObjectInteractionBoss para o "computador"
/// do Boss. Bloqueia a interacao e a pulsacao do contorno
/// enquanto o dialogo do Boss nao estiver finalizado.
/// </summary>
public class ComputerBossInteraction : ObjectInteractionBoss
{
    [Header("Visual Effects")]
    [Tooltip("Script responsavel pelo efeito de brilho no objeto.")]
    public PulseOutline pulseOutline;

    protected override void Start()
    {
        base.Start();

        if(challengePanel != null) challengePanel.SetActive(false);
        if(pulseOutline != null) pulseOutline.StopPulsing();
    }

    protected override void Update()
    {
        //Enquanto o dialogo do Boss nao terminar, a interacao fica bloqueada
        if(!DialogueManagerBoss.dialogueBossFinished)
        {
            //Esconde o aviso, para o outline e nao executa a logica da classe base
            if(interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);

            if(pulseOutline != null)
                pulseOutline.StopPulsing();

            return;
        }

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(collision.CompareTag("Player") && DialogueManagerBoss.dialogueBossFinished)
        {
            if(pulseOutline != null) pulseOutline.StartPulsing();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(collision.CompareTag("Player"))
        {
            if(pulseOutline != null) pulseOutline.StopPulsing();
        }
    }

    protected override void Interact()
    {
        if(pulseOutline != null) pulseOutline.StartPulsing();

        base.Interact();

        Debug.Log("Interação realizada com sucesso!");
    }
}