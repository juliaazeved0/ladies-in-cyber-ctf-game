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
    public GameObject miniMapCanvas;
    public GameObject cameraMiniMap;
    public Button buttonPlayAgain;
    public TextMeshProUGUI playerName;

    private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    [Header("Data")]
    public DialogueNode firstNode; 
    private DialogueNode dialogueCurrent; 

    void Start()
    {
        panelDialogue.SetActive(false);
        string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY);
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
            miniMapCanvas.SetActive(false);
            cameraMiniMap.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            DialogueView(firstNode);
        }
        else
        {
            Debug.LogWarning("inspector error");
        }
    }

    public void DialogueView(DialogueNode node)
    {
        dialogueCurrent = node;
        questionText.text = node.question;

        bool isLastNode = (node.nextDialogue.Length == 0);

        if(isLastNode)
        {
            buttonPlayAgain.gameObject.SetActive(true);
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

    public void DialoguePlayAgain()
    {
        StartDialogue();
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