using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class FileNavigationManager : MonoBehaviour
{
    [Header("Settings Folders Navigation")]
    public GameObject rootFolder;
    public Button backButton;
    public TextMeshProUGUI pathText;

    public Transform popUpConatiner;

    public GameObject boardFolders;

    private Stack<GameObject> historyStack = new Stack<GameObject>();
    private GameObject currentOpenFolder;

    void Start()
    {
       foreach (Transform child in popUpConatiner)
       {
            if(child.name == "pathFolder") continue;
            if(child.name == "BackButton") continue;

            child.gameObject.SetActive(false);
        }


        if(rootFolder != null)
        {
            currentOpenFolder = rootFolder;
            currentOpenFolder.SetActive(true);
            pathText.text ="/Desktop";
        }

        UpdateBackButton();
       
    }

    public void OpenFolder(Folder folder)
    {
        if(folder.contentFolder == currentOpenFolder) return;
        
        historyStack.Push(currentOpenFolder);
        currentOpenFolder.SetActive(false);

        currentOpenFolder = folder.contentFolder;
        currentOpenFolder.SetActive(true);

        pathText.text = "/" + folder.folderName;

        UpdateBackButton();
    }

    public void UpdateBackButton()
    {
        backButton.interactable = (historyStack.Count > 0);
    }

    public void GoBack()
    {
        if(historyStack.Count > 0)
        {
            currentOpenFolder.SetActive(false);

            currentOpenFolder = historyStack.Pop();

            currentOpenFolder.SetActive(true);

            pathText.text = "/" + currentOpenFolder.name;

            UpdateBackButton();
        }
    }

    public void ClosePopUp()
    {
        boardFolders.SetActive(false);
        
    }

}
