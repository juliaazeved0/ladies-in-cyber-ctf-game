using UnityEngine;
using UnityEngine.UI;
using TMPro; //Biblioteca para usar TextMeshPro
using UnityEngine.SceneManagement; //Biblioteca para utilizar a troca de cenas

public class DialogueManagerBoss : MonoBehaviour
{
    [Header("UI elements")] //Elementos de UI: painéis, textos, imagens e botões
    public GameObject panelDialogue;
    public TextMeshProUGUI questionText;
    public Image characterNPC;
    public Button[] buttonOption;
    public Button buttonPlayAgain;
    public TextMeshProUGUI playerNameText;
    public Button buttonDone;
    public Button buttonExit;
    public TextMeshProUGUI dialogueNPC;

    public WriteMachine writeMachine; //Script responsável pelo efeito de digitação

    [Header("Nodes")]
    public DialogueNodeBoss firstNode; //Primeiro nó do diálogo
    private DialogueNodeBoss dialogueCurrent; //Nó atual sendo mostrado

    public static bool dialogueBossFinished = false; //Variável booleana para verificar se o diálogo com o boss foi finalizado

    void Start()
    {
        panelDialogue.SetActive(false); //Começa com o diálogo escondido
        buttonExit.gameObject.SetActive(false); //Esconde o botão de sair

        if (playerNameText != null)
        {
            playerNameText.text = "JOGADORA"; //Define nome padrão
        }
    }

    public void StartDialogue()
    {
        if (firstNode != null)
        {
            panelDialogue.SetActive(true); //Mostra o painel
            buttonPlayAgain.gameObject.SetActive(false); //Esconde o botão de replay
            DialogueView(firstNode); //Mostra o primeiro diálogo
        }
        else
        {
            Debug.LogWarning("Erro: firstNode não foi arrastado no Inspector!");
        }
    }

    public void DialogueView(DialogueNodeBoss node)
    {
        dialogueCurrent = node; //Armazena qual o diálogo atual

        if(panelDialogue != null)
        {
            panelDialogue.SetActive(true);  //Ativa o painel pai
        }

        if(questionText != null)
        {
            questionText.gameObject.SetActive(true); //Garante que o objeto do texto esteja ativo

            if(questionText.gameObject.activeInHierarchy) //Checa se o objeto e todos os seus pais estão ativos
            {
                writeMachine.Run(node.question, questionText); //Inicia a animação de digitação
            }
            else
            {
                //Se o objeto ainda estiver inativo por erro de hierarquia, o texto aparece para a jogadora ler
                questionText.text = node.question;
                Debug.LogWarning("Objeto 'Text' ainda está inativo na hierarquia. Corrotina não iniciada.");
            }
        }

        bool isLastNode = (node.nextDialogue == null || node.nextDialogue.Length == 0); //Checa se o próximo diálogo está vazio. Se estiver, é o fim da conversa

        if(isLastNode)
        {
            //Esconde as opções padrão para mostrar apenas os botões de conclusão
            buttonPlayAgain.gameObject.SetActive(false);
            buttonDone.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(false);

            //Verifica no ScriptableObject qual o tipo de encerramento foi configurado: jogar novamente, finalizado ou sair
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
            //Se ainda há mais falas, garante que os botões de conclusão fiquem escondidos
            buttonDone.gameObject.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(true);
        }

        for(int i = 0; i < buttonOption.Length; i++)
        {
            //Se o índice for menor que a quantidade de opções no nó, o botão é necessário
            if(i < node.options.Length)
            {
                buttonOption[i].gameObject.SetActive(true);
                buttonOption[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i]; //Atualiza o texto do botão
                buttonOption[i].interactable = true;
            }
            else
            {
                buttonOption[i].gameObject.SetActive(false); //Se o botão não tem texto definido, ele é escondido
            }
        }
    }

    public void OnClickDone()
    {
        dialogueBossFinished = true; //Marca que o diálogo foi concluído
        panelDialogue.SetActive(false); //Fecha o diálogo
    }

    public void OnClickExit()
    {
        panelDialogue.SetActive(false); //Apenas fecha
        //SceneManager.LoadScene("BossRoom"); //Retorna para o mapa do Boss
    }

    public void DialoguePlayAgain()
    {
        StartDialogue(); //Reinicia o diálogo
    }

    public void ChooseOption(int index)
    {
        if (dialogueCurrent.nextDialogue != null && dialogueCurrent.nextDialogue.Length > 0) //Verifica se há próximos diálogos
        {
            DialogueNodeBoss next;

            if(dialogueCurrent.nextDialogue.Length == 1)
            {
                next = dialogueCurrent.nextDialogue[0]; //Se só tem um, usa ele
            }
            else
            {
                next = dialogueCurrent.nextDialogue[index]; //Usa baseado na escolha
            }

            if (next != null)
            {
                DialogueView(next); //Vai para o próximo nó
            }
            else
            {
                panelDialogue.SetActive(false); //Fecha se não tiver
            }
        }
        else
        {
            panelDialogue.SetActive(false); //Fecha se não tiver próximos
        }
    }

    public bool CurrentNodeHasOptions() //Expõe se o node atual tem opções para que o outro script consiga consultar
    {
        if (dialogueCurrent == null) return false;
        return dialogueCurrent.HasOptions(); //Usa metodo do node
    }
}