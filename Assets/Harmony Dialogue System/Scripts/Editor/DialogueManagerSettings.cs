using UnityEngine;
using UnityEditor;
using System.Linq;

#if UNITY_EDITOR
namespace HarmonyDialogueSystem
{
    public class DialogueManagerSettings : EditorWindow
    {
        private bool isInkInstalled;

        [MenuItem("Tools/Harmony Dialogue/Ink Settings")]
        public static void ShowWindow()
        {
            GetWindow<DialogueManagerSettings>("Ink Settings");
        }

        void OnEnable()
        {
            // Check if Ink is installed
            isInkInstalled = System.AppDomain.CurrentDomain
                .GetAssemblies()
                .Any(a => a.GetTypes()
                    .Any(t => t.FullName == "Ink.Runtime.Story"));

            // If Ink is not installed but USE_INK is defined, remove it
            if (!isInkInstalled)
            {
                RemoveInkDefine();
            }
        }

        void OnGUI()
        {
            EditorGUILayout.Space(10);
            GUILayout.Label("Ink Integration Status", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Ink Installed: " + (isInkInstalled ? "Yes" : "No"));

            if (!isInkInstalled)
            {
                EditorGUILayout.HelpBox(
                    "Ink is not installed. Install Ink from the Asset Store to enable Ink features.",
                    MessageType.Info);
            }
            else
            {
                if (GUILayout.Button(HasInkDefine() ? "Disable Ink Integration" : "Enable Ink Integration"))
                {
                    ToggleInkDefine();
                }
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Ink Integration: " + (HasInkDefine() ? "Enabled" : "Disabled"));
        }

        private bool HasInkDefine()
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);
            return defines.Contains("USE_INK");
        }

        private void ToggleInkDefine()
        {
            if (HasInkDefine())
            {
                RemoveInkDefine();
            }
            else
            {
                AddInkDefine();
            }
        }

        private void AddInkDefine()
        {
            var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (!defines.Contains("USE_INK"))
            {
                defines = string.IsNullOrEmpty(defines) ? "USE_INK" : defines + ";USE_INK";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            }
        }

        private void RemoveInkDefine()
        {
            var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup)
                .Split(';')
                .Where(d => d != "USE_INK")
                .ToArray();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defines));
        }
    }
}
#endif
