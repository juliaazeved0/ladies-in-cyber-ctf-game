using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType { PlayAgain, Done }
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string question;
    public string[] options;
    public DialogueNode[] nextDialogue;

    public ButtonType buttonType;
}
