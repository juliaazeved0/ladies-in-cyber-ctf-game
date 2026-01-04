using System.Collections.Generic;
using UnityEngine;

namespace HarmonyDialogueSystem
{
    /// <summary>
    /// A block or a single line of dialogue. 
    /// It contains the dialogue line, a dictionary of character tags, and a list of choices.
    /// </summary>
    public class DialogueBlock
    {
        /// <summary>
        /// The actual line of dialogue that will be displayed or spoken by the character.
        /// </summary>
        public string dialogueLine;

        /// <summary>
        /// A dictionary of tags that provide additional information about the character or dialogue line.
        /// The key represents the tag type, and the value represents the tag value.
        /// </summary>
        public Dictionary<string, string> characterTags = new Dictionary<string, string>();

        /// <summary>
        /// A list of dialogue choices available after this line. Allows branching dialogue based on player choices.
        /// </summary>
        public HarmonyChoice choices;
    }

    public class HarmonyChoice
    {
        public static HarmonyChoice Default;
        public List<string> choices = new List<string>();
    }


    public class TXTReader : ScriptableObject
    {
        protected TextAsset textFile;

        public virtual bool ReadTXT(TextAsset _text, out List<DialogueBlock> dialogueBlocks)
        {
            textFile = _text;
            dialogueBlocks = new List<DialogueBlock>();
            if (textFile == null) return false;
            return true;
        }
    }
}