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

    public static bool dialogueBossFinished = false;

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

    public void DialogueView(DialogueNodeBoss node)
    {
        dialogueCurrent = node;

        if (panelDialogue != null)
        {
            panelDialogue.SetActive(true);
        }

        if (questionText != null)
        {
            questionText.gameObject.SetActive(true);

            if (questionText.gameObject.activeInHierarchy)
            {
                writeMachine.Run(node.question, questionText);
            }
            else
            {
                questionText.text = node.question;
                Debug.LogWarning("Objeto 'Text' ainda está inativo na hierarquia. Corrotina não iniciada.");
            }
        }

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
                buttonOption[i].interactable = true;
            }
            else
            {
                buttonOption[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickDone()
    {
        dialogueBossFinished = true;
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
        if (dialogueCurrent.nextDialogue != null && dialogueCurrent.nextDialogue.Length > 0)
        {
            DialogueNodeBoss next;

            if (dialogueCurrent.nextDialogue.Length == 1)
            {
                next = dialogueCurrent.nextDialogue[0];
            }
            else
            {
                next = dialogueCurrent.nextDialogue[index];
            }

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

    public bool CurrentNodeHasOptions()
    {
        if (dialogueCurrent == null) return false;
        return dialogueCurrent.HasOptions();
    }
}
