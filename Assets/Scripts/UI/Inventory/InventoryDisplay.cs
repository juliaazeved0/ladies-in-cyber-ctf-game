using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryDisplay : MonoBehaviour
{
    [Header("UI Text Slots")]
    public TextMeshProUGUI[] textSlots;

    [Header("Navigation Buttons")]
    public Button btnNext;
    public Button btnPrevious;

    [Header("Settings")]
    public string emptyText = "--- VACANT SLOT ---";
    
    private const int TOTAL_FLAGS = 9; 
    private int currentPage = 0;

    void OnEnable()
    {
        currentPage = 0; 
        UpdateSlots();
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
                    textSlots[i].text = capturedFlags[flagIndex];
                }
                else
                {
                    textSlots[i].text = emptyText; 
                }
            }
            else
            {
                textSlots[i].text = ""; 
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