using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
    public Transform optionsParent;
    public GameObject optionButtonPrefab;

    private DialogueNode currentNode;

    public void StartDialogue(DialogueNode dialogue)
    {
        dialoguePanel.SetActive(true);
        ShowNode(dialogue);
    }

    void ShowNode(DialogueNode node)
    {
        currentNode = node;

        dialogueText.text = node.dialogueText;
        characterImage.sprite = node.speakerSprite;

        foreach (Transform child in optionsParent)
            Destroy(child.gameObject);

        foreach (DialogueOption option in node.options)
        {
            GameObject btn = Instantiate(optionButtonPrefab, optionsParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = option.optionText;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (option.nextDialogue != null)
                    ShowNode(option.nextDialogue);
                else
                    EndDialogue();
            });
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
