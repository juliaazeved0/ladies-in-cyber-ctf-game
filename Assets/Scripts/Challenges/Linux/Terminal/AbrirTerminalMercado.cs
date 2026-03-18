using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System;

public class AbrirTerminalMercado : AbrirTerminalPressaoNoBash
{

    public void OnButtonClickCaptureFlag(){
        popCaptureFlag.SetActive(true);
        string newFlag = SafeBase.ViewBase(SafeBase.flag_2);
        FlagManager.Instance.SaveFlag(newFlag);
    }

    private void OnMouseDown()
    {
        UnityEngine.Debug.Log("Acessando computador do Luiz...");
        IniciarDesafio();
    }

    public void IniciarDesafioTerminal()
    {
        StartCoroutine(MonitorarTerminal());
    }

    private IEnumerator MonitorarTerminal()
    {
        string pastaStreaming = Path.GetFullPath(Application.streamingAssetsPath);
        string vitoriaPath = Path.Combine(pastaStreaming, "vitoria.txt");

        // Limpa vitórias antigas antes de começar
        if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);

        Process terminal = new Process();
        terminal.StartInfo.UseShellExecute = true; 
        terminal.StartInfo.WorkingDirectory = pastaStreaming;

        // Configuração para WINDOWS
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string arquivoBat = Path.Combine(pastaStreaming, "DesafioMercado.bat");
            terminal.StartInfo.FileName = "cmd.exe";
            terminal.StartInfo.Arguments = $"/c \"\"{arquivoBat}\"\"";
        }
        // Configuração para LINUX
        else if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
        {
            string arquivoScript = Path.Combine(pastaStreaming, "script_mercado.sh");
            Process.Start("chmod", "+x " + arquivoScript);

            terminal.StartInfo.FileName = "/usr/bin/xterm"; 
            terminal.StartInfo.Arguments = $"-e /bin/bash {arquivoScript}";
        }

        try {
            terminal.Start();
        } catch (Exception e) {
            UnityEngine.Debug.LogError("Erro ao abrir terminal: " + e.Message);
            yield break;
        }

        while (!terminal.HasExited)
        {
            yield return null; 
        }

        UnityEngine.Debug.Log("Terminal fechou. Código: " + terminal.ExitCode);

        // VITÓRIA: Verifica o código 99 OU se o script criou o arquivo vitoria.txt
        if (terminal.ExitCode == 99 || File.Exists(vitoriaPath))
        {
            if (popUpSucesso != null) 
            {
                popUpSucesso.SetActive(true);
                UnityEngine.Debug.Log("Pop-up ativado!");
            }
            
            // Deleta o arquivo após usar para não travar o próximo teste
            if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);
        }
    }
}