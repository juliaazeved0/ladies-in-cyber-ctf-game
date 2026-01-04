using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HarmonyDialogueSystem
{

    [System.Serializable]
    public class Character
    {
        private int id;
        [Tooltip("Name of the character. This should match the character name in the TextAsset file. Not Case Sensitive")]
        public string name;

        [Tooltip("Display name for the character. This is what is displayed in the Dialogue UI")]
        public string displayName;

        [Tooltip("Portrait for the Character")]
        public Sprite portrait;

        [Tooltip("Prefab of the character.")]
        public GameObject characterGameObject;

        /// <summary>
        /// Character tags for each line of dialogue that the character mentions. In a Key:Value format
        /// </summary>
        public Dictionary<string, string> CharacterTags { get; private set; }

        [Tooltip("Event(s) that is Inovked when this character is speaking")]
        public UnityEvent characterSpeaking;

        public Character(int id, string name, Sprite portrait)
        {
            this.id = id;
            this.name = name;
            displayName = name;
            this.portrait = portrait;
        }

        /// <summary>
        /// Passes the Dialogue Tags to the Character
        /// </summary>
        /// <param name="characterTags">The Dictionary of character tags</param>
        public void SetCharacterTags(Dictionary<string, string> characterTags)
        {
            this.CharacterTags = characterTags;
        }

        /// <summary>
        /// Checks if a particular key tag exists in the dictionary
        /// </summary>
        /// <param name="key">The key to check with</param>
        /// <param name="outKey">The key of the tag</param>
        /// <param name="outValue">The value of the tag</param>
        /// <returns></returns>
        public bool LookForCharacterTag(string key, out string outValue)
        {
            outValue = null;

            if (CharacterTags.ContainsKey(key.ToLower()))
            {
                outValue = CharacterTags[key];
                return true;
            }
 
            return false;
        }

        /// <summary>
        /// Called when the character is speaking
        /// </summary>
        public void InvokeCharacter()
        {
            characterSpeaking?.Invoke();
        }
    }
}
