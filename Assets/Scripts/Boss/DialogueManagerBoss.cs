using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        panelDialogue.SetActive(false);
        buttonExit.gameObject.SetActive(false);

        if (playerNameText != null)
        {
            playerNameText.text = "JOGADORA";
        }
    }

    public void StartDialogue()
    {
        if (firstNode != null)
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

    // AQUI ESTAVA O ERRO: Mudei de 'DialogueNode' para 'DialogueNodeBoss'
    public void DialogueView(DialogueNodeBoss node)
    {
        dialogueCurrent = node;
        writeMachine.Run(node.question, questionText);

        // Verificação de segurança para evitar erro de NullReference
        bool isLastNode = (node.nextDialogue == null || node.nextDialogue.Length == 0);

        if (isLastNode)
        {
            buttonPlayAgain.gameObject.SetActive(false);
            buttonDone.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(false);

            if (node.buttonType == ButtonType.PlayAgain)
            {
                buttonPlayAgain.gameObject.SetActive(true);
            }
            else if (node.buttonType == ButtonType.Done)
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
            buttonDone.gameObject.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(true);
        }

        for (int i = 0; i < buttonOption.Length; i++)
        {
            if (i < node.options.Length)
            {
                buttonOption[i].gameObject.SetActive(true);
                buttonOption[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i];
            }
            else
            {
                buttonOption[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickDone()
    {
        panelDialogue.SetActive(false);
    }

    public void OnClickExit()
    {
        panelDialogue.SetActive(false);
    }

    public void DialoguePlayAgain()
    {
        StartDialogue();
    }

    public void ChooseOption(int index)
    {
        // Outro ponto corrigido para garantir que use o array do Boss
        if (dialogueCurrent.nextDialogue != null && index < dialogueCurrent.nextDialogue.Length)
        {
            DialogueNodeBoss next = dialogueCurrent.nextDialogue[index];

            if (next != null)
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
}