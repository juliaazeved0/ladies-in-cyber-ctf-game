using System.Collections.Generic;
using UnityEngine;

namespace HarmonyDialogueSystem.Demo
{
    public class ExampleGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject Alert;
        [SerializeField] private GameObject Player;
        [SerializeField] private float rotationSpeed;

        [SerializeField] private Dictionary<string, GameObject> characterObjects;

        GameObject target;

        private void Awake()
        {
            Alert.SetActive(false);
        }

        public void ShowInteraction()
        {
            Alert.SetActive(true);
        }

        public void HideInteraction()
        {
            Alert.SetActive(false);
        }

        public void TurnCameraToSpeaker()
        {
            if (target != CharacterManager.CurrentSpeakingCharacter.characterGameObject)
            {
                target = CharacterManager.CurrentSpeakingCharacter.characterGameObject;
                transform.LookAt(target.transform.position);
            }
        }


        public void AddScubscriber()
        {
            CharacterManager.instance.characterChanged.AddListener(GetCharacterTag);
        }

        public void GetCharacterTag(Character character)
        {
            string tag = "speaker";
            if (character.LookForCharacterTag(tag, out string value))
            {
                Debug.Log(value);
            }
        }
    }
}
