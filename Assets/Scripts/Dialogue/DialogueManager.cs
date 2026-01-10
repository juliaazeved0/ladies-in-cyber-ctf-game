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
    public TextMeshProUGUI playerNameText;
    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    public Button buttonDone;
    public Button exitDialogue;
   
    public BlockLevels disableLevels;
    public PlayerNameplate playerNameplate;

    [Header("Nodes")]
    public DialogueNode firstNode; 
    private DialogueNode dialogueCurrent; 


    void Start()
    {
        panelDialogue.SetActive(false);
        
        string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");

        if (playerNameText != null)
        {
            playerNameText.text = playerName.ToUpper();
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
            buttonPlayAgain.gameObject.SetActive(false);
            buttonDone.gameObject.SetActive(false);

            if (node.buttonType == ButtonType.PlayAgain)
            {
                buttonPlayAgain.gameObject.SetActive(true);
            }
            else if (node.buttonType == ButtonType.Done)
            {
                buttonDone.gameObject.SetActive(true);
            }
        }
        else
        {
            buttonDone.gameObject.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
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
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);
        playerNameplate.SetNameplateIdPlayer();
        disableLevels.DisableBlock();

    }

    public void OnClickExit()
    {
        panelDialogue.SetActive(false);
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);
        
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