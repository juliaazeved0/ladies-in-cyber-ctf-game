using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryDisplay : MonoBehaviour
{
    [Header("UI Text Slots")]
    public TextMeshProUGUI[] textSlots;

    [Header("UI Lock Icons (Empty State)")]
    public GameObject[] lockIcons; 

    [Header("Copy Buttons")]
    public GameObject[] copyButtons; 

    [Header("Navigation Buttons")]
    public Button btnNext;
    public Button btnPrevious;
    
    private const int TOTAL_FLAGS = 9; 
    private int currentPage = 0;


    void OnEnable()
    {
        currentPage = 0; 
        
        if (FlagManager.Instance != null)
        {
            UpdateSlots();
        }
    }

    public void NextPage()
    {
        int totalPages = Mathf.CeilToInt((float)TOTAL_FLAGS / textSlots.Length);
        
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            UpdateSlots();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateSlots();
        }
    }

    private void UpdateSlots()
    {
        List<string> capturedFlags = FlagManager.Instance.flagsCapture;
        int startIndex = currentPage * textSlots.Length;

        for (int i = 0; i < textSlots.Length; i++)
        {
            int flagIndex = startIndex + i;

            if (flagIndex < TOTAL_FLAGS)
            {
                if (flagIndex < capturedFlags.Count)
                {
                    if (textSlots[i] != null) 
                    {
                        textSlots[i].gameObject.SetActive(true);
                        textSlots[i].text = capturedFlags[flagIndex];
                    }
                    if (i < lockIcons.Length && lockIcons[i] != null) lockIcons[i].SetActive(false);
                    if (i < copyButtons.Length && copyButtons[i] != null) copyButtons[i].SetActive(true);
                }
                else
                {
                    if (textSlots[i] != null) textSlots[i].gameObject.SetActive(false);
                    if (i < lockIcons.Length && lockIcons[i] != null) lockIcons[i].SetActive(true);
                    if (i < copyButtons.Length && copyButtons[i] != null) copyButtons[i].SetActive(false);
                }
            }
            else
            {
                if (textSlots[i] != null) textSlots[i].gameObject.SetActive(false);
                if (i < lockIcons.Length && lockIcons[i] != null) lockIcons[i].SetActive(false);
                if (i < copyButtons.Length && copyButtons[i] != null) copyButtons[i].SetActive(false);
            }
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        int totalPages = Mathf.CeilToInt((float)TOTAL_FLAGS / textSlots.Length);
        
        if (btnPrevious != null) btnPrevious.interactable = (currentPage > 0);
        if (btnNext != null) btnNext.interactable = (currentPage < totalPages - 1);
    }
}