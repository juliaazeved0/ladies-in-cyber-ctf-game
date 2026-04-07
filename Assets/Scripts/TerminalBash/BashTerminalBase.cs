using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BashTerminal
{
    public abstract class BashTerminalBase : MonoBehaviour
    {
        [Header("Terminal UI - Referencias")]
        [SerializeField] protected GameObject terminalPanel; 
        [SerializeField] protected TMP_Text outputText;
        [SerializeField] protected TMP_InputField inputField;
        [SerializeField] protected ScrollRect scrollRect;

        protected string currentDirectory;
        protected readonly StringBuilder outputBuffer = new StringBuilder();
        protected bool isOpen = false;

        protected abstract string User { get; }
        protected abstract string Hostname { get; }
        protected abstract string HomeDirectory { get; }

        protected readonly Dictionary<string, string> files = new Dictionary<string, string>();
        protected readonly HashSet<string> directories = new HashSet<string>();
        protected readonly HashSet<string> executableFiles = new HashSet<string>();
        protected readonly Dictionary<string, string> fileOwners = new Dictionary<string, string>();

        protected virtual void Awake()
        {
            if (terminalPanel != null) terminalPanel.SetActive(false);
            if (inputField != null)
            {
                inputField.customCaretColor = true;
                inputField.caretColor = new Color(0.33f, 1f, 0.33f);
                inputField.caretWidth = 5;
            }
        }

        public void OpenTerminal()
        {
            if (terminalPanel == null) return;
            terminalPanel.SetActive(true);
            isOpen = true;
            currentDirectory = HomeDirectory;
            outputBuffer.Clear();
            SetupFilesystem();
            string welcome = GetWelcomeMessage();
            if (!string.IsNullOrEmpty(welcome)) AppendLine(welcome);
            AppendPrompt();
            inputField.text = "";
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(HandleInput);
            StartCoroutine(FocusNextFrame());
        }

        public void CloseTerminal()
        {
            isOpen = false;
            if (terminalPanel != null) terminalPanel.SetActive(false);
        }

        protected abstract void SetupFilesystem();
        protected abstract string GetWelcomeMessage();
        protected abstract string GetHelpText();

        protected void AppendLine(string line = "")
        {
            outputBuffer.AppendLine(line);
            RefreshOutput();
        }

        protected void RefreshOutput()
        {
            if (outputText != null) outputText.text = outputBuffer.ToString();
            if (scrollRect != null) StartCoroutine(ScrollToBottomNextFrame());
        }

        private IEnumerator ScrollToBottomNextFrame()
        {
            yield return null;
            if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0f;
        }

        private IEnumerator FocusNextFrame()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            if (inputField != null) { inputField.ActivateInputField(); inputField.Select(); }
        }

        protected void AppendPrompt()
        {
            string path = GetDisplayPath();
            outputBuffer.Append($"\n<color=#55FF55>{User}@{Hostname}</color>:<color=#5599FF>{path}</color>$ ");
            RefreshOutput();
        }

        protected string GetDisplayPath()
        {
            if (currentDirectory == HomeDirectory) return "~";
            return currentDirectory.Replace(HomeDirectory, "~");
        }

        protected void HandleInput(string input)
        {
            if (!isOpen) return;
            outputBuffer.AppendLine(input);
            if (!string.IsNullOrEmpty(input)) ProcessCommand(input.Trim());
            AppendPrompt();
            inputField.text = "";
            StartCoroutine(FocusNextFrame());
        }

        protected virtual void ProcessCommand(string fullCommand)
        {
            string[] tokens = fullCommand.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return;
            string cmd = tokens[0];
            string[] args = tokens.Skip(1).ToArray();

            if (ExecuteSpecialCommand(cmd, args, fullCommand)) return;

            switch (cmd)
            {
                case "ls": AppendLine(ExecuteLs(args)); break;
                case "cd": ExecuteCd(args); break;
                case "cat": AppendLine(ExecuteCat(args)); break;
                case "clear": outputBuffer.Clear(); break;
                case "help": AppendLine(GetHelpText()); break;
                case "exit": CloseTerminal(); break;
                default: AppendLine($"bash: {cmd}: command not found"); break;
            }
        }

        protected virtual bool ExecuteSpecialCommand(string cmd, string[] args, string fullCommand) => false;
        protected virtual string OnCatFile(string resolvedPath, string originalArg) => null;

        // --- COMANDO LS ---
        protected string ExecuteLs(string[] args)
        {
            bool showHidden = args.Any(a => a.Contains("a"));
            bool longFormat = args.Any(a => a.Contains("l"));
            var contents = files.Keys.Concat(directories)
                .Where(p => p.StartsWith(currentDirectory) && p != currentDirectory)
                .Select(p => p.Substring(currentDirectory.Length).TrimStart('/'))
                .Where(p => !p.Contains("/"))
                .ToList();

            var sb = new StringBuilder();
            foreach (var item in contents)
            {
                if (!showHidden && item.StartsWith(".")) continue;
                string path = currentDirectory + "/" + item;
                bool isDir = directories.Contains(path);
                bool isExec = executableFiles.Contains(path);

                if (longFormat)
                {
                    sb.AppendLine($"{BuildPermString(path, isDir, isExec)}  {User}  {item}");
                }
                else
                {
                    string color = isDir ? "#5599FF" : (isExec ? "#55FF55" : "#FFFFFF");
                    sb.Append($"<color={color}>{item}</color>  ");
                }
            }
            return sb.ToString();
        }

        // --- METODO QUE ESTAVA CAUSANDO O ERRO ---
        // Adicionada a palavra 'virtual' para permitir override
        protected virtual string BuildPermString(string path, bool isDir, bool isExec)
        {
            string d = isDir ? "d" : "-";
            string x = isExec ? "x" : "-";
            return $"{d}rwxr-{x}r-{x}";
        }

        protected void ExecuteCd(string[] args)
        {
            if (args.Length == 0) { currentDirectory = HomeDirectory; return; }
            string target = currentDirectory + "/" + args[0];
            if (directories.Contains(target)) currentDirectory = target;
            else AppendLine("cd: diretorio nao encontrado.");
        }

        protected string ExecuteCat(string[] args)
        {
            if (args.Length == 0) return "cat: missing operand";
            string path = currentDirectory + "/" + args[0];
            string intercepted = OnCatFile(path, args[0]);
            if (intercepted != null) return intercepted;
            return files.ContainsKey(path) ? files[path] : "cat: arquivo nao encontrado.";
        }

        protected void AddDirectory(string path) => directories.Add(path);
        protected void AddFile(string path, string content, string ownerGroup = null)
        {
            files[path] = content;
            if (ownerGroup != null) fileOwners[path] = ownerGroup;
        }
    }
    protected void HandleInput(string input)
{
    if (!isOpen) return;

    input = input.Trim();
    
    // 1. Antes de limpar o input, mandamos o prompt + o comando pro histórico
    // Isso faz com que a linha que você acabou de digitar "suba" para o OutputText
    outputBuffer.AppendLine($"<color=#55FF55>{User}@{Hostname}</color>:<color=#5599FF>{GetDisplayPath()}</color>$ {input}");

    if (!string.IsNullOrEmpty(input))
        ProcessCommand(input);

    // 2. Atualiza o texto de cima
    RefreshOutput();

    // 3. Limpa o campo onde você digita
    inputField.text = "";
    
    // 4. Mantém o foco
    StartCoroutine(FocusNextFrame());
    }

// O método AppendPrompt pode ficar vazio ou ser removido, 
// já que o prompt agora é um objeto fixo na sua InputArea
    protected void AppendPrompt() { }
}