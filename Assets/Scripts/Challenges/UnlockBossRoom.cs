using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class UnlockBossRoom : MonoBehaviour
{
    [Header("Panels UI")]
    public GameObject passwordPanel;
    public GameObject devicePanel;
    public GameObject panelDialogueJoana;

    [Header("Input")]
    public TMP_InputField input;

    [Header("Puzzle Visual Lock (Panel que cobre a sala)")]
    public GameObject lockObject;

    [Header("Pulse do Device")]
    public PulseOutline pulse;
    private bool unlocked = false;

    [Header("Fade Global (Animator no Panel preto da tela inteira)")]
    public Animator fadeAnimator;

    public GameObject panelBlack;
    public LockObjectInteraction lockInteraction;

    [Header("Cena Boss")]
    public string bossSceneName = "BossRoom";

    private string correctPassword = "1541";
    private bool isTransitioning = false;


    public void OpenPasswordPanel()
    {
        CanvasManager.Instance.OpenPanel(passwordPanel.name);

        if (pulse != null)
            pulse.StartPulsing();
    }

    public void ClosePasswordPanel()
    {
        CanvasManager.Instance.ClosedPanel(passwordPanel.name);

        if (pulse != null)
            pulse.StopPulsing();

        if (lockInteraction != null)
            lockInteraction.isUnlocked = true;
    }

    public void OpenDevicePanel()
    {
        CanvasManager.Instance.OpenPanel(devicePanel.name);
    }

    public void CloseDevicePanel()
    {
        CanvasManager.Instance.ClosedPanel(devicePanel.name);

        if (panelDialogueJoana != null)
            CanvasManager.Instance.ClosedPanel(panelDialogueJoana.name);
    }

    public void PressKey(string value)
    {
        input.text += value;
    }

    public void ClearInput()
    {
        input.text = "";
    }

    public void PressEnter()
    {
        if (input.text == correctPassword)
        {
            StartCoroutine(SuccessRoutine());
        }
        else
        {
            input.text = "ACESSO NEGADO";
            StartCoroutine(ClearAfterDelay());
        }
    }

    IEnumerator SuccessRoutine()
    {
        input.text = "ACESSO CONCEDIDO";
        lockObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        unlocked = true;

        CloseDevicePanel();
    }

    IEnumerator ClearAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        ClearInput();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && unlocked && !isTransitioning)
        {   
            isTransitioning = true;
            StartCoroutine(BossTransitionRoutine());
        }
    }

    IEnumerator BossTransitionRoutine()
{
    isTransitioning = true;
    CanvasManager.Instance.OpenPanel(panelBlack.name);

    fadeAnimator.SetTrigger("FadeOut");

    yield return new WaitForSeconds(0.8f);

    //CanvasManager.Instance.OpenPanel(panelBlack.name);
    SceneManager.LoadScene("BossRoom");

    yield return new WaitForSeconds(0.2f);

    //fadeAnimator.SetTrigger("FadeIn");

    isTransitioning = false;
}
}

