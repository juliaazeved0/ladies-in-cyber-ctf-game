// Minimal stand-ins for isolated logic tests. These do not simulate Unity lifecycle or rendering.
using System;
using System.Collections;
using System.Collections.Generic;
namespace UnityEngine
{
    public class Object
    {
        public static void Destroy(Object target) { }
        public static void DontDestroyOnLoad(Object target) { }
    }
    public class GameObject : Object
    {
        public GameObject gameObject => this;
        public string name;
        public bool activeSelf = true;
        public GameObject parent;
        public bool activeInHierarchy => activeSelf && (parent == null || parent.activeInHierarchy);
        private readonly Dictionary<Type, object> components = new Dictionary<Type, object>();
        public GameObject(string name = "") { this.name = name; }
        public void SetActive(bool value) { activeSelf = value; }
        public T GetComponent<T>() where T : class => components.TryGetValue(typeof(T), out var c) ? c as T : null;
        public void Attach<T>(T component) { components[typeof(T)] = component; }
    }
    public class MonoBehaviour : Object
    {
        public GameObject gameObject = new GameObject();
        public string name => gameObject.name;
        public bool enabled = true;
        public bool isActiveAndEnabled => enabled && gameObject.activeInHierarchy;
        protected T GetComponent<T>() where T : class => gameObject.GetComponent<T>();
        public IEnumerator LastCoroutine;
        protected void StartCoroutine(IEnumerator routine) { LastCoroutine = routine; routine.MoveNext(); }
        protected void StartCoroutine(string method) { }
        protected void StopCoroutine(string method) { }
    }
    public class Transform : IEnumerable<Transform>
    {
        public GameObject gameObject = new GameObject();
        public string name => gameObject.name;
        public readonly List<Transform> children = new List<Transform>();
        public IEnumerator<Transform> GetEnumerator() => children.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public struct Vector2 { public float x, y; public Vector2(float x, float y) { this.x = x; this.y = y; } }
    public struct Color { public float a; }
    public class AudioClip { }
    public class Animator { public bool enabled; public void SetTrigger(string trigger) { } }
    public class ScriptableObject : Object { }
    public class Sprite { }
    public class CreateAssetMenuAttribute : Attribute { public string fileName, menuName; }
    public class TextAreaAttribute : Attribute { public TextAreaAttribute(int a, int b) { } }
    public class Material : Object
    {
        public readonly Dictionary<int, float> values = new Dictionary<int, float>();
        public bool HasProperty(int id) => true;
        public void SetFloat(int id, float value) => values[id] = value;
    }
    public class SpriteRenderer
    {
        public Material sharedMaterial = new Material();
        public Material material = new Material();
    }
    public static class Shader { public static int PropertyToID(string key) => key.GetHashCode(); }
    public static class Time { public static float time, deltaTime = 0.02f; }
    public class HeaderAttribute : Attribute { public HeaderAttribute(string value) { } }
    public class TooltipAttribute : Attribute { public TooltipAttribute(string value) { } }
    public class SerializeField : Attribute { }
    public class WaitForSeconds { public WaitForSeconds(float seconds) { } }
    public static class Debug
    {
        public static void Log(object text) { }
        public static void LogWarning(object text, object context = null) { }
        public static void LogError(object text, object context = null) { }
    }
    public static class PlayerPrefs
    {
        private static readonly Dictionary<string, string> values = new Dictionary<string, string>();
        public static string GetString(string key, string fallback = "") => values.TryGetValue(key, out var v) ? v : fallback;
        public static int GetInt(string key, int fallback = 0) => values.TryGetValue(key, out var v) ? int.Parse(v) : fallback;
        public static bool HasKey(string key) => values.ContainsKey(key);
        public static void DeleteKey(string key) => values.Remove(key);
        public static void SetFloat(string key, float value) => values[key] = value.ToString();
        public static void SetInt(string key, int value) => values[key] = value.ToString();
        public static void SetString(string key, string value) => values[key] = value;
        public static void DeleteAll() => values.Clear();
        public static void Save() { }
    }
    public static class GUIUtility { public static string systemCopyBuffer; }
    public static class Application { public static string streamingAssetsPath = "/tmp"; }
    public static class Mathf {
        public static int CeilToInt(float f) => (int)Math.Ceiling(f);
        public static float Clamp(float x, float min, float max) => Math.Clamp(x, min, max);
        public static float Clamp01(float x) => Clamp(x, 0, 1);
        public static float Max(float x, float y) => Math.Max(x, y);
        public static float PingPong(float x, float max) => max == 0 ? 0 : max - Math.Abs(x % (2 * max) - max);
    }
    public enum KeyCode { E }
    public static class Input { public static bool GetKeyDown(KeyCode key) => false; }
    public class Collider2D { public bool CompareTag(string tag) => tag == "Player"; }
    public class AsyncOperation
    {
        public event Action<AsyncOperation> completed;
        public void Complete() => completed?.Invoke(this);
    }
}
namespace UnityEngine.SceneManagement
{
    public enum LoadSceneMode { Single, Additive }
    public struct Scene { public bool isLoaded; }
    public static class SceneManager
    {
        public static bool IntroductionLoaded;
        public static int Loads;
        public static UnityEngine.AsyncOperation Pending;
        public static Scene GetSceneByName(string name) => new Scene { isLoaded = IntroductionLoaded };
        public static UnityEngine.AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Loads++;
            return Pending = new UnityEngine.AsyncOperation();
        }
        public static void UnloadSceneAsync(string name) { IntroductionLoaded = false; }
    }
}
namespace TMPro
{
    public class TMP_Text : UnityEngine.MonoBehaviour { public string text = ""; public void SetActive(bool value) => gameObject.SetActive(value); }
    public class TextMeshProUGUI : TMP_Text { }
    public class TMP_InputField : TMP_Text
    {
        public bool readOnly, interactable;
        public void ActivateInputField() { }
    }
}
namespace UnityEngine.UI
{
    public class Image : UnityEngine.MonoBehaviour { public UnityEngine.Color color; public UnityEngine.Sprite sprite; }
    public class Button
    {
        public T GetComponentInChildren<T>() where T : new() => new T();
        public UnityEngine.GameObject gameObject = new UnityEngine.GameObject();
        public bool interactable;
        public ButtonClickedEvent onClick = new ButtonClickedEvent();
        public class ButtonClickedEvent
        {
            private event Action callbacks;
            public void AddListener(Action callback) => callbacks += callback;
            public void Invoke() => callbacks?.Invoke();
        }
    }
}

public class CanvasManager
{
    public static CanvasManager Instance;
    public System.Collections.Generic.List<UnityEngine.GameObject> allPanels = new System.Collections.Generic.List<UnityEngine.GameObject>();
    public void OpenPanel(string name) { }
    public void ClosedPanel(string name) { }
    public void ToggleMiniMap(bool active) { }
    public void ClosedAllPanels() { }
    public bool IsAnyPanelOpen() => allPanels.Exists(p => p.activeInHierarchy);
}
public static class DialogueManagerBoss { public static bool dialogueBossFinished; }

public class NPCDialogueNode { }
public class SimpleDialogue { public static bool isSimpleDialogueActive; public PulseOutline pulsingObject; public void StartDialogue(NPCDialogueNode node) { } }
public class WriteMachine { public void Run(string text, TMPro.TextMeshProUGUI label) { } }
public class PlayerNameplate { public void SetNameplateIdPlayer() { } }
public static class BackgroundMusic { public static void ChangeMusic(UnityEngine.AudioClip clip) { } }

