using UnityEngine;

/// <summary>
/// Adiciona o efeito de pulsacao e bloqueia a interacao enquanto
/// o dialogo do boss ainda nao tiver sido concluido.
/// </summary>
public class ComputerBossInteraction : ObjectInteractionBoss
{
    [Header("Visual Effects")]
    [Tooltip("Script responsavel pelo efeito de brilho no objeto.")]
    public PulseOutline scriptPulse;

    protected override void Start()
    {
        base.Start();

        //Painel de desafio e pulsacao escondidos
        if(challengePanel != null) challengePanel.SetActive(false);
        if(scriptPulse != null) scriptPulse.StopPulsing();
    }

    protected override void Update()
    {
        //Enquanto o dialogo com o boss nao tiver terminado, a interacao fica completamente bloqueada
        if(!DialogueManagerBoss.dialogueBossFinished)
        {
            if(interactionNotice != null && interactionNotice.activeSelf)
                interactionNotice.SetActive(false);

            if(scriptPulse != null)
                scriptPulse.StopPulsing();

            return;
        }

        //Dialogo concluido
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        //So inicia a pulsacao se o dialogo ja tiver terminado
        if(collision.CompareTag("Player") && DialogueManagerBoss.dialogueBossFinished)
        {
            if(scriptPulse != null) scriptPulse.StartPulsing();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(collision.CompareTag("Player"))
        {
            if(scriptPulse != null) scriptPulse.StopPulsing();
        }
    }

    /// <summary>
    /// Sobrescreve a interacao da classe base para tambem acionar a 
    /// pulsacao antes de delegar o restante do comportamento.
    /// </summary>
    protected override void Interact()
    {
        if(scriptPulse != null) scriptPulse.StartPulsing();

        base.Interact();

        Debug.Log("Interação realizada com sucesso!");
    }
}