using UnityEngine;

namespace BashTerminal
{
    /// <summary>
    /// WebGL terminal for the "Mercado Escondido" challenge.
    ///
    /// Simulates Luiz's workstation (hidrogenio-pc).
    /// Victory: navigate to ~/.backup/.old/.cache and run  cat notas.txt
    ///
    /// Unity setup:
    ///   - Attach this component to a GameObject in the Mercado challenge scene.
    ///   - Wire terminalPanel / outputText / inputField in the Inspector.
    ///   - Wire popUpSucesso and popCaptureFlag se usar.
    ///   - Hook the button OnClick -> OnClickTerminalMercado()
    ///   - Hook the close button OnClick -> CloseTerminal()
    /// </summary>
    public class TerminalMercado : BashTerminalBase
    {
        [Header("Desafio Mercado")]
        [SerializeField] private GameObject popUpSucesso;
        [SerializeField] private GameObject popCaptureFlag;

        // ── Identity ──────────────────────────────────────────────────────────
        protected override string User => "Luiz";
        protected override string Hostname => "hidrogenio-pc";
        protected override string HomeDirectory => "/home/Luiz";

        // ── Entry points ──────────────────────────────────────────────────────
        /// <summary>Call from the terminal button OnClick event.</summary>
        public void OnClickTerminalMercado()
        {
            OpenTerminal();
        }

        // ── Filesystem ────────────────────────────────────────────────────────
        protected override void SetupFilesystem()
        {
            AddDirectory("/home/Luiz");
            AddDirectory("/home/Luiz/documentos");
            AddDirectory("/home/Luiz/.backup");
            AddDirectory("/home/Luiz/.backup/.old");
            AddDirectory("/home/Luiz/.backup/.old/.cache");

            AddFile("/home/Luiz/sistema_2021.bak",
                "Backup corrompido - arquivo ilegivel\n0x0000: FF FE 00 01 AB CD EF 22 ...");

            AddFile("/home/Luiz/documentos/listaCompras.txt",
                "Racao cachorro\nRacao gato\nWhey sabor cafe");

            AddFile("/home/Luiz/documentos/notas.txt",
                "Pista: verifique o cache oculto.");

            // Victory file - its content is handled specially in OnCatFile()
            AddFile("/home/Luiz/.backup/.old/.cache/notas.txt", "__victory__");
        }

        // ── Welcome / help ────────────────────────────────────────────────────
        protected override string GetWelcomeMessage()
        {
            return "ESTACAO DE TRABALHO - ANALISTA LUIZ\nDigite 'help' para comandos.";
        }

        protected override string GetHelpText()
        {
            return
                "<color=#FFFF55>Comandos disponiveis:</color>\n" +
                "  ls         - Lista os arquivos e pastas no diretorio atual.\n" +
                "  ls -a      - Lista todos os arquivos, incluindo os <color=#FF5555>ocultos</color>.\n" +
                "  ls -l      - Exibe os arquivos em formato de lista detalhada.\n" +
                "  ls -la     - Combina parametros de arquivos ocultos e detalhes.\n" +
                "  cd [pasta] - Entra em uma pasta especifica. Use 'cd ..' para voltar.\n" +
                "  cat [arq]  - Le e exibe o conteudo de um arquivo de texto.\n" +
                "  clear      - Limpa a tela do terminal.\n" +
                "  exit       - Fecha o terminal.";
        }

        // ── Victory interception via cat ──────────────────────────────────────
        protected override string OnCatFile(string resolvedPath, string originalArg)
        {
            if (resolvedPath == "/home/Luiz/.backup/.old/.cache/notas.txt")
            {
                TriggerVictory();
                return "";
            }
            return null;
        }

        private void TriggerVictory()
        {
            string flag = SafeBase.ViewBase(SafeBase.flag_2);

            AppendLine("<color=#55FF55>> SUCESSO: Registro oculto encontrado.</color>");
            AppendLine("Nota de Luiz: 'Nunca confie no que esta na superficie.'");
            AppendLine("");
            AppendLine($"<color=#FFFF55>[FLAG: {flag}]</color>");
            AppendLine("");

            FlagManager.Instance.SaveFlag("Mercado Escondido", flag);

            if (popUpSucesso != null)
                popUpSucesso.SetActive(true);

            if (popCaptureFlag != null)
                popCaptureFlag.SetActive(true);
        }
    }
}
