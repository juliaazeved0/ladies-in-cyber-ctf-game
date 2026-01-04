using UnityEngine;
using UnityEngine.Events;

namespace HarmonyDialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueSettings", menuName = "Settings/DialogueSettings")]
    public class DialogueTriggerSettings : ScriptableObject
    {
        [Tooltip("Add events that will be invoked when the player enters the trigger")]
        public UnityEvent dialogueEnterEvent;

        [Tooltip("Add events that will be invoked when the player exits the trigger")]
        public UnityEvent dialogueExitEvent;

        [Tooltip("The player tag string. Case Sensitive!")]
        public string playerTag;


        [Header("Dialogue File")]
        [Tooltip("The file type you wish to use")]
        public FileTypeUsed fileTypeUsed;

        public TextAsset inkJSON;

        public TextAsset TXTFile;
    }
}
