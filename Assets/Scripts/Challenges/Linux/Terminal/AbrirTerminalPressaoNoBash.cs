using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices;

public class AbrirTerminalPressaoNoBash : MonoBehaviour
{
    [Header("Configurações de UI")]
    public GameObject popUpSucesso;

    public GameObject popCaptureFlag;
    public GameObject captureFlag;
    public GameObject panelChallengePressureBash;
    public PulseOutline objectPulse;

    void Start()
    {
        popUpSucesso.SetActive(false);
        popCaptureFlag.SetActive(false);
    }

    public void IniciarDesafio()
    {
        StartCoroutine(MonitorarTerminal());
    }

    private IEnumerator MonitorarTerminal()
    {
        string pastaStreaming = Path.GetFullPath(Application.streamingAssetsPath);
        string vitoriaPath = Path.Combine(pastaStreaming, "vitoria_h2.txt");

        if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);

        bool terminou = false;
        int exitCode = 0;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        System.Diagnostics.Process terminal = new System.Diagnostics.Process();
        terminal.StartInfo.UseShellExecute = true;
        terminal.StartInfo.WorkingDirectory = pastaStreaming;
        string arquivoBat = Path.Combine(pastaStreaming, "DesafioHidrogenio.bat");
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
        string capturedScript = Path.Combine(pastaStreaming, "script_h2.sh");

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

        while (!terminou)
        {
            if (File.Exists(vitoriaPath))
            {
                desafioResolvido = true;
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (desafioResolvido || exitCode == 99 || File.Exists(vitoriaPath))
        {
            UnityEngine.Debug.Log("Protocolo H2 estabilizado! Pop-up aberto.");

            if (popUpSucesso != null)
                popUpSucesso.SetActive(true);

            if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);
        }
    }

    public void OnClickButton()
    {
        popCaptureFlag.SetActive(true);
        string newFlag = SafeBase.ViewBase(SafeBase.flag_1);
        FlagManager.Instance.SaveFlag("Pressão no Bash", newFlag);

        if (ChallengeManager.Instance != null)
        {
            ChallengeManager.Instance.CompleteChallenge("DesafioPressao");
        }
    }

    public void ClosedChallenge()
    {
        if (popCaptureFlag.activeSelf)
        {
            objectPulse.StopPulsing();
            CanvasManager.Instance.ClosedPanel("ChallengePressureBash");
            CanvasManager.Instance.ToggleMiniMap(true);
        }
        CanvasManager.Instance.ClosedPanel("ChallengePressureBash");
        CanvasManager.Instance.ToggleMiniMap(true);
    }
}
