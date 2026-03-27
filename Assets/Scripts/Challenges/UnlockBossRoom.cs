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
    public LockObjectInteraction lockInteraction;


    [Header("Cena Boss")]
    public string bossSceneName = "IntroductionScene";

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

    // libera o objeto interativo corretamente
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

        yield return new WaitForSeconds(3f);

        if (lockObject != null)
            lockObject.SetActive(false);

        unlocked = true;

        CloseDevicePanel();
    }

    IEnumerator ClearAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        ClearInput();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!unlocked) return;
        if (isTransitioning) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(BossTransitionRoutine());
        }
    }

    IEnumerator BossTransitionRoutine()
    {
        isTransitioning = true;

        // Fade OUT
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(2f);

        // Carrega cena Boss
        SceneManager.LoadScene(bossSceneName);

        yield return null;

        // Fade IN
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("FadeIn");
    }
}