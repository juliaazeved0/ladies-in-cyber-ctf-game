using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia o sistema de dialogos, incluindo a interface do usuario,
/// escolhas da jogadora e o progresso narrativo (Narrador e NPCs).
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image characterNPC;
    [SerializeField] private Button[] buttonOption;
    [SerializeField] private GameObject miniMapCanvas;
    [SerializeField] private GameObject cameraMiniMap;
    [SerializeField] private Button buttonPlayAgain;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button buttonDone;
    [SerializeField] private Button buttonExit;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private TextMeshProUGUI dialogueNPC;

    [Header("References")]
    [SerializeField] private WriteMachine writeMachine;
    [SerializeField] private PlayerNameplate playerNameplate;

    [Header("Dialogue Nodes")]
    [SerializeField] private DialogueNode firstNode;
    private DialogueNode dialogueCurrent;
    private DialogueNode pendingNextNode;

    [Header("Narrator UI")]
    [SerializeField] private GameObject panelNarrator;
    [SerializeField] private TextMeshProUGUI textNarrator;
    [SerializeField] private Button buttonNextNarrator;

    [Header("Game State")]
    public static bool isDialogueActive = false;

    [Header("Wordl Objects")]
    [SerializeField] private GameObject lockLadder;

    //Chaves de persistencia
    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    public const string INICIAL_KEY = "dialogueInicial";

    void Start()
    {
        panelDialogue.SetActive(false);

        if(panelNarrator != null) panelNarrator.SetActive(false);
        if(buttonNextNarrator != null) buttonNextNarrator.onClick.AddListener(OnClickNextNarrator);

        buttonExit.gameObject.SetActive(false);

        int dialogueInicialDone = PlayerPrefs.GetInt(INICIAL_KEY, 0);

        if(dialogueInicialDone == 1)
        {
            lockImage.gameObject.SetActive(false);
            
            if(lockLadder != null) 
            {
                lockLadder.SetActive(false);
            }
        }
        
        string playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Jogadora");

        if(playerNameText != null)
        {
            playerNameText.text = playerName.ToUpper();
        }
    }

    public void StartDialogue()
    {
        if(firstNode != null)
        {
            isDialogueActive = true;

            panelDialogue.SetActive(true);
            miniMapCanvas.SetActive(false);
            cameraMiniMap.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            DialogueView(firstNode);
        }
        else
        {
            Debug.LogWarning("Erro no Inspector!");
        }
    }


    public void DialogueView(DialogueNode node)
    {
        dialogueCurrent = node;
        writeMachine.Run(node.question, questionText);

        bool isLastNode = (node.nextDialogue.Length == 0);

        if(isLastNode)
        { 
            buttonPlayAgain.gameObject.SetActive(false);
            buttonDone.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(false);

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
            buttonDone.gameObject.SetActive(false);
            buttonPlayAgain.gameObject.SetActive(false);
            buttonExit.gameObject.SetActive(true);
        }

        for(int i = 0; i < buttonOption.Length; i++)
        {
            if(i < node.options.Length)
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
        isDialogueActive = false; 

        PlayerPrefs.SetInt(INICIAL_KEY, 1);
        PlayerPrefs.Save();

        panelDialogue.SetActive(false);
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);
    
        if(lockImage != null) lockImage.gameObject.SetActive(false);
        
        dialogueNPC.text = "Bem-vinda ao Centro de Tecnologia do Itaipu Parquetec!";
        
        if(playerNameplate != null) playerNameplate.SetNameplateIdPlayer();
        if(lockLadder != null) lockLadder.SetActive(false);
    }

    public void OnClickExit()
    {
        isDialogueActive = false; 

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
        pendingNextNode = dialogueCurrent.nextDialogue[index];

        if(dialogueCurrent.narratorFeedbacks != null && index < dialogueCurrent.narratorFeedbacks.Length)
        {
            textNarrator.text = dialogueCurrent.narratorFeedbacks[index];
        }

        for(int i = 0; i < buttonOption.Length; i++)
        {
            buttonOption[i].gameObject.SetActive(false);
        }

        panelNarrator.SetActive(true);
    }

    public void OnClickNextNarrator()
    {
        panelNarrator.SetActive(false);
        if(pendingNextNode != null)
        {
            DialogueView(pendingNextNode);
        }
        else
        {
            isDialogueActive = false; 

            panelDialogue.SetActive(false);
            miniMapCanvas.SetActive(true);
            cameraMiniMap.SetActive(true);
        }
    }
}