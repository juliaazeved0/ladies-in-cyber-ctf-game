using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class UnlockBossRoom : MonoBehaviour
{
   [Header("Panels")]
    public GameObject passwordPanel;   
    public GameObject devicePanel;     
    [Header("Input")]
    public TMP_InputField input;

    public GameObject panelDialogueJoana;

    [Header("Puzzle")]
    public GameObject lockObject;
    public PulseOutline pulse;

    [Header("Boss Trigger")]
    public string bossSceneName = "IntroductionScene";
    public Animator fadeAnimator;

    private string correctPassword = "1541";
    private bool unlocked = false;


    public void OpenPasswordPanel()
    {
        CanvasManager.Instance.OpenPanel(passwordPanel.name);

        if (pulse != null)
            pulse.StartPulsing();
    }

    public void ClosePasswordPanel()
{
    CanvasManager.Instance.ClosedPanel(passwordPanel.name);

    // libera interação com o device depois do diálogo
    if (lockObject != null)
    {
        LockObjectInteraction lockInteraction = lockObject.GetComponent<LockObjectInteraction>();

        if (lockInteraction != null)
        {
            lockInteraction.isUnlocked = true;
        }
    }
}


    public void OpenDevicePanel()
    {
        CanvasManager.Instance.OpenPanel(devicePanel.name);
    }

    public void CloseDevicePanel()
    {
        CanvasManager.Instance.ClosedPanel(devicePanel.name);
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
        input.text = "ACESSO CONCEDIDO";
        UnlockBossRoomDoor();
    }
    else
    {
        input.text = "ACESSO NEGADO";
        StartCoroutine(ClearAfterDelay());
    }
}

IEnumerator ClearAfterDelay()
{
    yield return new WaitForSeconds(3f);
    ClearInput();
}

    void UnlockBossRoomDoor()
    {
        unlocked = true;

        if (lockObject != null)
            lockObject.SetActive(false);

        CanvasManager.Instance.ClosedPanel(devicePanel.name);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!unlocked) return;

        if (other.CompareTag("Player"))
        {
            EnterBossScene();
        }
    }

    void EnterBossScene()
    {
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("Fade");

        Invoke(nameof(LoadBossScene), 2f);
    }

    void LoadBossScene()
    {
        SceneManager.LoadScene(bossSceneName);
    }
}
