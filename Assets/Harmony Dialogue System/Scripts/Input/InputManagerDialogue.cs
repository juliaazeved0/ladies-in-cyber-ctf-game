using UnityEngine;

namespace HarmonyDialogueSystem
{
    public class InputManagerDialogue : MonoBehaviour
    {
        public static InputManagerDialogue instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;
        }

        /// <summary>
        /// If the User is interacting with a trigger
        /// </summary>
        /// <returns></returns>
        public static bool isInteracting()
        {
            return Input.GetKeyDown(KeyCode.I);
        }

        /// <summary>
        /// If the user is continuing on with the dialogue flow
        /// </summary>
        /// <returns></returns>
        public static bool isContinuing()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}
