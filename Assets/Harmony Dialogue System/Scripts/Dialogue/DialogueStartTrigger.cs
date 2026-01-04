using UnityEngine;

namespace HarmonyDialogueSystem
{
    public class DialogueStartTrigger : MonoBehaviour
    {
        [Tooltip("The File type being used")]
        public FileTypeUsed fileTypeUsed;

        [Tooltip("The Dialogue File to use")]
        public TextAsset dialogueFile;

        private void Awake()
        {
            DialogueManager.instance.EnterDialogueMode(dialogueFile, fileTypeUsed);
        }
    }
}
