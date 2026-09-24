using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia a exibicao e navegacao de dialogos em arvore (DialogueNodeBoss)
/// na sala do boss, controlando textos, opcoes, botoes de finalizacao e o
/// efeito de digitacao via WriteMachine.
/// </summary>
public class DialogueManagerBoss : MonoBehaviour
{
    [Header("UI elements")]
    public GameObject panelDialogue;
    public TextMeshProUGUI questionText;
    public Image characterNPC;
    public Button[] buttonOption;
    public Button buttonPlayAgain;
    public TextMeshProUGUI playerNameText;
    public Button buttonDone;
    public Button buttonExit;
    public TextMeshProUGUI dialogueNPC;

    public WriteMachine writeMachine;

    [Header("Nodes")]
    public DialogueNodeBoss firstNode;
    private DialogueNodeBoss dialogueCurrent;

    //Representa se o dialogo do boss ja foi concluido em toda a sessao de jogo
    public static bool dialogueBossFinished = false;

    void Start()
    {
        //Evita NullReferenceException logo no primeiro frame caso referencias
        //obrigatorias nao tenham sido arrastadas no Inspector
        if(panelDialogue == null || buttonExit == null)
        {
            Debug.LogError($"{gameObject.name} está sem referências obrigatórias!");
            enabled = false;
            return;
        }

        panelDialogue.SetActive(false);
        buttonExit.gameObject.SetActive(false);

        if(playerNameText != null)
        {
            playerNameText.text = "JOGADORA";
        }
    }

    /// <summary>
    /// Inicia o dialogo a partir do no configurado em firstNode,
    /// reabrindo o painel e escondendo o botao de "jogar novamente".
    /// </summary>
    public void StartDialogue()
    {
        if(firstNode != null)
        {
            panelDialogue.SetActive(true);
            buttonPlayAgain.gameObject.SetActive(false);
            DialogueView(firstNode);
        }
        else
        {
            Debug.LogWarning("Erro: firstNode não foi arrastado no Inspector!");
        }
    }

    /// <summary>
    /// Exibe um no especifico do dialogo: atualiza o texto, configura os botoes 
    /// de finalizacao apropriados e popula os botoes de opcao disponiveis.
    /// </summary>
    public void DialogueView(DialogueNodeBoss node)
    {
        dialogueCurrent = node;

        if(panelDialogue != null)
        {
            panelDialogue.SetActive(true);
        }

        if(questionText != null)
        {
            questionText.gameObject.SetActive(true);

            //A coroutine de digitacao so funciona corretamente se o objeto ja estiver ativo na hierarquia
            if(questionText.gameObject.activeInHierarchy)
            {
                writeMachine.Run(node.question, questionText);
            }
            else //Caso contrario, define o texto diretamente sem animacao
            {
                questionText.text = node.question;
                Debug.LogWarning("Objeto 'Text' ainda está inativo na hierarquia. Corrotina não iniciada!");
            }
        }

        //Um no eh considerado "final" quando nao possui proximos dialogos configurados
        bool isLastNode = (node.nextDialogue == null || node.nextDialogue.Length == 0);

        if(isLastNode)
        {
            buttonPlayAgain.gameObject.SetActive(false);
            buttonDone.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(false);

            //O tipo de botao exibido no fim do dialogo depende de uma configuracao
            //propria do no, permitindo que diferentes ramos terminem de formas diferentes
            if(node.buttonType == ButtonType.PlayAgain)
            {
                buttonPlayAgain.gameObject.SetActive(true);
            }
            else if(node.buttonType == ButtonType.Done)
            {
                buttonDone.gameObject.SetActive(true);
            }
            else
            {
                buttonExit.gameObject.SetActive(true);
            }
        }
        else
        {
            //Sempre mostra o botao de saida, ja que a conversa ainda pode continuar
            buttonDone.gameObject.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(true);
        }

        //Popula dinamicamente os botoes de opcao com base na quantidade de opcoes do no atual
        for(int i = 0; i < buttonOption.Length; i++)
        {
            if(i < node.options.Length)
            {
                buttonOption[i].gameObject.SetActive(true);
                buttonOption[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i];
                buttonOption[i].interactable = true;
            }
            else
            {
                buttonOption[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Marca o dialogo do boss como concluido globalmente
    /// (afetando outros scripts que checam essa flag) e
    /// fecha o painel.
    /// </summary>
    public void OnClickDone()
    {
        dialogueBossFinished = true;
        panelDialogue.SetActive(false);
    }

    //Apenas fecha o painel, sem marcar o dialogo como concluido
    public void OnClickExit()
    {
        panelDialogue.SetActive(false);
    }

    //Reinicia o dialogo desde o firstNode
    public void DialoguePlayAgain()
    {
        StartDialogue();
    }

    /// <summary>
    /// Avanca o dialogo com base na opcao escolhida pelajogadora.
    /// Se o no atual tiver apenas um proximo dialogo, avanca
    /// direto para ele, ignorando o indice.
    /// </summary>
    public void ChooseOption(int index)
    {
        //Evita NullReferenceException caso esse metodo seja chamado antes de qualquer
        //no de dialogo ter sido exibido
        if(dialogueCurrent == null)
        {
            Debug.LogWarning("ChooseOption chamado, mas não há um nó de diálogo atual definido!");
        }

        if(dialogueCurrent.nextDialogue != null && dialogueCurrent.nextDialogue.Length > 0)
        {
            DialogueNodeBoss next;

            if(dialogueCurrent.nextDialogue.Length == 1)
            {
                next = dialogueCurrent.nextDialogue[0];
            }
            else
            {
                next = dialogueCurrent.nextDialogue[index];
            }

            if(next != null)
            {
                DialogueView(next);
            }
            else
            {
                panelDialogue.SetActive(false);
            }
        }
        else
        {
            panelDialogue.SetActive(false);
        }
    }

    /// <summary>
    /// Indica se o no de dialogo atual possui opcoes de escolha, para
    /// decidir se a interacao deve avancar automaticamente ou aguardar
    /// clique do mouse em uma opcao.
    /// </summary>
    public bool CurrentNodeHasOptions()
    {
        if(dialogueCurrent == null) return false;
        return dialogueCurrent.HasOptions();
    }
}