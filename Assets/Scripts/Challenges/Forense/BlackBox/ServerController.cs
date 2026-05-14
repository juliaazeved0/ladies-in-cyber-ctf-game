using UnityEngine;

/// <summary>
/// Controla a logica do Servidor no desafio Black Box.
/// Gerencia estados de bloqueio visual, interacao via hackeamento e conclusao do puzzle.
/// </summary>
public class ServerController : LockObjectInteraction
{
    [Header("Visual State Indicator")]
    [Tooltip("Referencia ao icone do cadeado fechado.")]
    public SpriteRenderer lockClosed;

    [Tooltip("Referencia ao icone do cadeado aberto.")]
    public SpriteRenderer lockOpened;

    [Header("Server Challenges")]
    [Tooltip("Indica se o desafio interno do servidor foi finalizado.")]
    public bool isFinished = false;

    [Tooltip("Painel UI para a etapa de conexao de cabos.")]
    public GameObject connectionPanel;

    /// <summary>
    /// Inicializa o servidor bloqueado e esconde os paineis de desafio.
    /// </summary>
    new void Start()
    {
        //O servidor inicia bloqueado e nao finalizado
        isUnlocked = false;
        isFinished = false;

        //Configuracao visual inicial dos cadeados
        if(lockClosed != null) lockClosed.gameObject.SetActive(true);
        if(lockOpened != null) lockOpened.gameObject.SetActive(false);

        //Garante que as interfaces de desafio comecem escondidas
        if(challengePanel != null) challengePanel.gameObject.SetActive(false);
        if(connectionPanel != null) connectionPanel.SetActive(false);
    }

    protected override void Update()
    {
        //Executa a logica base de proximidade e deteccao de tecla
        base.Update();
    }

    /// <summary>
    /// Chamado externamente apos o sucesso no hacking via PC.
    /// </summary>
    public void UnlockByHacking()
    {
        isUnlocked = true;

        //Atualiza o estado visual para "desbloqueado"
        if(lockClosed != null) lockClosed.gameObject.SetActive(false);
        if(lockOpened != null) lockOpened.gameObject.SetActive(true);

        Debug.Log("[ServerController] Servidor desbloqueado via hacking.");
    }

    /// <summary>
    /// Sobrescreve a interacao para lidar com as duas fases do servidor
    /// </summary>
    protected override void Interact()
    {
        //Fase 2: Conexao de cabos (se o desafio ja foi concluido)
        if(isFinished)
        {
            if(connectionPanel != null)
            {
                connectionPanel.SetActive(true);
                HideInteractionNotice();
            }
            return;
        }
        
        //Fase 1: Desafio do servidor (so acessivel se desbloqueado pelo hacking)
        if(isUnlocked)
        {
            if(challengePanel != null)
            {
                challengePanel.SetActive(true);
                HideInteractionNotice();
            }
        }
        else
        {
            Debug.Log("[ServerController] O servidor ainda esta bloqueado.");
        }
    }

    /// <summary>
    /// Marca o desafio tecnico do servidor como concluido.
    /// </summary>
    public void CompleteServer()
    {
        isFinished = true;
        Debug.Log("[ServerController] Desafio do servidor concluido.");
    }

    public void HideInteractionNotice()
    {
        if(interactionNotice != null) interactionNotice.SetActive(false);
    }
}