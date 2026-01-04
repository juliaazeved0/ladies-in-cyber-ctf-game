#if USE_INK
using Ink.Runtime;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

namespace HarmonyDialogueSystem
{
    public class ChoiceSystem : MonoBehaviour
    {
        public static ChoiceSystem Instance { get; private set; }

        List<GameObject> choiceList;

        private GameObject choicePrefab;

        VerticalLayoutGroup choicesContainer;


        int choiceIndex;

        private void Awake()
        {
            Instance = this;
            choiceList = new List<GameObject>();
        }

        public void SetChoiceSystem(GameObject choicePrefab, VerticalLayoutGroup choiceContainer)
        {
            this.choicePrefab = choicePrefab;
            choicesContainer = choiceContainer;
        }

#if USE_INK
        /// <summary>
        /// This Displays all the Choices to the User
        /// </summary>
        /// <param name="currentStory">The current Story being used</param>
        public void DisplayChoices(Story currentStory)
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            int index = 0;
            DisableChoices();

            foreach (Choice choice in currentChoices)
            {
                Debug.Log(currentChoices.Count);
                if (currentChoices.Count > choiceList.Count) CreateChoice(index, choice.text, currentStory);
                else EnableChoice(index, choice.text, currentStory);

                if (currentChoices.Count > 1) StartCoroutine(SelectFirstChoice());

                index++;
            }
        }
#endif


        public void DisplayChoices(TextAsset TXTasset)
        {

        }

#if USE_INK
        /// <summary>
        /// This creates a Choice GameObject
        /// </summary>
        /// <param name="index">index of the choice in the choiceList</param>
        /// <param name="_text">Text that should be on the choice</param>
        /// <param name="currentStory">The current Story being used</param>
        /// <exception cref="Exception"></exception>
        private void CreateChoice(int index, string _text, Story currentStory)
        {
            GameObject newButton = Instantiate(choicePrefab, choicesContainer.transform);

            newButton.GetComponentInChildren<Button>().onClick.AddListener(() => MakeChoice(index, currentStory));

            TextMeshProUGUI choicesText = newButton.GetComponentInChildren<TextMeshProUGUI>() ?? throw new Exception("TextMeshPro is not a child of your Dialogue Choice Prefab. Cannot find Text");
            choicesText.text = _text;
            choiceList.Add(newButton);
        }

        /// <summary>
        /// Enables already existing Choice GameObjects
        /// </summary>
        /// <param name="index">index of the choice in the choiceList</param>
        /// <param name="_text">Text that should be on the choice</param>
        void EnableChoice(int index, string _text, Story currentStory)
        {
            GameObject choice = choiceList[index];
            Button choiceButton = choice.GetComponentInChildren<Button>();
            choiceButton.onClick.RemoveAllListeners();
            choiceButton.onClick.AddListener(() => MakeChoice(index, currentStory));

            TextMeshProUGUI choicesText = choice.GetComponentInChildren<TextMeshProUGUI>() ?? throw new Exception("TextMeshPro is not a child of your Dialogue Choice Prefab. Cannot find Text");
            choicesText.text = _text;
            choice.SetActive(true);
        }

        /// <summary>
        /// Disables all the choice gameObjects
        /// </summary>
        void DisableChoices()
        {
            foreach (var choice in choiceList) choice.SetActive(false);
        }

        /// <summary>
        /// Automatically Selects the first choice as default when the dialogue starts
        /// </summary>
        /// <returns></returns>
        private IEnumerator SelectFirstChoice()
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choiceList[0]);
        }

        /// <summary>
        /// Function that runs when any choice is clicked one
        /// </summary>
        /// <param name="choiceIndex">index of the choice in the choiceList</param>
        /// <param name="currentStory">he current Story being used</param>
        public void MakeChoice(int choiceIndex, Story currentStory)
        {
            if (currentStory.currentChoices.Count > choiceIndex) currentStory.ChooseChoiceIndex(choiceIndex);
            DialogueManager.instance.ContinueStory();
        }
#endif

    }
}
