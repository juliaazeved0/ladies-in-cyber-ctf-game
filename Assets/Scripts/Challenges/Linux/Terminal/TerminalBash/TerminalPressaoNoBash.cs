using UnityEngine;

namespace BashTerminal
{
    /// <summary>
    /// Terminal WebGL para o desafio "Pressao no Bash".
    /// Simula o sistema SCADA no Itaipu Parquetec (carlos@itaipu-parquetec).
    /// </summary>
    public class TerminalPressaoNoBash : BashTerminalBase
    {
        [Header("Challenge References")]
        [SerializeField] private GameObject popUpSucesso;
        [SerializeField] private GameObject popCaptureFlag;
        [SerializeField] private PulseOutline objectPulse;

        private bool hasPermission = false;
        private const string H2_RESET_PATH = "/var/www/html/h2_scada/.H2_reset.sh";

        //Identidade (Bash)
        protected override string User => "carlos";
        protected override string Hostname => "itaipu-parquetec";
        protected override string HomeDirectory => "/var/www/html/h2_scada";

        // ── Entry points ──────────────────────────────────────────────────────
        
        /// <summary>
        /// Chamado pelo botao do Terminal na UI do Unity.
        /// </summary>
        public void OnClickTerminalBash()
        {
            Debug.Log("Iniciando Terminal do Engenheiro Carlos...");
            hasPermission = false; //Reseta a permissão ao abrir para garantir a integridade do desafio
            OpenTerminal();
        }

        //Sistema de Arquivos
        protected override void SetupFilesystem()
        {
            //Limpa dicionarios da classe base par evitar duplicatas ao reabrir o terminal
            files.Clear();
            directories.Clear();
            executableFiles.Clear();
            fileOwners.Clear();

            //Configuracao de Diretorios
            AddDirectory("/var/www/html/h2_scada");
            AddDirectory("/var/www/html/h2_scada/logs");
            AddDirectory("/var/www/html/h2_scada/backups");

            //Arquivos Visiveis (dicas para a jogadora)
            AddFile("/var/www/html/h2_scada/manual_eletrolise.txt",
                "<color=#55FFFF>MANUAL DE OPERACAO</color>\n" +
                "Nota: 'Aquilo que esta parado precisa receber o poder (+x) para agir.'");

            AddFile("/var/www/html/h2_scada/log_erro.log",
                "<color=#FF5555>[CRITICO] Monitoramento parado.</color>",
                ownerGroup: "root root");

            AddFile("/var/www/html/h2_scada/config_vazamento.cfg",
                "# Configuracao de limites de vazamento\nMAX_H2_PRESSURE=120\nALERT_THRESHOLD=90");

            //Script oculto (o objetivo do desafio)
            AddFile(H2_RESET_PATH,
                "#!/bin/bash\n# Script de reset do modulo H2\necho 'Protocolo de estabilizacao iniciado...'");

            //Arquivos secundarios para imersao
            AddFile("/var/www/html/h2_scada/logs/system_uptime.log", "Sistema ativo ha 142 dias.");
            AddFile("/var/www/html/h2_scada/backups/emergency_stop_v1.sh", "#!/bin/bash\n# Parada de emergencia");
        }

        //Mensagens do Terminal
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

        //Logica de Permissoes (visual no ls -l)
        protected override string BuildPermString(string path, bool isDir, bool isExec)
        {
            //Sobrescreve visualmente as permissoes apenas do arquivo do desafio
            if(path == H2_RESET_PATH)
                return hasPermission ? "-rwxr-xr-x" : "-rw-r--r--";

            return base.BuildPermString(path, isDir, isExec);
        }

        //Comandos Especiais
        protected override bool ExecuteSpecialCommand(string cmd, string[] args, string fullCommand)
        {
            //Logica do comando CHMOD
            if(cmd == "chmod")
            {
                string argument = string.Join(" ", args);

                if(argument.Contains("+x") && argument.Contains(".H2_reset.sh"))
                {
                    hasPermission = true;

                    //Adiciona o path a lista de executaveis permitidos no sistema base
                    if(!executableFiles.Contains(H2_RESET_PATH))
                        executableFiles.Add(H2_RESET_PATH);
                    
                    AppendLine("Permissoes de execucao atualizadas para .H2_reset.sh");
                }
                else
                {
                    AppendLine("Uso: chmod +x [arquivo]");
                }
                return true;
            }

            //Verifica as diversas formas que a jogadora pode tentar rodar o script
            if(cmd == "./.H2_reset.sh" || cmd == ".H2_reset.sh" || 
               (cmd == "bash" && args.Length > 0 && args[0] == ".H2_reset.sh"))
            {
                //Verifica se a jogadora esta no diretorio correto
                if(currentDirectory != HomeDirectory)
                {
                    AppendLine($"bash: {cmd}: No such file or directory");
                    return true;
                }

                //Verifica se a permissao foi dada via chmod
                if(!hasPermission)
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
            //Obtem a flag do sistema de seguranca (SafeBase)
            string flag = SafeBase.ViewBase(SafeBase.flag_1);

            AppendLine("<color=#55FF55>> SUCESSO: Pressao do modulo H2 estabilizada. Sistema reiniciado.</color>");
            AppendLine("");
            AppendLine($"<color=#FFFF55>[FLAG: {flag}]</color>");
            AppendLine("Flag copiada paraa a Bolsa de Flags com sucesso!");

            //Salva o progresso no sistema de Flags e Desafios
            FlagManager.Instance.SaveFlag("Pressao no Bash", flag);

            if(ChallengeManager.Instance != null)
                ChallengeManager.Instance.CompleteChallenge("DesafioPressao");

            //Ativa o feedback visual de vitoria
            if(popUpSucesso != null) popUpSucesso.SetActive(true);
            if(popCaptureFlag != null) popCaptureFlag.SetActive(true);
        }

        public void OnClickButton() //Usado para coletar a flag se ja resolvido
        {
            string flag = SafeBase.ViewBase(SafeBase.flag_1);
            FlagManager.Instance.SaveFlag("Pressao no Bash", flag);

            if(popCaptureFlag != null) popCaptureFlag.SetActive(true);
        }

        public void ClosedChallenge()
        {
            //Para o efeito de pulsacao no mundo caso o desafio seja fechado com sucesso
            if(objectPulse != null && popCaptureFlag != null && popCaptureFlag.activeSelf)
                objectPulse.StopPulsing();

            CanvasManager.Instance.ClosedPanel("ChallengePressureBash");
            CanvasManager.Instance.ToggleMiniMap(true);
        }
    }
}