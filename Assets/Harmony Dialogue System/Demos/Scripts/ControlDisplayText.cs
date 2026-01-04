using UnityEngine;

namespace HarmonyDialogueSystem
{
    public class ControlDisplayText : MonoBehaviour
    {

        [SerializeField] private GameObject DisplayText;

        private void Start()
        {
            if (DialogueManager.instance)
            {
                DialogueManager.instance.dialogueSystemOnEvent += HideDisplayText;
                DialogueManager.instance.dialogueSystemOffEvent += ShowDisplayText;
            }
        }

        void ShowDisplayText()
        {
            if (DisplayText) DisplayText.SetActive(true);
        }

        void HideDisplayText()
        {
            if (DisplayText) DisplayText.SetActive(false);
        }

        private void OnDisable()
        {
            if (DialogueManager.instance)
            {
                DialogueManager.instance.dialogueSystemOnEvent -= HideDisplayText;
                DialogueManager.instance.dialogueSystemOffEvent -= ShowDisplayText;
            }
        }
    }
}
