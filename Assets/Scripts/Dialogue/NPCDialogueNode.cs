using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "DialogueNPC/Node")]
public class NPCDialogueNode : ScriptableObject
{
    [TextArea(3,10)]

    public string talkNPC;

    public NPCDialogueNode nextNode;

    public Sprite characterNPC;
}
