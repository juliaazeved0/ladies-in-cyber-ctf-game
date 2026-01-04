using System;
using UnityEngine;
using UnityEngine.Events;

namespace HarmonyDialogueSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class DialogueTrigger3D : MonoBehaviour
    {
        [Header("Dialogue Triggers")]
        [Tooltip("If the system should use the Scriptbale Object Settings or the settings on the script")]
        [SerializeField] DialogueSettingType settingsType;

        [Tooltip("Settings for your Dialogue Trigger")]
        [SerializeField] private DialogueTriggerSettings dialogueTriggerSettings;

        [Tooltip("Add events that will be invoked when the player enters the trigger")]
        [SerializeField] UnityEvent dialogueEnterEvent;

        [Tooltip("Add events that will be invoked when the player exits the trigger")]
        [SerializeField] UnityEvent dialogueExitEvent;

        [Tooltip("The player tag string. Case Sensitive!")]
        [SerializeField] string playerTag;

        private bool playerInRange;

        [Header("Dialogue File")]
        [Tooltip("The file type you wish to use")]
        [SerializeField] FileTypeUsed fileTypeUsed;

        [SerializeField] private TextAsset inkJSON;

        [SerializeField] private TextAsset TXTFile;

        private void Awake()
        {
            playerInRange = false;
            if (DialogueManager.instance == null) throw new Exception("Dialogue Manager isn't present");

            DialogueManager.instance.dialogueSystemOnEvent += TurnOnExitEvents;

            if (settingsType == DialogueSettingType.UseDialogueObjectSettings && dialogueTriggerSettings != null)
            {
                //Sets these three to the Settings if they are null
                dialogueEnterEvent = dialogueTriggerSettings.dialogueEnterEvent;

                dialogueExitEvent = dialogueTriggerSettings.dialogueExitEvent;

                if (string.IsNullOrEmpty(playerTag)) playerTag = dialogueTriggerSettings.playerTag;

                fileTypeUsed = dialogueTriggerSettings.fileTypeUsed;

                inkJSON = dialogueTriggerSettings.inkJSON;

                TXTFile = dialogueTriggerSettings.TXTFile;
            }
        }

        private void Update()
        {
            if (playerInRange && InputManagerDialogue.isInteracting())
            {
                DialogueManager.instance.EnterDialogueMode(ReturnTextAsset(), fileTypeUsed);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = true;
                TurnOnEnterEvents();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = false;
                TurnOnExitEvents();
            }
        }

        private void TurnOnEnterEvents()
        {
            dialogueEnterEvent?.Invoke();
        }

        private void TurnOnExitEvents()
        {
            dialogueExitEvent?.Invoke();
        }

        /// <summary>
        /// Returns the Proper File
        /// </summary>
        /// <returns>The File to be used</returns>
        /// <summary>
        /// Returns the Proper File
        /// </summary>
        /// <returns>The File to be used</returns>
        private TextAsset ReturnTextAsset()
        {
#if USE_INK
            if (fileTypeUsed == FileTypeUsed.Ink)
            {
                if (inkJSON == null) Debug.LogWarning("No INK Text Asset File Added");
                return inkJSON;
            }
            else
            {
                if (TXTFile == null) Debug.LogWarning("No TXT Text Asset File Added");
                fileTypeUsed = FileTypeUsed.TXT;
                return TXTFile;
            }

#else
            if (TXTFile == null) Debug.LogWarning("No TXT Text Asset File Added");
            fileTypeUsed = FileTypeUsed.TXT;
            return TXTFile;
#endif
        }
    }
}
