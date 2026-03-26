using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices; //Biblioteca necessária para receber comandos do sistema Linux
using UnityEngine.UI;

public class AbrirTerminalBoss : MonoBehaviour
{
    [Header("Configurações do Desafio")]
    public Button steghideButton; //Botão que será desbloqueado na interface
    public GameObject terminalSuccessPopup;

    public static bool challengeSolved; //Variável global para o estado do desafio (resolvido ou não)
    private void OnMouseDown() //Chamado quando a jogadora clicar em um objeto
    {
        UnityEngine.Debug.Log("Tentando acessar o computador do BOSS...");
        StartChallenge();
    }

    public void StartChallenge()
    {
        challengeSolved = false; //Garante que o desafio começa bloqueado (pois não foi resolvido)
        StartCoroutine(MonitorTerminalBoss()); //Inicia o processo de vigia
    }

    private IEnumerator MonitorTerminalBoss()
    {
        //Define o caminho da pasta StreamingAssets e do arquivo de vitória
        string streamingFolder = Path.GetFullPath(Application.streamingAssetsPath);
        string victoryPath = Path.Combine(streamingFolder, "boss_resolvido.txt");

        if(File.Exists(victoryPath)) File.Delete(victoryPath); //Deleta o arquivo de vitória antigo se ele existir

        bool finished = false; //Variável booleana para controlar o estado de finalizado ou não

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        //Lógica para WINDOWS
        System.Diagnostics.Process terminal = new System.Diagnostics.Process();
        terminal.StartInfo.UseShellExecute = true;
        terminal.StartInfo.WorkingDirectory = streamingFolder;
        string fileBat = Path.Combine(streamingFolder, "DesafioBoss.bat");
        terminal.StartInfo.FileName = "cmd.exe";
        terminal.StartInfo.Arguments = $"/c \"\"{fileBat}\"\""; //Executa o comando e depois termina

        try { terminal.Start(); }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Erro ao abrir terminal (Windows): " + e.Message);
            yield break;
        }

        //Cria uma Thread para não travar a Unity enquanto espera o terminal fechar
        System.Threading.Thread waitWin = new System.Threading.Thread(() =>
        {
            try { terminal.WaitForExit(); }
            catch (Exception e) { UnityEngine.Debug.LogError("Erro aguardando terminal: " + e.Message); }
            finally { finished = true; } //Avisa que o terminal foi fechado
        });

        waitWin.IsBackground = true;
        waitWin.Start();

#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX

        //Lógica para LINUX: fork + exec via P/Invoke — IL2CPP safe
        //Busca por um xterm ou gnome-terminal e executa o .sh
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
        string capturedScript   = Path.Combine(pastaStreaming, "script_boss.sh");
        string[] argv           = new string[] { capturedTerminal, "-e", "/bin/bash", capturedScript, null };

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
                terminou = true;
            }
        });
        waitLinux.IsBackground = true;
        waitLinux.Start();

#else
        UnityEngine.Debug.LogError("Plataforma não suportada.");
        yield break;
#endif

        //A Unity fica esperando aqui até a Thread marcar "finished = true"
        while (!finished)
            yield return null;

        UnityEngine.Debug.Log("Terminal Boss fechou.");

        if (File.Exists(victoryPath)) //Se o arquivo de vitória foi criado pelo script externo, a jogadora venceu
        {
            UnlockSteghide();
            File.Delete(victoryPath); //Limpa para a próxima vez
        }
    }

    void UnlockSteghide()
    {
        challengeSolved = true; //Marca que o desafio foi resolvido
        if (steghideButton != null) steghideButton.interactable = true;
        //if (terminalSuccessPopup != null) terminalSuccessPopup.SetActive(true);
        UnityEngine.Debug.Log("O arquivo foi movido! Steghide desbloqueado.");
    }
}