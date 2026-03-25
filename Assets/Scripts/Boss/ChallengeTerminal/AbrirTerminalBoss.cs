using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class AbrirTerminalBoss : MonoBehaviour
{
    [Header("Configurações do Desafio")]
    public Button botaoSteghide; // Botão que será desbloqueado
    public GameObject popUpSucessoTerminal; // Feedback visual que o terminal foi resolvido

    private void OnMouseDown()
    {
        UnityEngine.Debug.Log("Tentando acessar o computador do BOSS...");
        IniciarDesafio();
    }

    public void IniciarDesafio()
    {
        StartCoroutine(MonitorarTerminalBoss());
    }

    private IEnumerator MonitorarTerminalBoss()
    {
        string pastaStreaming = Path.GetFullPath(Application.streamingAssetsPath);
        string vitoriaPath = Path.Combine(pastaStreaming, "boss_resolvido.txt");

        // Limpa vitórias antigas
        if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);

        Process terminal = new Process();
        terminal.StartInfo.UseShellExecute = true; 
        terminal.StartInfo.WorkingDirectory = pastaStreaming;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string arquivoBat = Path.Combine(pastaStreaming, "DesafioBoss.bat");
            terminal.StartInfo.FileName = "cmd.exe";
            terminal.StartInfo.Arguments = $"/c \"\"{arquivoBat}\"\"";
        }
        else if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
        {
            string arquivoScript = Path.Combine(pastaStreaming, "script_boss.sh");
            Process.Start("chmod", "+x " + arquivoScript);
            terminal.StartInfo.FileName = "/usr/bin/xterm"; 
            terminal.StartInfo.Arguments = $"-e /bin/bash {arquivoScript}";
        }

        try { terminal.Start(); }
        catch (Exception e) { UnityEngine.Debug.LogError("Erro: " + e.Message); yield break; }

        while (!terminal.HasExited) { yield return null; }

        // Se o arquivo de vitória existir, o comando 'mv' foi executado corretamente
        if (File.Exists(vitoriaPath))
        {
            DesbloquearSteghide();
            if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);
        }
    }

    void DesbloquearSteghide()
    {
        if (botaoSteghide != null) botaoSteghide.interactable = true;
        if (popUpSucessoTerminal != null) popUpSucessoTerminal.SetActive(true);
        UnityEngine.Debug.Log("O arquivo foi movido! Steghide desbloqueado.");
    }
}