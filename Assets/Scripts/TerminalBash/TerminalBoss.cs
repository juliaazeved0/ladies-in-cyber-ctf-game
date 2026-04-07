using UnityEngine;
using UnityEngine.UI;

namespace BashTerminal
{
    /// <summary>
    /// WebGL terminal for the Boss challenge.
    ///
    /// Simulates the Core OS administration system (BOSS@MAIN_SERVER - Level 5 Access).
    /// Victory: navigate to ~/Documentos and run  mv praia.jpg ../Imagens
    ///
    /// After victory the directory state inverts (mirrors the shell script behaviour):
    ///   Before: Documentos contains praia.jpg, Imagens is empty.
    ///   After:  Documentos is empty, Imagens contains praia.jpg.
    ///
    /// Unity setup:
    ///   - Wire terminalPanel / outputText / inputField in the Inspector.
    ///   - Wire steghideButton to unlock it on challenge completion.
    ///   - Wire terminalSuccessPopup se usar.
    ///   - Hook your open-button OnClick -> OpenBossTerminal()
    ///   - Hook your close button OnClick -> CloseTerminal()
    /// </summary>
    public class TerminalBoss : BashTerminalBase
    {
        [Header("Desafio Boss")]
        [SerializeField] private Button steghideButton;
        [SerializeField] private GameObject terminalSuccessPopup;

        /// <summary>
        /// Mirrors the static flag from AbrirTerminalBoss so other scripts can
        /// read the completion state without a direct reference.
        /// </summary>
        public static bool challengeSolved = false;

        private bool praiaInDocumentos = true;

        // ── Identity ──────────────────────────────────────────────────────────
        protected override string User => "BOSS";
        protected override string Hostname => "MAIN_SERVER";
        protected override string HomeDirectory => "/home/BOSS";

        // ── Entry points ──────────────────────────────────────────────────────
        /// <summary>Call from the terminal button OnClick event.</summary>
        public void OpenBossTerminal()
        {
            challengeSolved = false;
            praiaInDocumentos = true;
            OpenTerminal();
        }

        // ── Filesystem ────────────────────────────────────────────────────────
        protected override void SetupFilesystem()
        {
            AddDirectory("/home/BOSS");
            AddDirectory("/home/BOSS/Documentos");
            AddDirectory("/home/BOSS/Imagens");
            AddDirectory("/home/BOSS/Projetos");

            AddFile("/home/BOSS/Documentos/praia.jpg", "[BINARY] Arquivo de imagem: praia.jpg");

            AddFile("/home/BOSS/Projetos/system_core.bin",
                "[BINARY] Nucleo do sistema operacional - acesso restrito.");
            AddFile("/home/BOSS/Projetos/logs_setor_7.db",
                "[BINARY] Banco de dados de logs - setor 7.");
        }

        // ── Welcome / help ────────────────────────────────────────────────────
        protected override string GetWelcomeMessage()
        {
            return
                "<color=#FF5555>SISTEMA OPERACIONAL CORE - ACESSO NIVEL 5</color>\n" +
                "Bem-vinda, Administradora. Acesso ao Terminal Liberado.";
        }

        protected override string GetHelpText()
        {
            return "Comandos: ls, cd, mv, cat, clear, exit";
        }

        // ── ls override: mirror the shell script's dynamic state ──────────────
        protected override void ProcessCommand(string fullCommand)
        {
            string[] tokens = fullCommand.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return;

            if (tokens[0] == "ls")
            {
                AppendLine(ExecuteBossLs());
                return;
            }

            base.ProcessCommand(fullCommand);
        }

        private string ExecuteBossLs()
        {
            if (currentDirectory == "/home/BOSS")
                return "Documentos/  Imagens/  Projetos/";

            if (currentDirectory == "/home/BOSS/Documentos")
                return praiaInDocumentos ? "praia.jpg" : "pasta vazia";

            if (currentDirectory == "/home/BOSS/Imagens")
                return praiaInDocumentos ? "pasta vazia" : "praia.jpg";

            if (currentDirectory == "/home/BOSS/Projetos")
                return "system_core.bin  logs_setor_7.db";

            return "";
        }

        // ── mv: the only special command for Boss ─────────────────────────────
        protected override bool ExecuteSpecialCommand(string cmd, string[] args, string fullCommand)
        {
            if (cmd != "mv") return false;

            if (args.Length < 2)
            {
                AppendLine("Erro: sintaxe incorreta. Uso: mv [arquivo] [destino]");
                return true;
            }

            string src = args[0];
            string dst = args[1];

            bool srcIsPraia = src == "praia.jpg";
            bool dstIsImagens = dst == "../Imagens" || dst == "/home/BOSS/Imagens";
            bool inDocumentos = currentDirectory == "/home/BOSS/Documentos";

            if (inDocumentos && srcIsPraia && dstIsImagens)
            {
                if (!praiaInDocumentos)
                {
                    AppendLine("Erro: Arquivo nao encontrado.");
                    return true;
                }

                AppendLine("Iniciando transferencia de 'praia.jpg'...");
                AppendLine("Movendo para o diretorio de processamento esteganografico...");

                files.Remove("/home/BOSS/Documentos/praia.jpg");
                AddFile("/home/BOSS/Imagens/praia.jpg", "[BINARY] Arquivo de imagem: praia.jpg");
                praiaInDocumentos = false;

                AppendLine("<color=#55FF55>Sucesso! Arquivo movido. O Steghide ja pode processar a imagem.</color>");
                AppendLine("");

                UnlockSteghide();
            }
            else
            {
                AppendLine("Erro: Arquivo nao encontrado ou sintaxe de comando incorreta.");
            }

            return true;
        }

        private void UnlockSteghide()
        {
            challengeSolved = true;

            if (steghideButton != null)
                steghideButton.interactable = true;

            if (terminalSuccessPopup != null)
                terminalSuccessPopup.SetActive(true);

            Debug.Log("TerminalBoss: arquivo movido! Steghide desbloqueado.");
        }
    }
}
