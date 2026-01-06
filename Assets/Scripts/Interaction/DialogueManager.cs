using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI elements")]
    public GameObject panelDialogue;
    public TextMeshProUGUI questionText;
    public Image characterNPC;
    public Button[] buttonOption;

    [Header("Data")]
    public DialogueNode firstNode; 
    private DialogueNode dialogueCurrent; 

    void Start()
    {
        panelDialogue.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (firstNode != null)
        {
            panelDialogue.SetActive(true);
            DialogueView(firstNode);
        }
        else
        {
            Debug.LogWarning("Esqueceu de arrastar o First Node no Inspetor!");
        }
    }

    public void DialogueView(DialogueNode node)
    {
        dialogueCurrent = node;
        questionText.text = node.question;

     
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

    public void ChooseOption(int index)
    {
       
        DialogueNode next = dialogueCurrent.nextDialogue[index];

        if (next != null)
        {
            DialogueView(next);
        }
        else
        {
            panelDialogue.SetActive(false);
        }
    }
}