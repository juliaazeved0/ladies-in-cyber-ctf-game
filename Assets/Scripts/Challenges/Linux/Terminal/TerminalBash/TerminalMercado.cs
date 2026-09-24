using UnityEngine;

namespace BashTerminal
{
    /// <summary>
    /// Terminal WebGL para o desafio "Mercado Escondido".
    /// Simula a estacao de trabalho do Luiz (hidrogenio-pc).
    /// Objetivo: Navegar ate ~/.backup/.old/.cache e ler notas.txt.
    /// </summary>
    public class TerminalMercado : BashTerminalBase
    {
        [Header("Challenge Settings")]
        [SerializeField] private GameObject sucessPopup;
        [SerializeField] private GameObject captureFlagPopup;

        [Tooltip("Efeito de pulsacao para destacar o objeto apos a vitoria.")]
        public PulseOutline objectPulse;

        //Identidade (Bash)
        protected override string User => "Luiz";
        protected override string Hostname => "hidrogenio-pc";
        protected override string HomeDirectory => "/home/Luiz";

        /// <summary>
        /// Finaliza o desafio e limpa o feedback visual.
        /// </summary>
        public void ClosedChallenge()
        {
            if(objectPulse != null && captureFlagPopup != null && captureFlagPopup.activeSelf)
                objectPulse.StopPulsing();

            CanvasManager.Instance.ClosedPanel("HiddenMarketChallenge");
            CanvasManager.Instance.ToggleMiniMap(true);
        }

        /// <summary>
        /// Chamado pelo botao do Teminal na cena do Mercado.
        /// </summary>
        public void OnClickTerminalMercado()
        {
            OpenTerminal();
        }

        //Sistema de Arquivos
        protected override void SetupFilesystem()
        {
            //Estrutura de Diretorios (focada em caminhos ocultos)
            AddDirectory("/home/Luiz");
            AddDirectory("/home/Luiz/documentos");
            AddDirectory("/home/Luiz/.backup");
            AddDirectory("/home/Luiz/.backup/.old");
            AddDirectory("/home/Luiz/.backup/.old/.cache");

            //Arquivos de "Distracao" e Imersao
            AddFile("/home/Luiz/sistema_2021.bak",
                "Backup corrompido - arquivo ilegivel\n0x0000: FF FE 00 01 AB CD EF 22 ...");

            AddFile("/home/Luiz/documentos/listaCompras.txt",
                "Racao cachorro\nRacao gato\nWhey sabor cafe");

            AddFile("/home/Luiz/documentos/notas.txt",
                "Pista: verifique o cache oculto.");

            //Arquivo de Vitoria: O conteudo "__victory__" eh interceptado no OnCatFile
            AddFile("/home/Luiz/.backup/.old/.cache/notas.txt", "__victory__");
        }

        //Mensagens do Terminal
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

        //Interceptacao de Vitoria via Comando 'cat'
        protected override string OnCatFile(string resolvedPath, string originalArg)
        {
            //Verifica se o caminho resolvido eh o arquivo final do desafio
            if(resolvedPath == "/home/Luiz/.backup/.old/.cache/notas.txt")
            {
                TriggerVictory();
                return ""; //Retorna vazio pois a mensagem de vitoria ja sera impressa no terminal
            }
            return null; //Deixa a base ler o conteudo normal para outros arquivos
        }

        private void TriggerVictory()
        {
            string flag = SafeBase.ViewBase(SafeBase.flag_2);

            AppendLine("<color=#55FF55>> SUCESSO: Registro oculto encontrado.</color>");
            AppendLine("Nota de Luiz: 'Nunca confie no que esta na superficie.'");
            AppendLine("");
            AppendLine($"<color=#FFFF55>[FLAG: {flag}]</color>");
            AppendLine("Flag copiada para a Bolsa de Flags com sucesso!");

            FlagManager.Instance.SaveFlag("Mercado Escondido", flag);

            if(sucessPopup != null)
                sucessPopup.SetActive(true);

            if(captureFlagPopup != null)
                captureFlagPopup.SetActive(true);
        }

        //Comndos Especiais
        protected override bool ExecuteSpecialCommand(string cmd, string[] args, string fullCommand)
        {
            //Logica customizada para o comando "cd .."
            if(cmd == "cd" && args.Length > 0 && args[0] == "..")
            {
                //Se ja estiver na Home, impede de voltar para a raiz do sistema (/) por seguranca do desafio
                if(currentDirectory == HomeDirectory)
                {
                    AppendLine("bash: cd: ja esta no diretorio home.");
                }
                else
                {
                    //Logica para subir um nivel no caminho (Path manipulation)
                    int lastSlash = currentDirectory.LastIndexOf('/');

                    if(lastSlash > 0)
                    {
                        string newPath = currentDirectory.Substring(0, lastSlash);

                        //Valida se o diretorio existe antes de mudar
                        if(directories.Contains(newPath))
                        {
                            currentDirectory = newPath;
                        }
                        else
                        {
                            currentDirectory = HomeDirectory;
                            AppendLine("");
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}