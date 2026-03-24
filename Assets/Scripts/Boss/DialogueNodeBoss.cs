using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NodeBoss")]
public class DialogueNodeBoss : ScriptableObject
{
    public string question;
    [TextArea(3, 10)]
    public string[] options;

    public DialogueNodeBoss[] nextDialogue;

    public ButtonType buttonType;
}