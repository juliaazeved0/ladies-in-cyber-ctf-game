using UnityEngine;
using UnityEngine.UI;

public class PlaybookManager : MonoBehaviour
{
    public Sprite[] bookPages; 
    private int indexCurrent = 0;
    public Image basePageLocal; 

    [Header("Buttons")]
    public Button buttonNext;
    public Button buttonBack;

    void OnEnable()
    {
        indexCurrent = 0;
        UpdatePage();
    }

    public void NextPage()
    {
        if (indexCurrent < bookPages.Length - 1)
        {
            indexCurrent++;
            UpdatePage();
        }
    }

    public void BackPage()
    {
        if (indexCurrent > 0)
        {
            indexCurrent--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        if (bookPages.Length == 0) return;

        basePageLocal.sprite = bookPages[indexCurrent];

        if (buttonBack != null) buttonBack.interactable = (indexCurrent > 0);
        if (buttonNext != null) buttonNext.interactable = (indexCurrent < bookPages.Length - 1);
    }
}