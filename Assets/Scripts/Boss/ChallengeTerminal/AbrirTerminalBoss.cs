using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices; 
using UnityEngine.UI;

public class AbrirTerminalBoss : MonoBehaviour
{
    [Header("Configurações do Desafio")]
    public Button steghideButton; 
    public GameObject terminalSuccessPopup;

    public static bool challengeSolved; 
    
    // Variável de controle para o loop da Coroutine
    private bool finished = false;

    // --- Importações para LINUX (Devem ficar fora dos métodos) ---
    #if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
    [DllImport("libc", EntryPoint = "fork", SetLastError = true)]
    private static extern int LinuxFork();

    [DllImport("libc", EntryPoint = "execvp", SetLastError = true)]
    private static extern int LinuxExecvp(string file, string[] argv);

    [DllImport("libc", EntryPoint = "waitpid", SetLastError = true)]
    private static extern int LinuxWaitpid(int pid, out int status, int options);

    [DllImport("libc", EntryPoint = "_exit", SetLastError = true)]
    private static extern void LinuxExit(int status);
    #endif

    private void OnMouseDown() 
    {
        UnityEngine.Debug.Log("Tentando acessar o computador do BOSS...");
        StartChallenge();
    }

    public void StartChallenge()
    {
        challengeSolved = false; 
        finished = false; // Resetar o estado antes de começar
        StartCoroutine(MonitorTerminalBoss()); 
    }

    private IEnumerator MonitorTerminalBoss()
    {
        string streamingFolder = Path.GetFullPath(Application.streamingAssetsPath);
        string victoryPath = Path.Combine(streamingFolder, "boss_resolvido.txt");

        if(File.Exists(victoryPath)) File.Delete(victoryPath); 

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        System.Diagnostics.Process terminal = new System.Diagnostics.Process();
        terminal.StartInfo.UseShellExecute = true;
        terminal.StartInfo.WorkingDirectory = streamingFolder;
        string fileBat = Path.Combine(streamingFolder, "DesafioBoss.bat");
        terminal.StartInfo.FileName = "cmd.exe";
        terminal.StartInfo.Arguments = $"/c \"\"{fileBat}\"\""; 

        try { terminal.Start(); }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Erro ao abrir terminal (Windows): " + e.Message);
            yield break;
        }

        System.Threading.Thread waitWin = new System.Threading.Thread(() =>
        {
            try { terminal.WaitForExit(); }
            catch (Exception e) { UnityEngine.Debug.LogError("Erro aguardando terminal: " + e.Message); }
            finally { finished = true; } 
        });

        waitWin.IsBackground = true;
        waitWin.Start();

#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX

        string terminalExe = null;
        string[] candidatos = { "/usr/bin/xterm", "/usr/bin/gnome-terminal", "/usr/bin/konsole" };
        foreach (var t in candidatos)
        {
            if (File.Exists(t)) { terminalExe = t; break; }
        }

        if (terminalExe == null)
        {
            UnityEngine.Debug.LogError("Nenhum terminal encontrado!");
            yield break;
        }

        string capturedTerminal = terminalExe;
        // CORREÇÃO AQUI: Usando Application.streamingAssetsPath
        string capturedScript = Path.Combine(Application.streamingAssetsPath, "script_boss.sh");
        string[] argv = new string[] { capturedTerminal, "-e", "/bin/bash", capturedScript, null };

        System.Threading.Thread waitLinux = new System.Threading.Thread(() =>
        {
            try
            {
                int pid = LinuxFork();

                if (pid < 0)
                {
                    UnityEngine.Debug.LogError("fork() falhou!");
                    return;
                }

                if (pid == 0)
                {
                    LinuxExecvp(capturedTerminal, argv);
                    LinuxExit(127);
                }
                else
                {
                    int status = 0;
                    LinuxWaitpid(pid, out status, 0);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Erro no fork/exec: " + e.Message);
            }
            finally
            {
                finished = true; // CORREÇÃO: Usando a mesma variável 'finished'
            }
        });
        waitLinux.IsBackground = true;
        waitLinux.Start();

#else
        UnityEngine.Debug.LogError("Plataforma não suportada.");
        yield break;
#endif

        // Espera o terminal fechar
        while (!finished)
            yield return null;

        UnityEngine.Debug.Log("Terminal Boss fechou.");

        if (File.Exists(victoryPath)) 
        {
            UnlockSteghide();
            File.Delete(victoryPath); 
        }
    }

    void UnlockSteghide()
    {
        challengeSolved = true; 
        if (steghideButton != null) steghideButton.interactable = true;
        UnityEngine.Debug.Log("O arquivo foi movido! Steghide desbloqueado.");
    }
}