using System.Collections;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Gerencia a abertura do terminal externo para o desafio "Mercado Escondido".
/// Herda funcionalidades base do desafio de pressao para reutilizar logica de SO.
/// </summary>
public class AbrirTerminalMercado : AbrirTerminalPressaoNoBash
{
    /// <summary>
    /// Evento de clique para capturar a Flag apos resolver o desafio no terminal externo.
    /// </summary>
    public void OnButtonClickCaptureFlag()
    {
        popCaptureFlag.SetActive(true);

        //Obtem a flag especifica do desafio 2
        string newFlag = SafeBase.ViewBase(SafeBase.flag_2);
        FlagManager.Instance.SaveFlag("Mercado Escondido", newFlag);
    }

    /// <summary>
    /// Detecta o clique do objeto (computador do Luz) no cenario.
    /// </summary>
    private void OnMouseDown()
    {
        UnityEngine.Debug.Log("Acessando o computador do Luiz...");
        IniciarDesafioTerminal();
    }

    public void IniciarDesafioTerminal()
    {
        StartCoroutine(MonitorarTerminalMercado());
    }

    private IEnumerator MonitorarTerminalMercado()
    {
        string pastaStreaming = Path.GetFullPath(Application.streamingAssetsPath);
        string vitoriaPath = Path.Combine(pastaStreaming, "vitoria.txt");

        //Limpa rastro de vitorias anteriores
        if(File.Exists(vitoriaPath)) File.Delete(vitoriaPath);

        bool terminou = false;
        int exitCode = 0;

        //Implementacao explicita para garantir os caminhos corretos do Mercado
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        System.Diagnostics.Process terminal = new System.Diagnostics.Process();
        terminal.StartInfo.UseShellExecute = true;
        terminal.StartInfo.WorkingDirectory = pastaStreaming;
        string arquivoBat = Path.Combine(pastaStreaming, "DesafioMercado.bat");
        terminal.StartInfo.FileName = "cmd.exe";
        terminal.StartInfo.Arguments = $"/c \"\"{arquivoBat}\"\"";

        try { terminal.Start(); }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Erro ao abrir terminal (Windows): " + e.Message);
            yield break;
        }

        System.Threading.Thread waitWin = new System.Threading.Thread(() =>
        {
            try
            {
                terminal.WaitForExit();
                exitCode = terminal.ExitCode;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Erro aguardando terminal: " + e.Message);
            }
            finally { terminou = true; }
        });
        waitWin.IsBackground = true;
        waitWin.Start();

#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX

        int LinuxFork()
        {
            [DllImport("libc", EntryPoint = "fork", SetLastError = true)]
            static extern int fork_impl();
            return fork_impl();
        }

        int LinuxExecvp(string file, string[] argv)
        {
            [DllImport("libc", EntryPoint = "execvp", SetLastError = true)]
            static extern int execvp_impl(string file, string[] argv);
            return execvp_impl(file, argv);
        }

        int LinuxWaitpid(int pid, out int status, int options)
        {
            [DllImport("libc", EntryPoint = "waitpid", SetLastError = true)]
            static extern int waitpid_impl(int pid, out int status, int options);
            return waitpid_impl(pid, out status, options);
        }

        void LinuxExit(int status)
        {
            [DllImport("libc", EntryPoint = "_exit", SetLastError = true)]
            static extern void exit_impl(int status);
            exit_impl(status);
        }

        int LinuxChdir(string path)
        {
            [DllImport("libc", EntryPoint = "chdir", SetLastError = true)]
            static extern int chdir_impl(string path);
            return chdir_impl(path);
        }

        bool WIfExited(int status) => (status & 0x7F) == 0;
        int WExitStatus(int status) => (status >> 8) & 0xFF;

        string terminalExe = null;
        string[] candidatos = { "/usr/bin/xterm", "/usr/bin/gnome-terminal", "/usr/bin/konsole" };
        foreach (var t in candidatos)
        {
            if (File.Exists(t)) { terminalExe = t; break; }
        }

        if (terminalExe == null)
        {
            UnityEngine.Debug.LogError("Nenhum terminal encontrado (xterm, gnome-terminal, konsole)!");
            yield break;
        }

        UnityEngine.Debug.Log("Terminal encontrado: " + terminalExe);

        string capturedTerminal = terminalExe;
        string capturedScript = Path.Combine(pastaStreaming, "script_mercado.sh");

        string[] argv;
        if (capturedTerminal.Contains("gnome-terminal"))
            argv = new string[] { capturedTerminal, "--", "/bin/bash", capturedScript, null };
        else
            argv = new string[] { capturedTerminal, "-e", "/bin/bash", capturedScript, null };

        System.Threading.Thread waitLinux = new System.Threading.Thread(() =>
        {
            try
            {
                int pid = LinuxFork();

                if (pid < 0)
                {
                    UnityEngine.Debug.LogError("fork() falhou! errno: " + Marshal.GetLastWin32Error());
                    return;
                }

                if (pid == 0)
                {
                    LinuxChdir(pastaStreaming);
                    LinuxExecvp(capturedTerminal, argv);
                    LinuxExit(127);
                }
                else
                {
                    int status = 0;
                    LinuxWaitpid(pid, out status, 0);
                    exitCode = WIfExited(status) ? WExitStatus(status) : -1;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Erro no fork/exec: " + e.Message);
            }
            finally
            {
                terminou = true;
            }
        });
        waitLinux.IsBackground = true;
        waitLinux.Start();

#else
        UnityEngine.Debug.LogError("Plataforma não suportada.");
        yield break;
#endif

        bool desafioResolvido = false;
        
        //Loop de verificacao de vitoria
        while(!terminou)
        {
            if(File.Exists(vitoriaPath))
            {
                desafioResolvido = true;
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }

        //Processamento do resultado final
        if(desafioResolvido || exitCode == 99 || File.Exists(vitoriaPath))
        {
            UnityEngine.Debug.Log("Desafio do mercado resolvido! Pop-up aberto.");

            if(popUpSucesso != null)
                popUpSucesso.SetActive(true);

            if(File.Exists(vitoriaPath)) File.Delete(vitoriaPath);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Terminal fechou, mas a condição de vitória não foi atingida.");
        }
    }
}