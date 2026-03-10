using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using System;

public class AbrirTerminalPressaoNoBash : MonoBehaviour
{
    [Header("Configurações de UI")]
    public GameObject popUpSucesso; 

    public GameObject popCaptureFlag;
    public GameObject captureFlag;
    public GameObject panelChallengePressureBash;
    public PulseOutline objectPulse;

    void Start(){
        popUpSucesso.SetActive(false);
        popCaptureFlag.SetActive(false);
    }

   // private void OnMouseDown()
    //{
    //    UnityEngine.Debug.Log("Iniciando acesso ao sistema SCADA - Itaipu Parquetec...");
    //    IniciarDesafio();
    //}

    public void IniciarDesafio()
    {
        StartCoroutine(MonitorarTerminal());
    }

    private IEnumerator MonitorarTerminal()
    {
        string pastaStreaming = Path.GetFullPath(Application.streamingAssetsPath);
        string vitoriaPath = Path.Combine(pastaStreaming, "vitoria_h2.txt");

        // Limpa vitórias antigas para evitar falsos positivos
        if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);

        Process terminal = new Process();
        terminal.StartInfo.UseShellExecute = true; 
        terminal.StartInfo.WorkingDirectory = pastaStreaming;

        // --- Configuração Multiplataforma ---
        
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string arquivoBat = Path.Combine(pastaStreaming, "DesafioHidrogenio.bat");
            terminal.StartInfo.FileName = "cmd.exe";
            terminal.StartInfo.Arguments = $"/c \"\"{arquivoBat}\"\"";
        }
        else if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
        {
            string arquivoScript = Path.Combine(pastaStreaming, "script_h2.sh");
            Process.Start("chmod", "+x " + arquivoScript);

            terminal.StartInfo.FileName = "/usr/bin/xterm"; 
            terminal.StartInfo.Arguments = $"-e /bin/bash {arquivoScript}";
        }

        try 
        {
            terminal.Start();
        }
        catch (Exception e) 
        {
            UnityEngine.Debug.LogError("Erro ao abrir terminal: " + e.Message);
            yield break;
        }

        while (!terminal.HasExited)
        {
            yield return null; 
        }

        UnityEngine.Debug.Log("Terminal do Carlos fechou. Codigo: " + terminal.ExitCode);

        // VITÓRIA: Abre o pop-up se o código for 99 OU se o arquivo de vitoria existir
        if (terminal.ExitCode == 99 || File.Exists(vitoriaPath))
        {
            if (popUpSucesso != null) 
            {
                popUpSucesso.SetActive(true);
                UnityEngine.Debug.Log("Protocolo H2 estabilizado! Pop-up aberto.");
            }
            
            // Deleta o arquivo após o uso
            if (File.Exists(vitoriaPath)) File.Delete(vitoriaPath);
        }
    }

    public void OnClickButton()
    {
        popCaptureFlag.SetActive(true);
    }

    public void ClosedChallenge()
    {
        if(popCaptureFlag.activeSelf)
        {
            objectPulse.StopPulsing();
            CanvasManager.Instance.ClosedPanel("ChallengePressureBash");
        }
        CanvasManager.Instance.ClosedPanel("ChallengePressureBash");
        
    }
}