#if USE_INK
using Ink.Runtime;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HarmonyDialogueSystem
{
    [RequireComponent(typeof(ChoiceSystem), typeof(InputManagerDialogue))]
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;

        [Header("Dialogue UI")]
        [Tooltip("The major UI of the Dialogue")]
        [SerializeField] GameObject DialogueUI;

        [Tooltip("The Text that displays the Dialogue")]
        [SerializeField] TextMeshProUGUI dialogueText;

        [Tooltip("A prefab of how a Choice Button. This should have a Button and a TextMeshPro Text in it")]
        [SerializeField] private GameObject choicePrefab;

        [Tooltip("This is a group that holds the instantiated Choices")]
        [SerializeField] VerticalLayoutGroup choicesContainer;

        [Header("Default Settings")]
        [Tooltip("Text Asset Script to be used for TXT Conversion")]
        public TXTReader tXTReader; 

        [Tooltip("Amount of Seconds to wait before exiting the dialogue")]
        [SerializeField] private float secondsToWait;

        [Tooltip("The way the dialogue should progress. Either Automatically or by User Press. This doesn't affect Choice Dialogues")]
        [SerializeField] DialoguePassingType dialoguePassingType;

        [Tooltip("Amount of time between dialogue lines if Passing Mode is set to Automatic")]
        [Range(2f, 100f)]
        [SerializeField] float secondsToMoveTonextStory;

        private StoryObject currentStoryObject;

        List<GameObject> choiceList;

        public bool dialogueIsPlaying { get; private set; }

        [Tooltip("Event for when dialogue is starting")]
        public event Action dialogueSystemOnEvent;

        [Tooltip("Event for when dialogue is ending")]
        public event Action dialogueSystemOffEvent;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;

            ChoiceSystem.Instance.SetChoiceSystem(choicePrefab, choicesContainer);
        }

        /// <summary>
        /// Clears all events in the dialogue system on event
        /// </summary>
        public void ClearOnEvent()
        {
            dialogueSystemOnEvent = null;
        }

        /// <summary>
        /// Clears all events in the dialogue system off event
        /// </summary>
        public void ClearOffEvent()
        {
            dialogueSystemOffEvent = null;
        }

        private void Start()
        {
            dialogueIsPlaying = false;
            DialogueUI.SetActive(false);

            choiceList = new List<GameObject>();
        }

        public void StopDialogue()
        {
            dialogueIsPlaying = false;
            DialogueUI.SetActive(false);
        }

        private void Update()
        {
            if (!dialogueIsPlaying) return;
            else
            {
                if (InputManagerDialogue.isContinuing() && currentStoryObject.CanManuallyContinue() && (dialoguePassingType == DialoguePassingType.Manual || dialoguePassingType == DialoguePassingType.Both))
                {
                    ContinueStory();
                }
            }
        }

        /// <summary>
        /// Goes to the next story after a couple of seconds
        /// </summary>
        private IEnumerator GoToNextStory()
        {
            yield return new WaitForSeconds(secondsToMoveTonextStory);
            ContinueStory();
        }

        /// <summary>
        /// This continues the Dialogue. 
        /// Should be used by Buttons if you want a button press to continue the dialogue
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ButtonDownContinueStory()
        {
            if (currentStoryObject == null) throw new Exception("No StoryObject found. EnterDialogueMode function has probably been bypassed or there is an error that doesn't allow it to be created");

            if (!dialogueIsPlaying && currentStoryObject.CanManuallyContinue()) ContinueStory();
        }

        /// <summary>
        /// Starts the Dialogue
        /// </summary>
        /// <param name="file">The File with the Dialogue</param>
        /// <param name="fileTypeUsed">The File Type Used</param>
        public void EnterDialogueMode(TextAsset file, FileTypeUsed fileTypeUsed)
        {
            if (file == null)
            {
                Debug.LogError("Can't find the necessary file to generate a dialogue");
                return;
            }

            currentStoryObject = new StoryObject(fileTypeUsed, file);
            currentStoryObject.StartStory();
            dialogueIsPlaying = true;
            dialogueSystemOnEvent?.Invoke();
            DialogueUI.SetActive(true);

            ContinueStory();
        }

        /// <summary>
        /// Exits the Dialogue mode
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(secondsToWait);
            dialogueIsPlaying = false;
            dialogueSystemOffEvent?.Invoke();
            DialogueUI.SetActive(false);
            dialogueText.text = " ";
        }

        /// <summary>
        /// This Continues to the Next Line of the Dialogue
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ContinueStory()
        {
            if (currentStoryObject == null) throw new Exception("No StoryObject found. EnterDialogueMode function has probably been bypassed or there is an error that doesn't allow it to be created");

            if (currentStoryObject.CanContinue())
            {
                string text = currentStoryObject.NextLine();

                if (text == "done")
                {
                    StartCoroutine(ExitDialogueMode());
                    return;
                }

                dialogueText.text = text;

                currentStoryObject.DisplayChoices();

                if ((dialoguePassingType == DialoguePassingType.Automatic) || dialoguePassingType == DialoguePassingType.Both && currentStoryObject.CanManuallyContinue())
                {
                    StartCoroutine(GoToNextStory());
                }
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }
        }

        /// <summary>
        /// Disables all the Events added to the Dialogue Actions
        /// </summary>
        private void OnDisable()
        {
            dialogueSystemOnEvent = null;
            dialogueSystemOffEvent = null;
        }
    }

    /// <summary>
    /// A StoryObject used to hold the Dialogue being displayed
    /// </summary>
    public class StoryObject
    {
#if USE_INK
        public Story currentStory;
#endif
        //public Story currentStory;
        public FileTypeUsed fileTypeUsed;
        private TextAsset textAsset;

        //List of the other tags
        private List<DialogueBlock> dialogueList;


        private Dictionary<string, string> otherTags;
        int dialogueIndex;
        string text;

        public StoryObject(FileTypeUsed fileTypeUsed, TextAsset textAsset)
        {
            dialogueIndex = 0;
            this.fileTypeUsed = fileTypeUsed;
            this.textAsset = textAsset;
        }

        /// <summary>
        /// Starts the Story or Dialogue
        /// </summary>
        public void StartStory()
        {
            if (fileTypeUsed == FileTypeUsed.TXT)
            {
                if(DialogueManager.instance.tXTReader != null)
                {
                    bool success = DialogueManager.instance.tXTReader.ReadTXT(textAsset, out dialogueList);
                }
                else
                {
                    Debug.LogWarning("No TXTReader added to Dialogue Manager so can't handle TXT Files. Use default if no custom is made");
                    dialogueList = new List<DialogueBlock>();
                }
            }
            else
            {
#if USE_INK
                currentStory = new Story(textAsset.text);
#endif
            }

            dialogueIndex = 0;
        }

        /// <summary>
        /// Checks if there is another string to display or another dialogue to present
        /// </summary>
        /// <returns>true is there is, false if there isn't</returns>
        public bool CanContinue()
        {
            if (fileTypeUsed == FileTypeUsed.TXT)
            {
                return dialogueIndex < dialogueList.Count;
            }
            else
            {
#if USE_INK
                return currentStory.canContinue;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gives out the Next Line of Text to display
        /// </summary>
        /// <returns>The text to display</returns>
        public string NextLine()
        {
            string nameString;

            if (!CanContinue()) return "done";

            if (fileTypeUsed == FileTypeUsed.TXT)
            {
                text = dialogueList[dialogueIndex].dialogueLine;
                otherTags = dialogueList[dialogueIndex].characterTags.ToDictionary(
                    key => key.Key.ToLower(),  // Convert the key to lowercase
                    val => val.Value.ToLower() // Convert the value to lowercase
                );
                otherTags.TryGetValue("speaker", out nameString);
                nameString ??= "Unknown";
                CharacterManager.instance.InvokeTheCharacter(nameString, otherTags);
                dialogueIndex++;
            }
            else
            {
#if USE_INK
                text = currentStory.Continue();
                otherTags = HandleTags(currentStory.currentTags).ToDictionary(
                    key => key.Key.ToLower(),  // Convert the key to lowercase
                    val => val.Value.ToLower() // Convert the value to lowercase
                );
                otherTags.TryGetValue("speaker", out nameString);
                nameString ??= "Unknown";
                CharacterManager.instance.InvokeTheCharacter(nameString, otherTags);
#else
                text = "done";
#endif
            }

            text = (string.IsNullOrEmpty(text)) ? NextLine() : text;
            return text;
        }

        /// <summary>
        /// Displays the available choices in the dialogue line
        /// </summary>
        public void DisplayChoices()
        {
#if USE_INK
            if (fileTypeUsed == FileTypeUsed.Ink)
            {
                ChoiceSystem.Instance.DisplayChoices(currentStory);
            }
            else
            {
                ChoiceSystem.Instance.DisplayChoices(textAsset);
            }
#else
            ChoiceSystem.Instance.DisplayChoices(textAsset);
#endif
        }

        /// <summary>
        /// Handle the Tags passed on by the INKJSON File
        /// </summary>
        /// <param name="currentTags">The Tags that are passed On</param>
        /// <returns>Dictionary of all the tags</returns>
        private Dictionary<string, string> HandleTags(List<string> currentTags)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var tag in currentTags)
            {
                string[] splitString = tag.Split(":");
                string tagKey = splitString[0].Trim().ToLower();
                string tagValue = splitString[1].Trim();

                keyValuePairs.Add(tagKey, tagValue);
            }

            return keyValuePairs;
        }

        /// <summary>
        /// Checks if a button can be used to continue the dialogue
        /// </summary>
        /// <returns>true if it can, false if it can't</returns>
        public bool CanManuallyContinue()
        {
#if USE_INK
            if (fileTypeUsed == FileTypeUsed.TXT) return true;
            else
            {
                if (currentStory.currentChoices.Count <= 0) return true;
            }

            return false;
#else
            return true;
#endif
        }
    }



    public enum FileTypeUsed
    {
        Ink,
        TXT
    }

    public enum DialoguePassingType
    {
        Automatic,
        Manual,
        Both
    }
}