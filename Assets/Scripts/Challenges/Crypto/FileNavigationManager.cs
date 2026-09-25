using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gerencia a navegacao entre pastas do sistema de arquivos simulado:
/// abre/fecha o conteudo de cada pasta, mantem um historico para o botao
/// "voltar" e atualiza o texto de caminho (breadcrumb) exibido na UI.
/// </summary>
public class FileNavigationManager : MonoBehaviour
{
    [Header("Settings Folders Navigation")]
    [Tooltip("Pasta raiz exibida ao iniciar (ex: Desktop).")]
    public GameObject rootFolder;

    [Tooltip("Botao para voltar a pasta anterior.")]
    public Button backButton;

    [Tooltip("Texto que exibe o caminho (breadcrumb) da pasta atual.")]
    public TextMeshProUGUI pathText;

    [Tooltip("Container onde ficam os elementos do pop-up de navegacao de arquivos.")]
    public Transform popUpConatiner;

    [Tooltip("Painel/quadro que exibe as pastas.")]
    public GameObject boardFolders;

    [Tooltip("Botao para reabrir o pop-up de navegacao.")]
    public GameObject buttonEnter;

    //Historico de pastas visitadas, usado pelo botao "voltar"
    private Stack<GameObject> historyStack = new Stack<GameObject>();

    //Pasta (conteudo) atualmente aberta
    private GameObject currentOpenFolder;

    void Start()
    {
        ResetNavigation();
    }

    public void ResetNavigation()
    {
        historyStack.Clear();
        if(popUpConatiner != null)
        {
            foreach(Transform child in popUpConatiner)
            {
                if(child.name == "pathFolder") continue;
                if(child.name == "BackButton") continue;

                child.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("[FileNavigationManager] popUpConatiner não está atribuído no Inspector.");
        }

        if(rootFolder != null)
        {
            currentOpenFolder = rootFolder;
            currentOpenFolder.SetActive(true);

            if(pathText != null)
                pathText.text = "/Desktop";
            else
                Debug.LogError("[FileNavigationManager] pathText não está atribuído no Inspector.");
        }

        UpdateBackButton();
    }

    /// <summary>
    /// Abre o conteudo da pasta informada, fechando a pasta atual e
    /// empilhando-a no historico para permitir voltar depois.
    /// </summary>
    public void OpenFolder(Folder folder)
    {
        if(folder == null)
        {
            Debug.LogError("[FileNavigationManager] OpenFolder chamado com folder nulo.");
            return;
        }

        if(folder.contentFolder == null)
        {
            Debug.LogError("[FileNavigationManager] contentFolder da pasta \"" + folder.folderName + "\" não está atribuído.");
            return;
        }

        if(folder.contentFolder == currentOpenFolder)
        {
            currentOpenFolder.SetActive(true);
            return;
        }

        if(currentOpenFolder != null) historyStack.Push(currentOpenFolder);

        if(currentOpenFolder != null)
            currentOpenFolder.SetActive(false);

        currentOpenFolder = folder.contentFolder;
        currentOpenFolder.SetActive(true);

        if(pathText != null)
            pathText.text = "/" + folder.folderName;
        else
            Debug.LogError("[FileNavigationManager] pathText não está atribuído no Inspector.");

        UpdateBackButton();
    }

    /// <summary>
    /// Atualiza se o botao de voltar esta interagivel, com base em haver
    /// ou nao historico de navegacao.
    /// </summary>
    public void UpdateBackButton()
    {
        if(backButton != null)
        {
            backButton.interactable = (historyStack.Count > 0);
        }
        else
        {
            Debug.LogError("[FileNavigationManager] backButton não está atribuído no Inspector.");
        }
    }

    //Volta para a pasta anterior do historico
    public void GoBack()
    {
        if(historyStack.Count > 0)
        {
            if(currentOpenFolder != null)
                currentOpenFolder.SetActive(false);

            currentOpenFolder = historyStack.Pop();

            if(currentOpenFolder != null)
                currentOpenFolder.SetActive(true);

            if(pathText != null)
                pathText.text = "/" + currentOpenFolder.name;
            else
                Debug.LogError("[FileNavigationManager] pathText não está atribuído no Inspector.");

            UpdateBackButton();
        }
    }

    /// <summary>
    /// Fecha o pop-up de navegacao de arquivos, escondendo o conteudo
    /// exibido e reabrindo o botao de entrada.
    /// </summary>
    public void ClosePopUp()
    {
        ResetNavigation();

        if(boardFolders != null)
            boardFolders.SetActive(false);
        else
            Debug.LogError("[FileNavigationManager] boardFolders não está atribuído no Inspector.");

        if(buttonEnter != null)
            buttonEnter.SetActive(true);
        else
            Debug.LogError("[FileNavigationManager] buttonEnter não está atribuído no Inspector.");
    }
}