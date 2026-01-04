using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HarmonyDialogueSystem
{
    [RequireComponent(typeof(DialogueManager))]
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance;

        [Tooltip("True if to remove numbers from the display string shown in the Dialogue UI.")]
        public bool removeEndingNumber;

        [Header("Character Specific UI")]
        [Tooltip("The Character Name Text")]
        [SerializeField] private TextMeshProUGUI characterName;

        [Tooltip("The Character Potrait UI")]
        [SerializeField] private Image characterPortrait;

        [Tooltip("The default Sprite that is used if a character has no portrait")]
        [SerializeField] private Sprite defaultPortrait;

        [Header("Characters")]
        [Tooltip("Called when a new character starts speaking")]
        public UnityEvent<Character> characterChanged;

        [Tooltip("List of Characters. This is automatically populated if a Character in the Text Asset is not found here")]
        [SerializeField] List<Character> characters;

        private Character currentSpeakingCharacter;
        private Character oldCharacter;

        /// <summary>
        /// The character that is currently speaking
        /// </summary>
        public static Character CurrentSpeakingCharacter { get => instance.currentSpeakingCharacter; }

        /// <summary>
        /// Called for each dialogue line
        /// </summary>
        public Action<Character> EachDialogueLine;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;
        }

        /// <summary>
        /// Finds a character in the character List
        /// </summary>
        /// <param name="_name">The name of the character to find</param>
        /// <returns>Returns a character</returns>
        private Character FindCharacter(string _name)
        {
            return characters.FirstOrDefault(c => c.name.ToLower().Equals(_name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Creates a new character
        /// </summary>
        /// <param name="_name">The name of the character</param>
        /// <returns>A newly created Character</returns>
        private Character CreateCharacter(string _name)
        {
            Character character = new Character(0, _name, defaultPortrait);
            characters.Add(character);

            return character;
        }

        /// <summary>
        /// Invokes the character that is speaking
        /// </summary>
        /// <param name="_name">The name of the character</param>
        /// <param name="characterTags">The tags of the character</param>
        public void InvokeTheCharacter(string _name, Dictionary<string, string> characterTags)
        {
            // Find the character by name or create a new one if not found
            Character newCharacter = FindCharacter(_name) ?? CreateCharacter(_name);

            // Update tags on the new character
            newCharacter.SetCharacterTags(characterTags);

            // Only invoke the characterChanged event if the character has changed
            if (currentSpeakingCharacter != newCharacter)
            {
                currentSpeakingCharacter = newCharacter;
                characterChanged?.Invoke(currentSpeakingCharacter);
            }

            // Invoke each dialogue line and character-specific events
            EachDialogueLine?.Invoke(currentSpeakingCharacter);
            currentSpeakingCharacter.InvokeCharacter();

            // Update the character UI
            SetCharacterUI(currentSpeakingCharacter);
        }

        /// <summary>
        /// Sets the necessary Character UI
        /// </summary>
        /// <param name="character">The character to set it with</param>
        private void SetCharacterUI(Character character)
        {
            if (removeEndingNumber) characterName.text = RemoveEndingNumber(GetDisplayName(character));
            else characterName.text = GetDisplayName(character);

            characterName.text = character.displayName;
            characterPortrait.sprite = character.portrait;
        }

        private string GetDisplayName(Character character)
        {
            if(string.IsNullOrEmpty(character.displayName))
            {
                return character.name;
            }

            return character.displayName;
        }

        /// <summary>
        /// Removes a Number at the end of a string
        /// </summary>
        /// <param name="input">The string to be edited</param>
        /// <returns>An edited string</returns>
        public static string RemoveEndingNumber(string input)
        {
            // Check if the input string ends with a number
            string pattern = @"\d+$";
            if (Regex.IsMatch(input, pattern))
            {
                // Remove the ending number from the string
                return Regex.Replace(input, pattern, "");
            }

            // If the input doesn't end with a number, return the original string
            return input;
        }
    }
}
