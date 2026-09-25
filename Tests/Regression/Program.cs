using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class Program
{
    private static int checks;
    private static void Check(bool condition, string description)
    {
        if(!condition) throw new Exception(description);
        checks++;
    }
    private static void Call(object target, string method) => target.GetType()
        .GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(target, null);
    private static void Set(object target, string field, object value) => target.GetType()
        .GetField(field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).SetValue(target, value);

    public static void Main()
    {
        PlayerPrefs.DeleteAll();
        Check(LoadSceneIntroduction.NeedsIntroduction, "First visit needs introduction");
        LoadSceneIntroduction.TryLoadIntroduction();
        LoadSceneIntroduction.TryLoadIntroduction();
        Check(SceneManager.Loads == 1, "Concurrent triggers cannot load two introductions");
        Check(PlayerPrefs.GetInt("introductionComplete") == 0, "Starting intro must not complete it");
        SceneManager.IntroductionLoaded = true;
        SceneManager.Pending.Complete();
        LoadSceneIntroduction.TryLoadIntroduction();
        Check(SceneManager.Loads == 1, "Loaded introduction must not reload");
        var intro = new IntroScreenController();
        intro.FinishIntroduction();
        Check(SceneManager.IntroductionLoaded, "Cannot finish intro without flag");
        PlayerPrefs.SetInt("introductionComplete", 1);
        Check(LoadSceneIntroduction.NeedsIntroduction, "Legacy interrupted save can recover missing flag");
        var flags = new FlagManager();
        Call(flags, "Awake");
        intro.OnFlagButtonClicked(); // UI refs may be absent; collection must still succeed.
        intro.FinishIntroduction();
        Check(!LoadSceneIntroduction.NeedsIntroduction, "Completed introduction stays completed");
        Check(!SceneManager.IntroductionLoaded, "Completed intro unloads");

        flags.SaveFlag("Senha Anotada", SafeBase.ViewBase(SafeBase.flag_6));
        FlagManager.Instance = null; // Simulate a scene/application restart before Awake.
        Check(ChallengeManager.IsCompleted("CryptoPassword"), "Migrate old crypto flag before manager Awake");
        Check(!ChallengeManager.IsCompleted("CryptoCapivara"), "Do not unlock uncollected second challenge");
        var manager = new ChallengeManager();
        manager.CompleteChallenge("CryptoCapivara");
        Check(new ChallengeManager().IsChallengeCompleted("CryptoCapivara"), "Progress survives a new manager");
        Check(!ChallengeManager.IsCompleted(""), "Empty challenge ID cannot unlock anything");
        Check(!FlagManager.HasSavedFlag(""), "Empty flag cannot match saved inventory");

        Check(ClipboardManager.ExtractFlag("Desafio - L1C{a - b}") == "L1C{a - b}", "Preserve separator inside flag");
        ClipboardManager.CopyFlag("Desafio - L1C{test}");
        Check(GUIUtility.systemCopyBuffer == "L1C{test}", "Copy flag without challenge title");
        var inventory = new InventoryDisplay();
        var copyObject = new GameObject();
        var copyButton = new Button();
        copyObject.Attach(copyButton);
        int staleCalls = 0;
        copyButton.onClick.AddListener(() => staleCalls++);
        var setup = typeof(InventoryDisplay).GetMethod("SetupCopyButton", BindingFlags.NonPublic | BindingFlags.Instance);
        setup.Invoke(inventory, new object[] { copyObject, "First - L1C{first}" });
        setup.Invoke(inventory, new object[] { copyObject, "Second - L1C{second}" });
        copyButton.onClick.Invoke();
        Check(staleCalls == 0 && GUIUtility.systemCopyBuffer == "L1C{second}", "Pagination replaces old copy callbacks");

        var login = new LoginSystem();
        Set(login, "postItText", new TMP_Text { text = "Q2gzZj\n FuaDA=" });
        login.CopyPostIt();
        Check(GUIUtility.systemCopyBuffer == "Q2gzZjFuaDA=", "Copy encoded clue without layout whitespace");
        Check(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(GUIUtility.systemCopyBuffer)) == "Ch3f1nh0", "Clue remains valid Base64, not the decoded answer");

        var lockedComputer = new LockObjectInteraction();
        Set(lockedComputer, "unlockAfterChallenge", "CryptoPassword");
        Call(lockedComputer, "Start");
        Check(lockedComputer.isUnlocked, "Polyana computer restores unlock without opening password panel");
        var closedRoot = new GameObject();
        closedRoot.SetActive(false);
        var childPanel = new GameObject { parent = closedRoot };
        CanvasManager.Instance = new CanvasManager();
        CanvasManager.Instance.allPanels.Add(childPanel);
        var interaction = new ObjectInteraction();
        var panelCheck = typeof(ObjectInteraction).GetMethod("IsAnyPanelOpen", BindingFlags.NonPublic | BindingFlags.Instance);
        Check(!(bool)panelCheck.Invoke(interaction, null), "Active child of closed parent does not block interaction");
        closedRoot.SetActive(true);
        Check((bool)panelCheck.Invoke(interaction, null), "Visible challenge blocks interaction");

        // The boss door requires all eight flags plus the successful Joana ending.
        PlayerPrefs.DeleteAll();
        FlagManager.Instance = null;
        var flagManager = new FlagManager();
        Call(flagManager, "Awake");
        CanvasManager.Instance = new CanvasManager();
        var gate = new UnlockBossRoom();
        var gateInput = new TMP_InputField { text = "1541" };
        var icon = new GameObject("LockIcon");
        var blocker = new GameObject("BossBlocker");
        Set(gate, "input", gateInput);
        Set(gate, "lockIcon", icon);
        Set(gate, "lockObject", blocker);
        Set(gate, "devicePanel", new GameObject("Device"));
        var joana = new DialogueManager();
        var joanaPanel = new GameObject("JoanaDialogue");
        var finalNode = new DialogueNode { buttonType = ButtonType.Done, nextDialogue = Array.Empty<DialogueNode>() };
        Set(joana, "requireAllMainFlags", true);
        Set(joana, "panelDialogue", joanaPanel);
        Set(joana, "miniMapCanvas", new GameObject("MiniMap"));
        Set(joana, "cameraMiniMap", new GameObject("MiniMapCamera"));
        Set(joana, "dialogueCurrent", finalNode);
        Set(gate, "dialogueManager", joana);
        Call(gate, "Start");
        gate.PressEnter();
        Check(PlayerPrefs.GetInt("BossRoomUnlocked") == 0 && icon.activeSelf, "Password cannot bypass flags or Joana");
        for(int i = 0; i < 8; i++) flagManager.SaveFlag("main", SafeBase.GetFlag(i));
        Check(FlagManager.HasAllMainFlags(), "Joana prerequisite counts the eight specific main flags");
        gate.PressEnter();
        Check(PlayerPrefs.GetInt("BossRoomUnlocked") == 0 && icon.activeSelf, "All flags alone do not unlock the door");
        gate.OpenPasswordPanel();
        Check(gate.CanEnterPassword && icon.activeSelf && blocker.activeSelf, "Joana authorizes password entry while door stays blocked");
        gateInput.text = "wrong";
        gate.PressEnter();
        Check(PlayerPrefs.GetInt("BossRoomUnlocked") == 0 && icon.activeSelf, "Wrong password leaves the door locked");
        gateInput.text = "1541";
        gate.PressEnter();
        Check(PlayerPrefs.GetInt("BossRoomUnlocked") == 0 && gateInput.text == "ACESSO CONCEDIDO", "Correct password waits for success feedback");
        gate.LastCoroutine.MoveNext();
        Check(PlayerPrefs.GetInt("BossRoomUnlocked") == 1 && !icon.activeSelf && !blocker.activeSelf, "Only correct password removes icon and blocker");
        gate.OpenPasswordPanel();
        Check(!gate.CanEnterPassword, "Door cannot request password again after unlock");

        var rendererObject = new GameObject("PulsingObjective");
        rendererObject.Attach(new UnityEngine.SpriteRenderer());
        var pulse = new PulseOutline();
        pulse.gameObject = rendererObject;
        Set(pulse, "completionFlagIndex", 8);
        Call(pulse, "Awake");
        pulse.StartPulsing();
        Check(pulse.IsPulsing, "Uncollected challenge can pulse");
        flagManager.SaveFlag("main", SafeBase.GetFlag(8));
        Call(pulse, "Update");
        Check(!pulse.IsPulsing, "Captured challenge outline stops and cannot be restarted");

        var root = new GameObject("Desktop");
        var content = new GameObject("Documents");
        var navigation = new FileNavigationManager {
            rootFolder = root, boardFolders = new GameObject("Board"), buttonEnter = new GameObject("Enter"),
            pathText = new TextMeshProUGUI(), backButton = new Button(), popUpConatiner = new Transform()
        };
        navigation.popUpConatiner.children.Add(new Transform { gameObject = root });
        navigation.popUpConatiner.children.Add(new Transform { gameObject = content });
        var folder = new Folder { contentFolder = content, folderName = "Documents" };
        navigation.ResetNavigation();
        navigation.OpenFolder(folder);
        Check(content.activeSelf && navigation.backButton.interactable, "Open nested folder");
        navigation.ClosePopUp();
        Check(root.activeSelf && !content.activeSelf && !navigation.backButton.interactable, "Closing resets root and history");
        navigation.boardFolders.SetActive(true);
        navigation.OpenFolder(folder);
        Check(content.activeSelf, "Same folder opens after reopening window");
        navigation.GoBack();
        Check(root.activeSelf && !content.activeSelf, "Back reaches root after reopening");
        Console.WriteLine($"Passed {checks} isolated C# regression checks.");
    }
}
