using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string question;
    public string[] options;
    public DialogueNode[] nextDialogue;

}
