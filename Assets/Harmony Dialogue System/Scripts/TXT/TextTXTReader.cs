using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HarmonyDialogueSystem
{
    [CreateAssetMenu()]
    public class TextTXTReader : TXTReader
    {
        public override bool ReadTXT(TextAsset textFile, out List<DialogueBlock> hashTags)
        {
            hashTags = new List<DialogueBlock>();
            string[] data = textFile.text.Split('\n');

            foreach (var line in data)
            {
                DialogueBlock dialogueBlock = new DialogueBlock();
                // Extract and add the hash tags to the hashTags list
                MatchCollection matches = Regex.Matches(line, @"#(\w+):(\w+)");
                foreach (Match match in matches)
                {
                    string key = match.Groups[1].Value.ToLower();
                    string value = match.Groups[2].Value;
                    dialogueBlock.characterTags.Add(key, value);
                }

                // Remove the hash tags from the line and add it to the sentences list
                string cleanedLine = Regex.Replace(line, @"#\w+:\w+", "").Trim();
                dialogueBlock.dialogueLine = cleanedLine;
                hashTags.Add(dialogueBlock);
            }

            return true;
        }
    }
}
