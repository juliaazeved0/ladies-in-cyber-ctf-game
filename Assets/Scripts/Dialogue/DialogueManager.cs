using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public Button buttonExit;
    public Image lockImage;
    public TextMeshProUGUI dialogueNPC;

    public WriteMachine writeMachine;

    public PlayerNameplate playerNameplate;
    public const string INICIAL_KEY = "dialogueInicial";

    [Header("Nodes")]
    public DialogueNode firstNode; 
    private DialogueNode dialogueCurrent; 

    [Header("UI do Narrador")]
    public GameObject panelNarrator;
    public TextMeshProUGUI textNarrator;
    public Button buttonNextNarrator;

    [Header("Control inventory")]
    public static bool isDialogueActive = false;

    [Header("Objetos do Mapa")]
    public GameObject lockLadder; // Variável renomeada
    
    private DialogueNode pendingNextNode;


    void Start()
    {
        panelDialogue.SetActive(false);

        if(panelNarrator != null) panelNarrator.SetActive(false);
        if(buttonNextNarrator != null) buttonNextNarrator.onClick.AddListener(OnClickNextNarrator);

        buttonExit.gameObject.SetActive(false);

        int dialogueInicialDone = PlayerPrefs.GetInt(INICIAL_KEY, 0);

        if (dialogueInicialDone == 1)
        {
            lockImage.gameObject.SetActive(false);
            
            // Verificação adicionada: Mantém a escada/bloqueio desativado se já tiver o progresso
            if (lockLadder != null) 
            {
                lockLadder.SetActive(false);
            }
        }
        
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
            isDialogueActive = true; // Tranca o inventário ao iniciar o diálogo

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
        writeMachine.Run(node.question, questionText);

        bool isLastNode = (node.nextDialogue.Length == 0);

        if(isLastNode)
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
        isDialogueActive = false; 

        PlayerPrefs.SetInt(INICIAL_KEY, 1);
        PlayerPrefs.Save();

        panelDialogue.SetActive(false);
        miniMapCanvas.SetActive(true);
        cameraMiniMap.SetActive(true);
    
        if(lockImage != null) lockImage.gameObject.SetActive(false);
        
        dialogueNPC.text = "Bem-vinda ao Centro de Tecnologia do Itaipu Parquetec!";
        
        if(playerNameplate != null) playerNameplate.SetNameplateIdPlayer();
    
        // Atualizado com o novo nome da variável e proteção extra
        if (lockLadder != null) lockLadder.SetActive(false);
    }

    public void OnClickExit()
    {
        isDialogueActive = false; // Destranca o inventário se o jogador sair no meio

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

        if (dialogueCurrent.narratorFeedbacks != null && index < dialogueCurrent.narratorFeedbacks.Length)
        {
            textNarrator.text = dialogueCurrent.narratorFeedbacks[index];
        }

        for (int i = 0; i < buttonOption.Length; i++)
        {
            buttonOption[i].gameObject.SetActive(false);
        }

        panelNarrator.SetActive(true);
    }

    public void OnClickNextNarrator()
    {
        panelNarrator.SetActive(false);
        if (pendingNextNode != null)
        {
            DialogueView(pendingNextNode);
        }
        else
        {
            isDialogueActive = false; //Destranca se o diálogo acabar pelo narrador

            panelDialogue.SetActive(false);
            miniMapCanvas.SetActive(true);
            cameraMiniMap.SetActive(true);
        }
    }
}