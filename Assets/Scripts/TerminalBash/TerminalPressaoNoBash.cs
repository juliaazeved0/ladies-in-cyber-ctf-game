using UnityEngine;

namespace BashTerminal
{
    /// <summary>
    /// WebGL terminal for the "Pressao no Bash" challenge.
    /// Simulates the SCADA system at Itaipu Parquetec (carlos@itaipu-parquetec).
    /// </summary>
    public class TerminalPressaoNoBash : BashTerminalBase
    {
        [Header("Desafio Pressao no Bash - Referencias")]
        [SerializeField] private GameObject popUpSucesso;
        [SerializeField] private GameObject popCaptureFlag;
        [SerializeField] private PulseOutline objectPulse;

        private bool hasPermission = false;
        private const string H2_RESET_PATH = "/var/www/html/h2_scada/.H2_reset.sh";

        // ── Identity ──────────────────────────────────────────────────────────
        protected override string User => "carlos";
        protected override string Hostname => "itaipu-parquetec";
        protected override string HomeDirectory => "/var/www/html/h2_scada";

        // ── Entry points ──────────────────────────────────────────────────────
        
        /// <summary>Chamado pelo botão do Terminal no Unity</summary>
        public void OnClickTerminalBash()
        {
            Debug.Log("Iniciando terminal do Engenheiro Carlos...");
            hasPermission = false; // Reset da permissão ao abrir
            OpenTerminal();
        }

        // ── Filesystem ────────────────────────────────────────────────────────
        protected override void SetupFilesystem()
        {
            // Limpa dicionários para evitar duplicação ao reabrir
            files.Clear();
            directories.Clear();
            executableFiles.Clear();
            fileOwners.Clear();

            // Diretorios
            AddDirectory("/var/www/html/h2_scada");
            AddDirectory("/var/www/html/h2_scada/logs");
            AddDirectory("/var/www/html/h2_scada/backups");

            // Arquivos Visíveis
            AddFile("/var/www/html/h2_scada/manual_eletrolise.txt",
                "<color=#55FFFF>MANUAL DE OPERACAO</color>\n" +
                "Nota: 'Aquilo que esta parado precisa receber o poder (+x) para agir.'");

            AddFile("/var/www/html/h2_scada/log_erro.log",
                "<color=#FF5555>[CRITICO] Monitoramento parado.</color>",
                ownerGroup: "root root");

            AddFile("/var/www/html/h2_scada/config_vazamento.cfg",
                "# Configuracao de limites de vazamento\nMAX_H2_PRESSURE=120\nALERT_THRESHOLD=90");

            // Script Oculto (Começa com ponto)
            AddFile(H2_RESET_PATH,
                "#!/bin/bash\n# Script de reset do modulo H2\necho 'Protocolo de estabilizacao iniciado...'");

            // Logs e Backups
            AddFile("/var/www/html/h2_scada/logs/system_uptime.log", "Sistema ativo ha 142 dias.");
            AddFile("/var/www/html/h2_scada/backups/emergency_stop_v1.sh", "#!/bin/bash\n# Parada de emergencia");
        }

        // ── Welcome / help ────────────────────────────────────────────────────
        protected override string GetWelcomeMessage()
        {
            return "<color=#FFFF55>ACESSO RESTRITO - ITAIPU PARQUETEC - MONITORAMENTO H2</color>\n" +
                   "Sessao iniciada para: Engenheiro Carlos\n" +
                   "Digite 'help' para comandos.";
        }

        protected override string GetHelpText()
        {
            return "<color=#FFFF55>Comandos disponiveis:</color>\n" +
                   "  ls [-a|-l]   - Lista arquivos (use -a para ocultos).\n" +
                   "  cd [pasta]   - Navega entre diretorios.\n" +
                   "  cat [arq]    - Le o conteudo de um arquivo.\n" +
                   "  chmod +x     - Da permissao de execucao.\n" +
                   "  ./[arquivo]  - Executa um script.\n" +
                   "  clear        - Limpa o terminal.\n" +
                   "  exit         - Fecha o terminal.";
        }

        // ── Override de Permissões (Visual no ls -l) ─────────────────────────
        protected override string BuildPermString(string path, bool isDir, bool isExec)
        {
            if (path == H2_RESET_PATH)
                return hasPermission ? "-rwxr-xr-x" : "-rw-r--r--";

            return base.BuildPermString(path, isDir, isExec);
        }

        // ── Comandos Especiais: chmod + execução ──────────────────────────────
        protected override bool ExecuteSpecialCommand(string cmd, string[] args, string fullCommand)
        {
            // Lógica do CHMOD
            if (cmd == "chmod")
            {
                string argument = string.Join(" ", args);
                if (argument.Contains("+x") && argument.Contains(".H2_reset.sh"))
                {
                    hasPermission = true;
                    // Adiciona o path completo à lista de executáveis da base
                    if (!executableFiles.Contains(H2_RESET_PATH))
                        executableFiles.Add(H2_RESET_PATH);
                    
                    AppendLine("Permissoes de execucao atualizadas para .H2_reset.sh");
                }
                else
                {
                    AppendLine("Uso: chmod +x [arquivo]");
                }
                return true;
            }

            // Lógica de Execução (./)
            if (cmd == "./.H2_reset.sh" || cmd == ".H2_reset.sh" || 
               (cmd == "bash" && args.Length > 0 && args[0] == ".H2_reset.sh"))
            {
                if (currentDirectory != HomeDirectory)
                {
                    AppendLine($"bash: {cmd}: No such file or directory");
                    return true;
                }

                if (!hasPermission)
                {
                    AppendLine("<color=#FF5555>bash: ./.H2_reset.sh: Permission denied</color>");
                }
                else
                {
                    AppendLine("<color=#55FF55>> ACESSO AUTORIZADO. INICIANDO RESET DO SISTEMA...</color>");
                    TriggerVictory();
                }
                return true;
            }

            return false;
        }

        private void TriggerVictory()
        {
            // Obtém a flag do sistema de segurança
            string flag = SafeBase.ViewBase(SafeBase.flag_1);

            AppendLine("<color=#55FF55>> SUCESSO: Pressao do modulo H2 estabilizada. Sistema reiniciado.</color>");
            AppendLine("");
            AppendLine($"<color=#FFFF55>[FLAG: {flag}]</color>");
            AppendLine("");

            // Salva o progresso
            FlagManager.Instance.SaveFlag("Pressao no Bash", flag);

            if (ChallengeManager.Instance != null)
                ChallengeManager.Instance.CompleteChallenge("DesafioPressao");

            // Ativa os Pop-ups de sucesso
            if (popUpSucesso != null) popUpSucesso.SetActive(true);
            if (popCaptureFlag != null) popCaptureFlag.SetActive(true);
        }

        // --- Eventos Adicionais ---

        public void OnClickButton() // Usado para coletar a flag se já resolvido
        {
            string flag = SafeBase.ViewBase(SafeBase.flag_1);
            FlagManager.Instance.SaveFlag("Pressao no Bash", flag);
            if (popCaptureFlag != null) popCaptureFlag.SetActive(true);
        }

        public void ClosedChallenge()
        {
            if (objectPulse != null && popCaptureFlag != null && popCaptureFlag.activeSelf)
                objectPulse.StopPulsing();

            CanvasManager.Instance.ClosedPanel("ChallengePressureBash");
        }
    }
}